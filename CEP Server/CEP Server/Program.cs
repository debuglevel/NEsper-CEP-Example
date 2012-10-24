using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

using System.ServiceModel;
using System.ServiceModel.Description;

namespace CEP.Server
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            Log.Info("CEP Server starting up...");

            Log.Info("Starting Event Processor...");
            var eventProcessor = new EventProcessor();
            eventProcessor.CreateStatements();

            Log.Info("Starting Services...");
            var clientAdaptor = new Adaptors();
            clientAdaptor.Start();

            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        }
    }
}
