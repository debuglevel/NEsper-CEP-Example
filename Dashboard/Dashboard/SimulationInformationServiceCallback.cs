using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceTester.ServiceReference1;

namespace CEP.Dashboard
{
    class SimulationInformationServiceCallback : ISimulationInformationServiceCallback
    {
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

        void ISimulationInformationServiceCallback.ReceiveIndividualAverageSpeed(string identifier, double? individualAverageSpeed)
        {
            Debug.WriteLine("Receive IndividualAverageSpeed");

            var kvp = Data.IndividualAverageSpeed.FirstOrDefault(x => x.Key == identifier);
            Data.IndividualAverageSpeed.Remove(kvp);

            Data.IndividualAverageSpeed.Add(new KeyValuePair<string, double?>(identifier, individualAverageSpeed));
        }

        void ISimulationInformationServiceCallback.PingDashboardVoid()
        {
            Debug.WriteLine("Receive Ping");
        }

    }
}
