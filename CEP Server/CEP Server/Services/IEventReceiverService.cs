using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using CEP.Common.Simulations;

namespace CEP.Server.Adaptor
{
    [ServiceContract]
    public interface IEventReceiverService
    {
        [OperationContract]
        void SendEvent(Sensor obj);
    }

}
