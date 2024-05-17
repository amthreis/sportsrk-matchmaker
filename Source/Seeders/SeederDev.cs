using SRkMatchmakerAPI.Framework;

namespace SRkMatchmakerAPI.Seeders;

public static class SeederDev
{
	public static List<Player> Make100Players()
	{
		Faker.RandomNumber.SetSeed(1224);
		RNG.SetSeed(1224);

		var players = new List<Player>();

		for(var i = 0; i < 100; i ++)
		{
			var p = new Player(new Visus.Cuid.Cuid2().ToString(), Faker.Internet.Email(), RNG.RandiRange(600, 4500), Helper.ChooseFromEnum<PlayerPos>());
			players.Add(p);
		}

		return players;
	}
}
