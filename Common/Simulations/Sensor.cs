using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CEP.Common.Simulations.Car;

namespace CEP.Common.Simulations
{
    [DataContract]
    [KnownType(typeof(SpeedSensor))]
    [KnownType(typeof(LocationSensor))]
    [KnownType(typeof(TireSensor))]
    public abstract class Sensor
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void SensorUpdatedHandler(Sensor sensor);
        public event SensorUpdatedHandler SensorUpdated;

        [DataMember]
        public object Identifier { get; private set; }

        public Sensor(object identifier)
        {
            this.Identifier = identifier;
        }

        public void Start()
        {
            Log.DebugFormat("Starting Update-Loop-Thread: {0}", this.ToString());

            var threadStart = new ThreadStart(loopUpdate);
            var thread = new Thread(threadStart);
            thread.Start();
        }

        public override string ToString()
        {
            return base.ToString() + "["+this.Identifier+"]";
        }

        private void loopUpdate()
        {
            while (true)
            {
                Thread.Sleep(Utils.NormalRandom.NextInt(1000, 200, 300, 2000));
                this.update();
            }
        }

        protected virtual void update()
        {
            Log.DebugFormat("Updated Sensor: {0}", this.ToString());

            if (SensorUpdated != null)
            {
                SensorUpdated(this);
            }
        }
    }
}
