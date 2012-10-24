using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("create service...");
            var instanceContext = new InstanceContext(new callback());
            var service = new ServiceReference1.SimulationInformationServiceClient(instanceContext);
            service.Open();

            Console.WriteLine("ping server boolean");
            var result = service.PingServerBoolean();
            Console.WriteLine("done: "+result);

            Console.WriteLine("ping server void");
            service.PingServerVoid();
            Console.WriteLine("done");

            //Console.WriteLine("ping server void and ping back");
            //result = service.PingServerBooleanAndPingBack();
            //Console.WriteLine("done: "+result);

            Console.WriteLine("subscribe to sensor data");
            result = service.SubscribeSensorData();
            Console.WriteLine("done: " + result);
           

            Console.ReadLine();
        }
    }
}
