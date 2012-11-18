using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEP.Common.Utils
{
    public static class NormalRandom
    {
        private static Random rand = UniformRandom.Rand;

        public static double Next(double mean, double standardDeviation)
        {
            double u1 = rand.NextDouble(); //these are uniform(0,1) random doubles
            double u2 = rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal = mean + standardDeviation * randStdNormal; //random normal(mean,stdDev^2)
            return randNormal;
        }

        public static int NextInt(double mean, double standardDeviation, int? lowerBoundary, int? upperBoundary)
        {
            //XXX: actually, we should check boundaries again after converting to integer
            return Convert.ToInt32(Next(mean, standardDeviation, lowerBoundary, upperBoundary));
        }

        public static double Next(double mean, double standardDeviation, double? lowerBoundary, double? upperBoundary)
        {
            if (lowerBoundary > upperBoundary)
            {
                throw new ArgumentOutOfRangeException("lower boundary must be smaller then upper boundary");
            }

            double random = Next(mean, standardDeviation);

            if (lowerBoundary != null && random < lowerBoundary)
            {
                random = (double)lowerBoundary;
            }

            if (upperBoundary != null && random > upperBoundary)
            {
                random = (double)upperBoundary;
            }

            return random;
        }
    }
}
