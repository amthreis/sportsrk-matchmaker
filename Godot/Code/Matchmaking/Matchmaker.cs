using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

using SRkMatchmakerAPI.Framework;

namespace SRkMatchmaker
{
    public partial class Matchmaker : Node
    {
        [Export] PackedScene psListPlayer;
        [Export] PackedScene psListMatch;
        [Export] VBoxContainer uiPlayerList;
        [Export] VBoxContainer uiMatchList;
        [Export] VBoxContainer uiMatchListALGO2;

        List<Player> players;
        SortedSet<Player> queue;
        Dictionary<PlayerPos, SortedSet<Player>> posQueue = new Dictionary<PlayerPos, SortedSet<Player>>();


        SortedSet<Player> queueALGO2;
        Dictionary<PlayerPos, SortedSet<Player>> posQueueALGO2 = new Dictionary<PlayerPos, SortedSet<Player>>();

        public static RandomNumberGenerator RNG;

        Dictionary<Player, UIListPlayer> playerListItem = new Dictionary<Player, UIListPlayer>();

        public override void _Ready()
        {
            Faker.RandomNumber.SetSeed(1224);

            RNG = new RandomNumberGenerator();
            RNG.Seed = 1224;

            players = new List<Player>();

            for(var i = 0; i < 100; i ++)
            {
                var p = new Player("", Faker.Internet.Email(), RNG.RandiRange(600, 4500), Helper.ChooseFromEnum<PlayerPos>());
                players.Add(p);
            }


            players = players.OrderByDescending(p => 1_000_000 + p.MMR).ToList();

            GeneratePlayerList();

            foreach(Button b in GetNode("HUD/Actions").GetChildren())
            {
                b.Pressed += () => OnActionPressed(b);
            }

            queue = new SortedSet<Player>(players, new PlayerComparer());

            foreach(PlayerPos pos in System.Enum.GetValues(typeof(PlayerPos)))
            {
                posQueue.Add(pos, new SortedSet<Player>(players, new PlayerComparerByPos(pos)));
                GD.Print($"best {pos}: {posQueue[pos].First().Username}");
            }

            queueALGO2 = new SortedSet<Player>(players, new PlayerComparer());

            foreach (PlayerPos pos in System.Enum.GetValues(typeof(PlayerPos)))
            {
                posQueueALGO2.Add(pos, new SortedSet<Player>(players, new PlayerComparerByPos(pos)));
                //GD.Print($"best {pos}: {posQueue[pos].First().Username}");
            }

        }

        void OnActionPressed(Button b)
        {
            switch(b.Name) 
            {
                case "Make1Match":
                    {
                        Make1Game(queue, posQueue, uiMatchList);
                        //Make1GameALGO2(queueALGO2, posQueueALGO2, uiMatchListALGO2);

                    }
                    break;
            }
        }

