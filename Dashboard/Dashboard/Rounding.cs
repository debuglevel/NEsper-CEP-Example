using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEP.Dashboard
{
    static class Rounding
    {
        private static int digits = 2;

        public static double Round(double x)
        {
            return Math.Round(x, Rounding.digits);
        }

        public static double? Round(double? x)
        {
            if (x == null)
            {
                return null;
            }

            return Math.Round((double)x, Rounding.digits);
        }
    }
}
