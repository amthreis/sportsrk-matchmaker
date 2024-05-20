using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRkMatchmaker.Code.Draft
{
    public enum DPickTier { A, B, C, D, E }
    public enum DPickType { GKP, DEF, MID, ATT }

    public class DPick
    {
        public string Name;

        public DPickType Type;
        public DPickTier Tier;

        public DPick(string name, DPickType type, DPickTier tier)
        {
            Name = name;
            Type = type;
            Tier = tier;
        }

        public static Dictionary<DPickType, Color> TypeColor = new Dictionary<DPickType, Color>()
        {
            { DPickType.GKP, new Color("a79b77") },

            { DPickType.DEF, new Color("56a6cf") },

            { DPickType.MID, new Color("4c7e4d") },

            { DPickType.ATT, new Color("ff8f77")}
        };

        public static Dictionary<DPickTier, Color> TierColor = new Dictionary<DPickTier, Color>()
        {
            { DPickTier.E, new Color("4d3924") },
            { DPickTier.D, new Color("815c51") },
            { DPickTier.C, new Color("b8b7b1") },
            { DPickTier.B, new Color("d4b357") },
            { DPickTier.A, new Color("87c3cc") },
        };

        
        public Color GetTypeColor()
        {
            return TypeColor[Type];
        }

        public Color GetTierColor()
        {
            return TierColor[Tier];
        }
    }
}
