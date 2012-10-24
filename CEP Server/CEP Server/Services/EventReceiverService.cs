using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CEP.Common.Simulations;
using CEP.Server;
using com.espertech.esper.client;
using log4net;

namespace CEP.Server.Adaptor.TCP
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class EventReceiverService : IEventReceiverService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        EPServiceProvider epService = EPServiceProviderManager.GetDefaultProvider();

        int eventsReceivedCount = 0;

        public EventReceiverService()
        {
            Log.Info("Created Service Instance of EventReceiverService");
        }

        /// <summary>
        /// Sends an event to the CEP Server
        /// </summary>
        /// <param name="obj">the event</param>
        public void SendEvent(Sensor obj)
        {
            Log.DebugFormat("Received Event #{1} at {2}: {0}", obj.ToString(), ++eventsReceivedCount, this.GetHashCode());

            epService.EPRuntime.SendEvent(obj);
        }

    }
}
