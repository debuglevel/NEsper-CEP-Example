using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CEP.Common;

namespace CEP.Common.Simulations.Car
{
    [DataContract]
    public class TireSensor : Sensor, IEsperEvent
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DataMember]
        public TirePosition Position { get; private set; }

        [DataMember]
        public double Pressure { get; private set; }

        public TireSensor(object identifier, TirePosition position) : base (identifier)
        {
            this.Position = position;
            this.Pressure = Utils.NormalRandom.Next(2.0, 0.2, 1.8, 2.2);
        }

        protected override void update()
        {
            double change = Utils.NormalRandom.Next(0, 0.1, 0, null);
            this.Pressure -= change;

            if (this.Pressure < 0)
            {
                this.Pressure = 0;
            }
        }
    }
}
