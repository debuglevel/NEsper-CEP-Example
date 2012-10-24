using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEP.Common.Simulations.Car;
using CEP.Common.Utils;
using CEP.EventGenerators.EventReceiverService;

namespace CEP.EventGenerators.Simulations
{
    class CarSimulator
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<Car> Cars = new List<Car>();
        List<String> cityIdentifiers = new List<String>() { "KA", "B", "M", "VS", "TUT", "RW", "BAD", "S" };
        int countCars = 0;

        public event CEP.Common.Simulations.Sensor.SensorUpdatedHandler ObjectUpdated;

        public CarSimulator(int countCars)
        {
            this.countCars = countCars;
        }

        public void Start()
        {
            for (int numCar = 0; numCar < countCars; numCar++)
            {
                var car = new Car(createRandomCarIdentifier());

                foreach (var sensor in car.Sensors)
                {
                    sensor.SensorUpdated += ObjectUpdated;
                    sensor.Start();
                }

                Cars.Add(car);
            }
        }

        private string createRandomCarIdentifier()
        {
            var rand = new Random();
            return String.Format("{0}-{1} {2}", cityIdentifiers[rand.Next(cityIdentifiers.Count())], RandomString.Next(2), rand.Next(1,1000));
        }
    }
}
