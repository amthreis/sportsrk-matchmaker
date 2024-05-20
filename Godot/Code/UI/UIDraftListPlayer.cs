using Godot;
using SRkMatchmaker.Code.Draft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SRkMatchmaker
{
    public partial class UIDraftListPlayer : Control
    {
        public Label LbPick => GetNode<Label>("Player");
        public Label LbPos => GetNode<Label>("Pos");
        public Label LbTier => GetNode<Label>("Tier");
        public Label LbType => GetNode<Label>("Type");

        public void SetPick(DPick p)
        {
            Modulate = new Color(1, 1, 1, 1);

            LbPick.Uppercase = false;
            LbPick.Text = p.Name;

            //LbPos.Uppercase = false;
            //LbPos.Text = $"{p.Type}";
            //LbPos.SelfModulate = p.GetPosColor();

            LbTier.Uppercase = false;
            LbTier.Text = $"{ p.Tier }";
            LbTier.SelfModulate = p.GetTierColor();

            LbType.Uppercase = false;
            LbType.Text = $" { p.Type }";
            LbType.SelfModulate = p.GetTypeColor();
        }

        public void Disable()
        {
           // LbPlayer.SelfModulate = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }
}
