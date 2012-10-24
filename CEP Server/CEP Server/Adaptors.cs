using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CEP.Server.Adaptor.TCP;
using CEP.Server.Services;
using com.espertech.esper.client;
using log4net;

namespace CEP.Server
{
    class Adaptors
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Start()
        {
            // get the <system.serviceModel> / <services> config section
            ServicesSection services = ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;

            // get all classs
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(t => t.IsClass == true);

            // enumerate over each <service> node
            foreach (ServiceElement service in services.Services)
            {
                Type serviceType = allTypes.SingleOrDefault(t => t.FullName == service.Name);
                if (serviceType == null)
                {
                    continue;
                }

                ServiceHost serviceHost = new ServiceHost(serviceType);

                Log.InfoFormat("Open Service Host for {0} on (at least) {1}", serviceType.Name, serviceHost.BaseAddresses.FirstOrDefault());
                serviceHost.Open();
            }




            //// a URI which is already allowed by Visual Studio by default
            //Uri baseAddress = new Uri("http://localhost:8733/Design_Time_Addresses/CEP/SimulationInformation");

            //// Create the ServiceHost.
            //ServiceHost host = new ServiceHost(typeof(SimulationInformationService), baseAddress);
            //Binding wsDualBinding = new WSDualHttpBinding();
            //host.AddServiceEndpoint(typeof(ISimulationInformationService), wsDualBinding, "http://localhost:8733/Design_Time_Addresses/CEP/SimulationInformation");

            //// Enable metadata publishing.
            //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            //smb.HttpGetEnabled = true;
            //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            //host.Description.Behaviors.Add(smb);

            //// Open the ServiceHost to start listening for messages. Since
            //// no endpoints are explicitly configured, the runtime will create
            //// one endpoint per base address for each service contract implemented
            //// by the service.
            //host.Open();
        }
    }
}
