using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEP.Dashboard.SimulationInformationService;
using CEP.Dashboard.Utils;

namespace CEP.Dashboard
{
    class SimulationInformationServiceCallback : ISimulationInformationServiceCallback
    {
        public delegate void LocationChangedEventHandler(LocationPoint location);
        public event LocationChangedEventHandler LocationChanged;

        public delegate void NotificationEventHandler(string notification);
        public event NotificationEventHandler NotificationReceived;

        public SimulationInformationServiceCallback(Data data)
        {
            Data = data;
        }

        public Data Data { get; private set; }

        public SimulationInformationServiceCallback()
        {
            Debug.WriteLine("Created Instance of SimulationInformationServiceCallback");
        }

        void ISimulationInformationServiceCallback.ReceiveSensorData(Sensor sensor)
        {
            Debug.WriteLine("Receive SensorData");
        }

        void ISimulationInformationServiceCallback.ReceiveOverallAverageSpeed(double? overallAverageSpeed)
        {
            Debug.WriteLine("Receive OverallAverageSpeed");
            Data.OverallAverageSpeed = overallAverageSpeed;
        }

        void ISimulationInformationServiceCallback.PingDashboardVoid()
        {
            Debug.WriteLine("Receive Ping");
        }

        void ISimulationInformationServiceCallback.ReceiveIndividualLocation(LocationPoint point)
        {
            if (this.LocationChanged != null)
            {
                this.LocationChanged(point);
            }
        }

        void ISimulationInformationServiceCallback.ReceiveNotificationDictionary(string statementName, Dictionary<string, object> dict)
        {
            if (this.NotificationReceived != null)
            {
                this.NotificationReceived(statementName + ": " + dict.ToDebugString());
            }
        }

        void ISimulationInformationServiceCallback.ReceiveSensorChange(Dictionary<string, object> dict)
        {
            var identifier = dict["Identifier"] as string;
            var speed = dict["Speed"] as double?;
            var x = dict["X"] as double?;
            var y = dict["Y"] as double?;
            var pressure = dict["Pressure"] as double?;

            var car = this.Data.Cars.FirstOrDefault(c => c.Identifier == identifier);

            if (car == null)
            {
                car = new CarInfo();
                this.Data.Cars.Add(car);
            }

            car.Identifier = identifier;
            car.Speed = speed;
            car.X = x;
            car.Y = y;
            car.Pressure = pressure;
        }
    }
}
