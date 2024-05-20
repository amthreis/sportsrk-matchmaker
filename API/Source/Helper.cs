using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRkMatchmakerAPI.Framework;

namespace SRkMatchmakerAPI
{
    public static class Helper
    {
       

        public static string RdDiv1k(this int v, int digits)
        {
            return $"{Math.Round(v / 1000f, digits).ToString(CultureInfo.InvariantCulture)}k";
        }
        public static string RdStr(this float v, int digits)
        {
            return $"{Math.Round(v, digits).ToString(CultureInfo.InvariantCulture)}";
        }
        public static string RdPerc(this float v, int digits)
        {
            return $"{Math.Round(v * 100f, digits).ToString(CultureInfo.InvariantCulture)}";
        }

        public static T? ChooseFromEnum<T>()
        {
            var values = Enum.GetValues(typeof(T));

            var i = RNG.RandiRange(0, values.Length - 1);
            return (T?)values.GetValue(i);
        }
    }
}
