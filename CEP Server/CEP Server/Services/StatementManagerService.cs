using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CEP.Server.Adaptor.TCP;
using com.espertech.esper.client;
using log4net;

namespace CEP.Server.Services
{
    class StatementManagerService : IStatementManagerService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        EPServiceProvider epService = EPServiceProviderManager.GetDefaultProvider();

        public StatementManagerService()
        {
            Log.Info("Created Service Instance of StatementManagerService");
        }

        public Boolean CreateStatement(string name, string statement)
        {
            if (epService.EPAdministrator.StatementNames.Contains(name))
            {
                return false;
            }

            var epStatement = epService.EPAdministrator.CreateEPL(statement, name);
            return true;
        }

        public IDictionary<string, string> GetStatements()
        {
            var dict = new Dictionary<string, string>();

            foreach (var statementName in epService.EPAdministrator.StatementNames)
            {
                var epStatement = epService.EPAdministrator.GetStatement(statementName);
                var name = epStatement.Name;
                var cql = epStatement.Text;
                dict.Add(name, cql);
            }

            return dict;
        }

        public void StopStatement(string name)
        {
            if (epService.EPAdministrator.StatementNames.Contains(name))
            {
                epService.EPAdministrator.GetStatement(name).Stop();
            }
        }

        public void StartStatement(string name)
        {
            if (epService.EPAdministrator.StatementNames.Contains(name))
            {
                epService.EPAdministrator.GetStatement(name).Start();
            }
        }

        public void StopAllStatements()
        {
            epService.EPAdministrator.StopAllStatements();
        }

        public void StartAllStatements()
        {
            epService.EPAdministrator.StartAllStatements();
        }
    }
}
