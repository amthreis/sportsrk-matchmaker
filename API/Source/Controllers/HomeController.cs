using Microsoft.AspNetCore.Mvc;
using SRkMatchmakerAPI.Framework;
using SRkMatchmakerAPI.Framework.DTO;
using SRkMatchmakerAPI.Framework.Mappers;
using SRkMatchmakerAPI.Persistence;
using SRkMatchmakerAPI.Seeders;


namespace SRkMatchmakerAPI.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    readonly MatchmakerDbContext ctx;
    //readonly MatchmakingTool mmTool;

    // readonly string redisHost;
    // readonly string redisPort;

    public HomeController()
    {
        ctx = new MatchmakerDbContext();

        //redis = new ConnectionMultiplexer();
        //mmTool = new MatchmakingTool();
    }

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpPost("/")]
    public async Task<IActionResult> MakeGames(MakeGamesDTO mgDTO)
    {
        var response = await Task.Run(() =>
        {
            var mmTool = new MatchmakingTool(mgDTO.Players);
            return mmTool.Start();
        });

        return Ok(response);
    }

    // [HttpPost("/make")]
    // public async Task<IActionResult> MakeGame(MakeGamesDTO mgDTO)
    // {
    //      //   var match = await connection.GetDatabase().ListRightPopAsync("football:matches:tbp");
    //     var mmTool = new MatchmakingTool(mgDTO.Players);

    //     var response = await Task.Run(() =>
    //     {
    //         var a = mmTool.Start();

    //         // if (!mgDTO.Defer)
    //         // {
    //         //     redis.conn
    //         // }
            
    //         return a;
    //     });

    //     return Ok(response);
    // }
    
    [HttpPost("makeseed")]
    public async Task<IActionResult> MakeGame()
    {
        var response = await Task.Run(() =>
        {
            var players = SeederDev.Make100Players();
            var mmTool = new MatchmakingTool(players.Select(p => p.ToPlayerDTO()).ToArray());

            return mmTool.Start();
        });

        return Ok(response);
    }


    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new string[] { "AA", "b", "ASIDASOD" });
    }

    [HttpGet("players/get")]
    public IActionResult GetAllPlayers()
    {
        return Ok(ctx.Players);
    }

    [HttpPost("cuidTest")]
    public IActionResult AddPlayer(UserDTO u)
    {
        return Ok(new 
        { 
            Message = $"Works!",  
            u
        });
    }

    [HttpPost("players/add")]
    public IActionResult AddPlayer(PlayerDTO p)
    {
        System.Diagnostics.Debug.WriteLine($"Players before: {ctx.Players.Count}");
        if (ctx.Players.ContainsKey(p.User.Email)) 
        {
            return BadRequest(new { Message = $"Player({p.User.Email}) is already in!" });
        }

        var ply = new Player(p.User.Id, p.User.Email, p.MMR, p.Pos);

        ctx.Players.Add(ply.Email, ply);

        return Ok(new 
        { 
            Message = $"Player({ply.Email}) added succesfully!",  
            ctx.Players.Count 
        });
    }
}
