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

            // every location change (for visualization)
            {
                var expr =
                    "SELECT Identifier, \n" +
                    "       X, \n" +
                    "       Y  \n" +
                    " FROM  LocationSensor";

                createStatement("LocationChange", expr);
            }

            // flow rate of all cars in the last x seconds
            {
                var expr =
                    "SELECT avg(Speed) \n" +
                    " FROM  SpeedSensor.win:time(30 sec)";

                createStatement("OverallAverageSpeed", expr);
            }

            // flow rate of each individual car in the last x seconds
            {
                var expr =
                    "SELECT    Identifier, \n" +
                    "          avg(Speed)  \n" +
                    " FROM     SpeedSensor.win:time(30 sec) \n" +
                    " GROUP BY Identifier";

                createStatement("IndividualAverageSpeed", expr);
            }

            // low air pressure of a tire while driving fast
            {
                String expression =
                    "SELECT speed.Identifier AS c, \n" +
                    "       Pressure         AS p, \n" +
                    "       avg(Speed)       AS s  \n" +
                    " FROM  SpeedSensor.win:time(10 sec) AS speed,   \n" +
                    "       TireSensor.win:time(10 sec)  AS pressure \n" +
                    " WHERE speed.Identifier  =  pressure.Identifier \n" +
                    "   AND pressure.Pressure <  1 \n" +
                    "   AND speed.Speed       >  50";

                createStatement("LowPressureAndHighSpeed", expression);
            }

            // speeding while driving in a certain area (which might be a city)
            {
                var expr =
                    "SELECT SpeedSensor.Identifier, \n" +
                    "       SpeedSensor.Speed, \n" +
                    "       LocationSensor.X, \n" +
                    "       LocationSensor.Y \n" +
                    " FROM  SpeedSensor.win:length(1), \n" +
                    "       LocationSensor.std:lastevent() \n" +
                    " WHERE SpeedSensor.Identifier  =  LocationSensor.Identifier \n" +
                    "   AND SpeedSensor.Speed       > 50 \n" + 
                    "   AND LocationSensor.X        > 50 \n" +
                    "   AND LocationSensor.Y        > 50 \n"
                    ;
                createStatement("Speeding", expr);
            }

            // air pressure in a tire falls under 1 bar while parking
            {
                String expr =
                    "SELECT SpeedSensor.Identifier  AS Identifier, \n" +
                    "       TireSensor.Pressure     AS Pressure, \n" +
                    "       SpeedSensor.Speed       AS Speed  \n" +
                    " FROM  SpeedSensor.std:lastevent(), \n" +
                    "       TireSensor.std:lastevent() \n" +
                    " WHERE SpeedSensor.Identifier  =  TireSensor.Identifier \n" +
                    "   AND TireSensor.Pressure     <  1 \n" +
                    "   AND SpeedSensor.Speed       =  0"
                    ;

                createStatement("LowPressure", expr);
            }
        }

        private void defaultUpdateEventHandler(object sender, UpdateEventArgs e)
        {
            var attributes = (e.NewEvents.FirstOrDefault().Underlying as Dictionary<String, object>);

            Log.Info("An Event occured: " + attributes.ToDebugString());
        }
    }
}
