using Godot;
using SRkMatchmakerAPI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SRkMatchmaker
{
    public partial class UIListPlayer : Control
    {
        public Label LbPlayer => GetNode<Label>("Player");
        public Label LbPos => GetNode<Label>("Pos");
        public Label LbMMR => GetNode<Label>("MMR");
        public Label LbMMRRd => GetNode<Label>("MMR/Rd");
        public Label LbMMRFull => GetNode<Label>("MMR/Full");

        public void SetPlayer(Player p)
        {
            Modulate = new Color(1, 1, 1, 1);

            LbPlayer.Uppercase = false;
            LbPlayer.Text = p.Username;

            LbPos.Uppercase = false;
            LbPos.Text = $"{p.Pos}";
            LbPos.SelfModulate = new Color(p.GetPosColor());

            LbMMRRd.Uppercase = false;
            LbMMRRd.Text = $"{ p.MMR.RdDiv1k(1) }";

            LbMMRFull.Uppercase = false;
            LbMMRFull.Text = $" ({ p.MMR })";
            LbMMRFull.Modulate = new Color(0.8f, 0.8f, 0.8f, 1f);
        }

        public void Disable()
        {
            LbPlayer.SelfModulate = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }
}
