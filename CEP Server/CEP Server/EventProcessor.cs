using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using com.espertech.esper.client;
using log4net;
using CEP.Common.Utils;

namespace CEP.Server
{
    class EventProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static EPServiceProvider epService = EPServiceProviderManager.GetDefaultProvider();

        public EventProcessor()
        {
            this.advertiseEventTypes();
        }

        private void advertiseEventTypes()
        {
            var configuration = epService.EPAdministrator.Configuration;

            //HACK: make EventTypes known manually, because NEsper does not seem to recognize classes in linked libraries
            var type = typeof(CEP.Common.IEsperEvent);
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type);

            foreach (var eventType in types)
            {
                Log.DebugFormat("Added EventType {0} as {1}", eventType.FullName, eventType.Name);
                configuration.AddEventType(eventType.Name, eventType);
            }
        }

        private EPStatement createStatement(string name, string expression)
        {
            Log.InfoFormat("Creating statement: {0}", expression);

            var statement = epService.EPAdministrator.CreateEPL(expression, name);
            statement.Events += defaultUpdateEventHandler;

            return statement;
        }

        public void CreateStatements()
        {
            // 
            createStatement("LocationChange", "select Identifier, X, Y \n from LocationSensor");

            // Flussgeschwindigkeit aller Autos in den letzten X Sekunden
            createStatement("OverallAverageSpeed", "select avg(Speed) \n from SpeedSensor.win:time(30 sec)");

            // Flussgeschwindigkeit jedes einzelnen Autos in den letzten X Sekunden
            createStatement("IndividualAverageSpeed", "select Identifier, avg(Speed) \n from SpeedSensor.win:time(30 sec) \n group by Identifier");

            // niedriger Reifendruck und hohe Geschwindigkeit
            {
                String expression =
                    "select speed.Identifier as c, Pressure as p, avg(Speed) as s " +
                    "from SpeedSensor.win:time(10 sec) as speed, TireSensor.win:time(10 sec) as pressure " +
                    "where speed.Identifier = pressure.Identifier " +
                    "and pressure.Pressure < 1 and speed.Speed > 50";

                createStatement("LowPressureAndHighSpeed", expression);
            }
        }

        private void defaultUpdateEventHandler(object sender, UpdateEventArgs e)
        {
            var attributes = (e.NewEvents.FirstOrDefault().Underlying as Dictionary<String, object>);

            Log.Info("An Event occured: " + attributes.ToDebugString());
        }
    }
}
