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
    public class SpeedSensor : Sensor, IEsperEvent
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DataMember]
        public double Speed { get; private set; }

        public SpeedSensor(object identifier) : base (identifier)
        {
            var rand = UniformRandom.Rand;
            this.Speed = rand.Next(40);
        }

        protected override void update()
        {
            double change = Utils.NormalRandom.Next(0, 5);
            this.Speed -= change;

            if (this.Speed < 0)
            {
                this.Speed = 0;
            }

            base.update();
        }
    }
}
