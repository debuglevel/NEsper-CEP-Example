using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServiceTester.ServiceReference1;

namespace CEP.Dashboard
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        SimulationInformationServiceClient proxy;
        SimulationInformationServiceCallback callback;

        Data data;

        public InformationWindow(Data data)
            : this()
        {
            this.data = data;
            this.DataContext = data;
        }

        public InformationWindow()
        {
            InitializeComponent();
        }

        private void createProxy()
        {
            Debug.WriteLine("Create Proxy");

            this.callback = new SimulationInformationServiceCallback(data);
            InstanceContext instanceContext = new InstanceContext(callback);
            this.proxy = new  ServiceTester.ServiceReference1.SimulationInformationServiceClient(instanceContext);
            
            this.proxy.Open();

            Debug.WriteLine("done");
        }

        private void Button_Connect(object sender, RoutedEventArgs e)
        {
            this.createProxy();

            Debug.WriteLine("Subscribe to Sensor Data");
            //var result = proxy.SubscribeSensorData();
            proxy.SubscribeSensorDataAsync();   // Caution: Skype listens on port 80 by default
            Debug.WriteLine("done: ");
            //Debug.WriteLine("done: "+result);
        }
    }
}
