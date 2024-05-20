using Godot;
using SRkMatchmakerAPI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRkMatchmaker
{
    public partial class UIMatch : PanelContainer
    {
        [Export] VBoxContainer uiHomePlayersList;
        [Export] VBoxContainer uiAwayPlayersList;
        [Export] RichTextLabel uiInfo;

        bool isOpen = false;

        public override void _GuiInput(InputEvent ev)
        {
            if (ev is InputEventMouseButton mb)
            {
                if (mb.ButtonIndex == MouseButton.Left && mb.Pressed ) 
                {
                    isOpen = !isOpen;

                    GetNode<Control>("Content/Closed").Visible = !isOpen;
                    GetNode<Control>("Content/Open").Visible = isOpen;
                }
            }
        }

        public void SetMatch(Match match)
        {
            uiInfo.Text = GetMatchMMRInfo(match);

            for (var i = 0; i < 11; i++)
            {
                UpdatePlayer(uiHomePlayersList, match.Home, i);
            }

            for (var i = 0; i < 11; i++)
            {
                UpdatePlayer(uiAwayPlayersList, match.Away, i);
            }

            void UpdatePlayer(VBoxContainer ctr, List<Player> team, int i)
            {
                var chd = ctr.GetChild(i);

                //chd.GetNode<Label>("Title").Text = $"{Match.Formation[i]}";
                //chd.GetNode<Label>("Title").SelfModulate = Player.PosColor[Match.Formation[i]];

                chd.GetNode<Label>("Name").Text = $"{team[i].Username}";
                chd.GetNode<RichTextLabel>("Info").Text = GetPlayerInfo(team[i], i);

                chd.GetNode<Label>("Pos").Text = $"{Match.Formation[i]}";
                chd.GetNode<Label>("Pos").SelfModulate = new Color(Player.PosColor[Match.Formation[i]]);
            }

            string GetPlayerInfo(Player p, int posId)
            {
                return $"[color=#9c9c9c](mmr: [color=#fff]{ p.GetMMRInPos(Match.Formation[posId]).RdDiv1k(1) }[/color], pos: [color={ p.GetPosColor() }]{p.Pos}[/color])[/color]";
            }

            string GetMatchMMRInfo(Match m)
            {
                return $"[color=#9c9c9c]players' mmr (min/avg/max): [color=#fff]{ m.MMR.Min.RdDiv1k(1)} [color=#9c9c9c]/[/color] {m.MMR.Avg.RdDiv1k(1)} [color=#9c9c9c]/[/color] {m.MMR.Max.RdDiv1k(1)}[/color]\nteams' mmr, home x away: [color=#fff]{m.MMR.HomeAvg.RdDiv1k(1)}[/color] x [color=#fff]{m.MMR.AwayAvg.RdDiv1k(1)}[/color]\nteam's mmr in pos, HxA: [color=#fff]{m.MMR.PosHomeAvg.RdDiv1k(1)}[/color] x [color=#fff]{m.MMR.PosAwayAvg.RdDiv1k(1)}[/color]";
            }
        }
    }
}
