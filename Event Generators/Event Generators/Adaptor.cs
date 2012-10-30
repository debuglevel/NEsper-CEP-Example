using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEP.Common.Simulations;
using CEP.EventGenerators.EventReceiverService;

namespace CEP.EventGenerators
{
    public class Adaptor
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        EventReceiverServiceClient service = new EventReceiverService.EventReceiverServiceClient();

        int eventsSentCount = 0;

        object reconnectLock = new object();

        public void SendEvent(Sensor sensor)
        {
            try
            {
                service.SendEvent(sensor);
                //service.SendEventAsync(sensor);

                Log.DebugFormat("Send Event #{1}: {0}", sensor.ToString(), ++eventsSentCount);
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Sending Event failed: {0}", e.Message);

                lock (reconnectLock)
                {
                    if (service.State == System.ServiceModel.CommunicationState.Faulted)
                    {
                        Log.Warn("Creating new service client");
                        service = new EventReceiverService.EventReceiverServiceClient();
                    }
                }
            }
        }
    }
}
