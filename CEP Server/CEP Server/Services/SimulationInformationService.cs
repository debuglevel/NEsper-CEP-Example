using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using CEP.Common.Simulations;
using CEP.Server;
using com.espertech.esper.client;
using log4net;

namespace CEP.Server.Adaptor.TCP
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SimulationInformationService : ISimulationInformationService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ISimulationInformationClient client;

        public SimulationInformationService()
        {
            Log.Info("Created Service Instance of SimulationInformationService");
        }

        public Boolean SubscribeSensorData()
        {
            Log.Info("Client subscribed to get sensor data");
            client = OperationContext.Current.GetCallbackChannel<ISimulationInformationClient>();

            EPServiceProvider epService = EPServiceProviderManager.GetDefaultProvider();

            epService.EPAdministrator.GetStatement("OverallAverageSpeed").Events += OnOverallAverageSpeed;
            epService.EPAdministrator.GetStatement("IndividualAverageSpeed").Events += OnIndividualAverageSpeed;

            Log.Debug("Ping Dashboard");
            client.PingDashboardVoid();
            Log.Debug("done pinging dashboard");

            return true;
        }

        void OnIndividualAverageSpeed(object sender, UpdateEventArgs e)
        {
            var dict = e.NewEvents.FirstOrDefault().Underlying as Dictionary<String, object>;
            var avgSpeed = dict["avg(Speed)"] as double?;
            var identifier = dict["Identifier"] as string;

            try
            {
                Log.Debug("Send OnIndividualAverageSpeed Event");
                client.ReceiveIndividualAverageSpeed(identifier, avgSpeed);
            }
            catch (TimeoutException ex)
            {
                Log.Error("Sending notification timed out: " + ex.Message);
            }
            catch (CommunicationException ex)
            {
                Log.Error("Sending notification failed: " + ex.Message);
            }
        }

        void OnOverallAverageSpeed(object sender, UpdateEventArgs e)
        {
            var dict = e.NewEvents.FirstOrDefault().Underlying as Dictionary<String, object>;
            var avgSpeed = dict["avg(Speed)"] as double?;

            try
            {
                Log.Debug("Send OnOverallAverageSpeed Event");
                client.ReceiveOverallAverageSpeed(avgSpeed);
            }
            catch (TimeoutException ex)
            {
                Log.Error("Sending notification timed out: " + ex.Message);
            }
            catch (CommunicationException ex)
            {
                Log.Error("Sending notification failed: " + ex.Message);
            }
        }

        public void PingServerVoid()
        {
            Log.Info("Ping Void");
        }

        public bool PingServerBoolean()
        {
            Log.Info("Ping Boolean");
            return true;
        }

        public void PingServerVoidAndPingBack()
        {
            Log.Info("Ping Void And Ping Back");
            Log.Info("create client proxy");
            var clientproxy = OperationContext.Current.GetCallbackChannel<ISimulationInformationClient>();
            Log.Info("ping back");
            clientproxy.PingDashboardVoid();
            Log.Info("done sending ping");
            
            Log.Info("Sleeping for a while");
            Thread.Sleep(10000);
        }

        public Boolean PingServerBooleanAndPingBack()
        {
            Log.Info("Ping Void And Ping Back");
            Log.Info("create client proxy");
            var clientproxy = OperationContext.Current.GetCallbackChannel<ISimulationInformationClient>();
            Log.Info("ping back");
            clientproxy.PingDashboardVoid();
            Log.Info("done sending ping");

            Log.Info("Sleeping for a while");
            Thread.Sleep(10000);

            return true;
        }

    }
}
