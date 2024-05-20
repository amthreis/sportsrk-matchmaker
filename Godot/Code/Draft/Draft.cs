using Godot;
using SRkMatchmaker.Code.Draft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.Projection;

namespace SRkMatchmaker
{
    public partial class Draft : Node
    {
        [Export] VBoxContainer uiPlayerList;
        [Export] PackedScene psListPlayer;

        List<DPick> picks = new List<DPick>();
        Dictionary<(DPickTier Tier, DPickType Type), List<DPick>> pickTTs = new ();

        const int PicksPerTeam = 18;
        const int Teams = 15;

        static readonly Dictionary<DPickType, int> PicksPerType = new()
        {
            { DPickType.DEF, 6 },
            { DPickType.MID, 5 },
            { DPickType.ATT, 5 }
        };



        public override void _Ready()
        {
            Faker.RandomNumber.SetSeed(1224);

            var pickCount = 0;
            var normalPicks = new[] { DPickType.DEF, DPickType.MID, DPickType.ATT };
            var gkTiers = new[] { DPickTier.A, DPickTier.B };

            //int gkps = PicksPerType[DPickType.GKP] * Teams; // 2 * 30 =  60
            int defs = PicksPerType[DPickType.DEF] * Teams; // 7 * 30 = 210
            int mids = PicksPerType[DPickType.MID] * Teams; // 6 * 30 = 180
            int atts = PicksPerType[DPickType.ATT] * Teams; // 5 * 30 = 150
            
            foreach(var tier in gkTiers)
            {
                var list = new List<DPick>();

                for(var i = 0; i < Teams; i++)
                {
                    var pick = new DPick(Faker.Internet.UserName(), DPickType.GKP, tier);
                    list.Add(pick);

                    var obj = psListPlayer.Instantiate<UIDraftListPlayer>();
                    obj.SetPick(pick);

                    uiPlayerList.AddChild(obj);

                    //playerListItem.Add(p, obj);
                    pickCount++;
                }

                pickTTs.Add((tier, DPickType.GKP), list);
            }

            foreach (var tier in Enum.GetValues<DPickTier>()) 
            {
                foreach (var type in normalPicks)
                {
                    var list = new List<DPick>();

                    for(var i = 0; i < PicksPerType[type] * normalPicks.Length; i++)
                    {
                        var pick = new DPick(Faker.Internet.UserName(), type, tier);
                        list.Add(pick);

                        var obj = psListPlayer.Instantiate<UIDraftListPlayer>();
                        obj.SetPick(pick);

                        uiPlayerList.AddChild(obj);

                        //playerListItem.Add(p, obj);
                        pickCount++;
                    }

                    pickTTs.Add((tier, type), list);
                }
            }

            GD.Print("Teams: ", Teams);
            GD.Print("Picks: ", pickCount, " (", pickCount / Teams,"/t)");

        }

        void GeneratePlayerList()
        {
        }
    }
}