        void Make1GameALGO2(SortedSet<Player> Q, Dictionary<PlayerPos, SortedSet<Player>> posQ, VBoxContainer matchList)
        {
            var lobby = new Lobby();

            GD.Print("-----------------------Searching...");
            while (lobby.Players.Count < 22 && Q.Count > 0)
            {

                var pos = Match.Formation[lobby.Players.Count % 11];
                var ply = PopFromPosQueue(pos);

                if (ply != null)
                {
                    lobby.Take(ply, pos);
                    //matchPlayers.Add(ply);
                }
                else break;
            }

            Player PopFromPosQueue(PlayerPos pos)
            {
                Player selected = null;

                foreach (var q in posQ[pos])
                {
                    if (q.Pos != pos)
                    {
                        var posSpotsAvailable = lobby.SpotsAvailable[q.Pos];

                        if (posSpotsAvailable > 0)
                        {
                            GD.Print($"<{ q.Username }> trying to take me as a {pos}, but since there are {posSpotsAvailable} for {q.Pos}, I'll pass.");
                            continue;
                        }
                    }

                    //if (q.GetMMRInPos(pos) < 100)
                    //{
                    //    continue;
                    //}


                    selected = q;
                    break;
                }

                if (selected != null)
                {
                    //GD.Print($"Pop from queue({pos}): {selected.Username}");
                    //queue.Remove(selected);
                    RemoveFromQueues(selected);

                }

                return selected;
            }

            void RemoveFromQueues(Player p)
            {
                Q.Remove(p);

                foreach (PlayerPos pos in System.Enum.GetValues(typeof(PlayerPos)))
                {
                    posQ[pos].Remove(p);
                }
            }

            Player PopFromQueue()
            {
                Player selected = null;

                foreach (var q in queue)
                {
                    selected = q;
                    break;
                }

                if (selected != null)
                {
                    GD.Print($"Pop from queue: {selected.Username}");
                    Q.Remove(selected);
                }

                return selected;
            }

            GD.Print($"Found {lobby.Players.Count}/22");

            if (lobby.Players.Count < 22)
            {

                GD.Print("Couldn't make a game!");

                GD.Print("----spots available");
                foreach(var p in lobby.SpotsAvailable)
                {
                    GD.Print($"{p.Key}: {p.Value}");
                }
            }
            else
            {
                foreach (var p in lobby.Players)
                {
                    playerListItem[p].Disable();
                }

                var m = Match.FromListOfPlayers(lobby.Players);

                GenerateMatchUI(m, matchList);
            }
        }


        void Make1Game(SortedSet<Player> Q, Dictionary<PlayerPos, SortedSet<Player>> posQ, VBoxContainer matchList)
        {
            var matchPlayers = new List<Player>();

            GD.Print("-----------------------Searching...");
            while(matchPlayers.Count < 22 && Q.Count > 0)
            {
                var ply = PopFromPosQueue(Match.Formation[matchPlayers.Count % 11]);

                if (ply != null)
                {
                    matchPlayers.Add(ply);
                }
                else break;
            }

            Player PopFromPosQueue(PlayerPos pos)
            {
                Player selected = null;

                foreach (var q in posQ[pos])
                {
                    selected = q;
                    break;
                }

                if (selected != null)
                {
                    GD.Print($"Pop from queue({pos}): {selected.Username}");
                    //queue.Remove(selected);
                    RemoveFromQueues(selected);

                }

                return selected;
            }

            void RemoveFromQueues(Player p)
            {
                Q.Remove(p);

                foreach (PlayerPos pos in System.Enum.GetValues(typeof(PlayerPos)))
                {
                    posQ[pos].Remove(p);
                }
            }

            Player PopFromQueue()
            {
                Player selected = null;

                foreach (var q in queue)
                {
                    selected = q;
                    break;
                }

                if (selected != null)
                {
                    GD.Print($"Pop from queue: {selected.Username}");
                    Q.Remove(selected);
                }

                return selected;
            }

            GD.Print($"Found {matchPlayers.Count}/22");

            if (matchPlayers.Count < 22)
            {
                GD.Print("Couldn't make a game!");
            }
            else
            {
                foreach(var p in matchPlayers)
                {
                    playerListItem[p].Disable();
                }

                var m = Match.FromListOfPlayers(matchPlayers);

                GenerateMatchUI(m, matchList);
            }
        }

        void GeneratePlayerList()
        {
            foreach(var p in players)   
            {
                var obj = psListPlayer.Instantiate<UIListPlayer>();
                obj.SetPlayer(p);

                uiPlayerList.AddChild(obj);

                playerListItem.Add(p, obj);
            }
        }

        List<Match> matches;

        void GenerateMatchUI(Match m, VBoxContainer matchList)
        {
            //foreach (var m in matches)
            //{
                var obj = psListMatch.Instantiate<UIMatch>();
                obj.SetMatch(m);

                matchList.AddChild(obj);
                obj.Show();
           // }
        }
    }
}
