using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using CEP.Common.Simulations;

namespace CEP.Server.Adaptor.TCP
{
    [ServiceContract]
    public interface IStatementManagerService
    {
        [OperationContract]
        Boolean CreateStatement(String name, String statement);

        [OperationContract]
        IDictionary<String, String> GetStatements();

        [OperationContract]
        void StopStatement(String name);

        [OperationContract]
        void StartStatement(String name);

        [OperationContract]
        void StopAllStatements();

        [OperationContract]
        void StartAllStatements();
    }

}
