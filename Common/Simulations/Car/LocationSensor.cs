using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CEP.Common;
using CEP.Common.Utils;

namespace CEP.Common.Simulations.Car
{
    [DataContract]
    public class LocationSensor : Sensor, IEsperEvent
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DataMember]
        public double X { get; private set; }

        [DataMember]
        public double Y { get; private set; }

        public LocationSensor(object identifier) : base(identifier)
        {
            var rand = UniformRandom.Rand;
            this.X = rand.Next(0, 101);
            this.Y = rand.Next(0, 101);
        }

        protected override void update()
        {
            X += Utils.NormalRandom.Next(0, 1);
            Y += Utils.NormalRandom.Next(0, 1);

            if (X < 0)
            {
                X = 0;
            }
            else if (X > 100)
            {
                X = 100;
            }

            if (Y < 0)
            {
                Y = 0;
            }
            else if (Y > 100)
            {
                Y = 0;
            }

            base.update();
        }
    }
}
