using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using CEP.Common.Simulations;
using CEP.Common.Simulations.Car;

namespace CEP.Server.Adaptor.TCP
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
        void ReceiveIndividualAverageSpeed(string identifier, double? individualAverageSpeed);


    }
}
