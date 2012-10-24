using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace CEP.EventGenerators
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            Log.Info("Event Generator starting up...");

            Log.Info("Starting TCP Adaptor...");
            var tcpAdaptor = new TcpAdaptor();

            Log.Info("Starting Car Simulator...");
            var carSimulator = new Simulations.CarSimulator(10);
            carSimulator.ObjectUpdated += tcpAdaptor.SendEvent;
            carSimulator.Start();

            Console.ReadLine();
        }
    }
}
