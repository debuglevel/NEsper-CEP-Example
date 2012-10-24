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
    [ServiceContract(CallbackContract = typeof(ISimulationInformationClient))]
    public interface ISimulationInformationService
    {
        [OperationContract(IsOneWay = true)]
        void PingServerVoid();

        [OperationContract(IsOneWay = true)]
        void PingServerVoidAndPingBack();

        [OperationContract]
        Boolean PingServerBooleanAndPingBack();

        [OperationContract]
        Boolean PingServerBoolean();

        [OperationContract]
        Boolean SubscribeSensorData();
    }
}
