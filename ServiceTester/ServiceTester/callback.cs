using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTester
{
    class callback : ServiceReference1.ISimulationInformationServiceCallback
    {
        public callback()
        {
            Console.WriteLine("Created Instance of ISimulationInformationServiceCallback");
        }

        void ServiceReference1.ISimulationInformationServiceCallback.PingDashboardVoid()
        {
            Console.WriteLine("got ping from server");
        }

        void ServiceReference1.ISimulationInformationServiceCallback.ReceiveSensorData(ServiceReference1.Sensor sensor)
        {
            throw new NotImplementedException();
        }

        void ServiceReference1.ISimulationInformationServiceCallback.ReceiveOverallAverageSpeed(double? overallAverageSpeed)
        {
            throw new NotImplementedException();
        }

        void ServiceReference1.ISimulationInformationServiceCallback.ReceiveIndividualAverageSpeed(string identifier, double? individualAverageSpeed)
        {
            throw new NotImplementedException();
        }
    }
}
