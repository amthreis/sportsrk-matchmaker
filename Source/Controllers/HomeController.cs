using Microsoft.AspNetCore.Mvc;
using SRkMatchmakerAPI.Framework;
using SRkMatchmakerAPI.Framework.DTO;
using SRkMatchmakerAPI.Persistence;

namespace SRkMatchmakerAPI.Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    readonly MatchmakerDbContext ctx;

    public HomeController()
    {
        ctx = new MatchmakerDbContext();
    }

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Summaries);
    }

    [HttpGet("players/get")]
    public IActionResult GetAllPlayers()
    {
        return Ok(ctx.Players);
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
