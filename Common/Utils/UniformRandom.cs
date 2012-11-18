using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEP.Common.Utils
{
    public static class UniformRandom
    {
        private static Random rand = new Random();
        public static Random Rand
        {
            get
            {
                return rand;
            }
        }
    }
}
