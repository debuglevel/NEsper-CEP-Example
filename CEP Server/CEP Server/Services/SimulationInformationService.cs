﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using CEP.Common.Simulations;
using CEP.Common.Utils;
using CEP.Server;
using com.espertech.esper.client;
using log4net;

namespace CEP.Server.Adaptor
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SimulationInformationService : ISimulationInformationService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ISimulationInformationClient client;

        EPServiceProvider epService = EPServiceProviderManager.GetDefaultProvider();

        public SimulationInformationService()
        {
            Log.Info("Created Service Instance of SimulationInformationService");
            
            client = OperationContext.Current.GetCallbackChannel<ISimulationInformationClient>();
        }

        public void SubscribeStatement(string statementName)
        {
            Log.Info("Client subscribed to get notifications about Statement "+statementName);

            var statement = epService.EPAdministrator.GetStatement(statementName);

            if (statement != null)
            {
                statement.Events += SendNotifications;
            }
            else
            {
                Log.WarnFormat("Client wants to subscribe to Statement {0} which does not exist.", statementName);
            }
        }

        public void UnsubscribeStatement(string statementName)
        {
            Log.Info("Client unsubscribed to get no more notifications about Statement " + statementName);

            var statement = epService.EPAdministrator.GetStatement(statementName);

            if (statement != null)
            {
                statement.Events -= SendNotifications;
            }
            else
            {
                Log.WarnFormat("Client wants to unsubscribe from Statement {0} which does not exist.", statementName);
            }
        }

        public Boolean SubscribeSensorData()
        {
            Log.Info("Client subscribed to get sensor data");

            epService.EPAdministrator.GetStatement("OverallAverageSpeed").Events += OnOverallAverageSpeed;
            epService.EPAdministrator.GetStatement("LocationChange").Events += OnIndividualLocationChange;
            epService.EPAdministrator.GetStatement("SensorChange").Events += OnSensorChange;

            Log.Debug("Ping Dashboard");
            client.PingDashboardVoid();
            Log.Debug("done pinging dashboard");

            return true;
        }

        private void OnSensorChange(object sender, UpdateEventArgs e)
        {
            var dict = e.NewEvents.FirstOrDefault().Underlying as Dictionary<String, object>;

            try
            {
                Log.Debug("Send OnSensorChange Event");
                client.ReceiveSensorChange(dict);
            }
            catch (TimeoutException ex)
            {
                Log.Error("Sending notification timed out: " + ex.Message);
                this.shutdownServiceInstance();
            }
            catch (CommunicationException ex)
            {
                Log.Error("Sending notification failed: " + ex.Message);
                this.shutdownServiceInstance();
            }
        }

        private void OnIndividualLocationChange(object sender, UpdateEventArgs e)
        {
            var dict = e.NewEvents.FirstOrDefault().Underlying as Dictionary<String, object>;

            var point = new LocationPoint();
            point.X = dict["X"] as double?;
            point.Y = dict["Y"] as double?;
            point.Identifier = dict["Identifier"] as string;

            try
            {
                Log.Debug("Send OnIndividualAverageSpeed Event");
                client.ReceiveIndividualLocation(point);
            }
            catch (TimeoutException ex)
            {
                Log.Error("Sending notification timed out: " + ex.Message);
                this.shutdownServiceInstance();
            }
            catch (CommunicationException ex)
            {
                Log.Error("Sending notification failed: " + ex.Message);
                this.shutdownServiceInstance();
            }
        }

        private void OnOverallAverageSpeed(object sender, UpdateEventArgs e)
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
                this.shutdownServiceInstance();
            }
            catch (CommunicationException ex)
            {
                Log.Error("Sending notification failed: " + ex.Message);
                this.shutdownServiceInstance();
            }
        }

        private void SendNotifications(object sender, UpdateEventArgs e)
        {
            var dict = e.NewEvents.FirstOrDefault().Underlying as Dictionary<String, object>;

            try
            {
                Log.Debug("Sending subscribed notification as dictionary");
                client.ReceiveNotificationDictionary(e.Statement.Name, dict);
            }
            catch (TimeoutException ex)
            {
                Log.Error("Sending notification timed out: " + ex.Message);
                this.shutdownServiceInstance();
            }
            catch (CommunicationException ex)
            {
                Log.Error("Sending notification failed: " + ex.Message);
                this.shutdownServiceInstance();
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

        private void shutdownServiceInstance()
        {
            Log.Warn("Shutting down service instance");

            EPServiceProvider epService = EPServiceProviderManager.GetDefaultProvider();

            epService.EPAdministrator.GetStatement("OverallAverageSpeed").Events -= OnOverallAverageSpeed;
            epService.EPAdministrator.GetStatement("LocationChange").Events -= OnIndividualLocationChange;
            epService.EPAdministrator.GetStatement("SensorChange").Events -= OnSensorChange;

        }
    }
}
