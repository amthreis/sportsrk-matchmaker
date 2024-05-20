using System.Diagnostics;
using SRkMatchmakerAPI.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DotNetEnv.Env.Load();

var port = Environment.GetEnvironmentVariable("PORT") ?? "9000";

builder.WebHost.UseUrls($"http://*:{ port }");



Console.WriteLine($"Listening on port {port}...");

builder.Services.AddSingleton<MatchmakerDbContext>();
//builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}

app.UseAuthorization();

app.MapControllers();

app.Run();