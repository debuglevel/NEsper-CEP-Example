using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEP.Common.Simulations.Car
{
    public class Car
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<Sensor> sensors = new List<Sensor>();
        public IList<Sensor> Sensors
        {
            get { return sensors.AsReadOnly(); }
        }

        public object Identifier { get; private set; }
        
        public Car(object identifier)
        {
            this.Identifier = identifier;

            sensors.Add(new LocationSensor(Identifier));
            sensors.Add(new SpeedSensor(Identifier));

            foreach (TirePosition tirePosition in Enum.GetValues(typeof(TirePosition)))
            {
                sensors.Add(new TireSensor(identifier, tirePosition));
            }
        }
    }
}
