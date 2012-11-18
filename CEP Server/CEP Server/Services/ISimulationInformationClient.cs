using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using CEP.Common.Simulations;
using CEP.Common.Simulations.Car;
using CEP.Common.Utils;

namespace CEP.Server.Adaptor
{
    [ServiceContract]
    public interface ISimulationInformationClient
    {
        [OperationContract(IsOneWay = true)]
        void PingDashboardVoid();

        [OperationContract(IsOneWay = true)]
        void ReceiveSensorData(Sensor sensor);

        [OperationContract(IsOneWay = true)]
        void ReceiveOverallAverageSpeed(double? overallAverageSpeed);

        [OperationContract(IsOneWay = true)]
        void ReceiveIndividualLocation(LocationPoint point);

        [OperationContract(IsOneWay = true)]
        void ReceiveNotificationDictionary(string statementName, Dictionary<String, object> dict);

        [OperationContract(IsOneWay = true)]
        void ReceiveSensorChange(Dictionary<String, object> dict);
    }
}
