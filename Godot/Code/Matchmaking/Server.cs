using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Newtonsoft.Json.Serialization;
using System.IO;
using StackExchange.Redis;
using SRkMatchmakerAPI.Framework;
using SRkMatchmakerAPI.Framework.DTO;

namespace SRkMatchmaker
{
    public partial class Server : Node
    {
        const int OwnPort = 8586;
        const int MainPort = 5757;
        const int RedisPort = 6379;

        private static string RedisConnectionString => $"{redisHost}:{RedisPort}";
        private static ConnectionMultiplexer connection;
        private const string Channel = "football:queue";

        HttpListener listener = new HttpListener();
        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

        static string redisHost = "localhost";
        static string prefixHost = "localhost";
        static string mainHost = "localhost";

        public override void _Ready()
        {
            GD.Print("DOCKER = ", OS.HasEnvironment("DOCKER"));
            Console.WriteLine($"<ln> DOCKER = {OS.HasEnvironment("DOCKER")}");

            if (OS.HasEnvironment("DOCKER"))
            {
                redisHost = "db-redis";
                prefixHost = "*";
                mainHost = "main";
            }
            //resolver.Done += OnDoneResolving;

            listener = new HttpListener();

            listener.Prefixes.Add($"http://{prefixHost}:" + OwnPort.ToString() + "/");
            listener.Start();

            App.Log($"Listening on port {OwnPort}...");
            Receive();

        }

        async Task<bool> SendResolve(string jsonObj)
        {
            var httpClient = new System.Net.Http.HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, $"http://{mainHost}:5757/common/football/matchmake");
            //req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            req.Content = new StringContent(jsonObj, Encoding.UTF8, "application/json"); ;

            //GD.Print("Prepare to send");
            var res = await httpClient.SendAsync(req);

            App.Log($"Sent resolved match making... Code from server: {(int)res.StatusCode} ({res.StatusCode})");
            //GD.Print(res.StatusCode);

            var content = await res.Content.ReadAsStringAsync();
            App.Log(content);

            return res.StatusCode == HttpStatusCode.OK;
        }

        public void Stop()
        {
            listener.Stop();
        }

        void Receive()
        {
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
            //GD.Print("Received");
        }

        public static T Deserialized<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            Converters = { new StringEnumConverter() },
                            TypeNameHandling = TypeNameHandling.Auto,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                        });
        }

        public static string Serialized(object o, Action callback = null)
        {
            var data = JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = { new StringEnumConverter() },
                    TypeNameHandling = TypeNameHandling.Auto,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            callback?.Invoke();

            return data;
        }

        List<Match> matchesToResolve;

        

        bool isWorking = false;

        void Work()
        {
            if (connection == null) 
            {
                connection = ConnectionMultiplexer.Connect(RedisConnectionString);
            }

            isWorking = true;

           // Task.Run(TrySimAMatch);
        }

        PlayerDTO[] playerDTOs;

        void Handle(HttpListenerContext ctx, HttpListenerRequest req)
        {
            playerDTOs = GetRequestBody<PlayerDTO[]>(req);

            var mmT = new MatchmakingTool(playerDTOs);

            Respond(ctx, HttpStatusCode.OK, new { message = "OK" });

            Task.Run(mmT.Start);

            App.Log($"{playerDTOs[0].User.Email}, {playerDTOs[0].User.Id}");
        }

        //List<MatchOTD> matches;
        //List<string> unmatchedIds;

        MyResponse r;

        void OnDoneArranging(MyResponse r)
        {
            this.r = r;

            CallDeferred("SendArranged");
        }

        async void SendArranged()
        {
            App.Log("Sending results back to main app...");

            var response = await SendResolve(Serialized(r));

            App.Log($"Done. All good? {response}");
        }


        void OnDoneMaking()
        {

        }

        void ListenerCallback(IAsyncResult result)
        {
            GD.Print("Listening: ", listener.IsListening);
            if (listener.IsListening)
            {
                var context = listener.EndGetContext(result);
                var request = context.Request;

                if (request.ContentType != "application/json")
                {
                    App.Log("Request didn't send a json object.");
                }

                App.Log($"got a {request.HttpMethod}");
                if (request.HttpMethod != HttpMethod.Post.ToString())
                {
                    Respond(context, HttpStatusCode.NotFound, new { Message = "Not found" });
                    Receive();
                    return;
                }

                try
                {
                    Handle(context, request);
                    //GD.Print("re");

                }
                catch (Exception ex)
                {
                    //GD.Print("exc");
                    Respond(context, HttpStatusCode.BadRequest, new { error = ex.Message });
                }

                Receive();
            }

        }

        T GetRequestBody<T>(HttpListenerRequest req)
        {
            string text;
            using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
            {
                text = reader.ReadToEnd();
            }

            //App.Log("------ text");
            //App.Log(text);
            //App.Log("------ ");

            return Deserialized<T>(text);
        }

        void Respond(HttpListenerContext ctx, HttpStatusCode code, object? body = null)
        {
            var response = ctx.Response;
            response.StatusCode = (int)code;

            if (body != null)
            {
                string responseString = Serialized(body);

                var buffer = new byte[] { };

                response.ContentType = "application/json";

                buffer = Encoding.ASCII.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;

                var output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
