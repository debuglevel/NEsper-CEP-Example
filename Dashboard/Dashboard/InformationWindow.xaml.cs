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
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CEP.Dashboard.SimulationInformationService;

namespace CEP.Dashboard
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        SimulationInformationServiceClient proxy;
        public SimulationInformationServiceClient Proxy
        {
            get { return proxy; }
            set { proxy = value; }
        }

        List<string> notifications = new List<string>();
        public List<string> Notifications
        {
            get { return notifications; }
            set { notifications = value; }
        }

        SimulationInformationServiceCallback callback;

        Data data;

        Chart chart;
        List<LocationPoint> positions = new List<LocationPoint>();

        public InformationWindow(Data data) : this()
        {
            this.data = data;
            this.DataContext = data;
        }

        public InformationWindow()
        {
            InitializeComponent();

            this.setupLocationChart();
        }

        private void createProxy()
        {
            Debug.WriteLine("Create Proxy");

            this.callback = new SimulationInformationServiceCallback(data);
            callback.LocationChanged += this.changeLocation;
            callback.NotificationReceived += this.displayNotification;

            InstanceContext instanceContext = new InstanceContext(callback);
            this.proxy = new SimulationInformationServiceClient(instanceContext);
            
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
        }

        private void setupLocationChart()
        {
            WindowsFormsHost host = this.windowsFormsHost;
            this.chart = this.createLocationChart();
            host.Child = this.chart;
        }

        #region wpf code
        private Chart createLocationChart()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            var chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();


            // 
            // chart1
            // 
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Enabled = false;
            chartArea1.AxisX.LabelStyle.IsEndLabelVisible = false;
            chartArea1.AxisX.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.MajorTickMark.Enabled = false;
            chartArea1.AxisX.Maximum = 100D;
            chartArea1.AxisX.MaximumAutoSize = 100F;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Enabled = false;
            chartArea1.AxisY.LabelStyle.IsEndLabelVisible = false;
            chartArea1.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.NotSet;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.AxisY.MaximumAutoSize = 100F;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            chart1.Location = new System.Drawing.Point(573, 12);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Name = "Autos";
            chart1.Series.Add(series1);
            chart1.Size = new System.Drawing.Size(400, 400);
            chart1.TabIndex = 1;
            chart1.Text = "chart1";

            // Set point chart type
            chart1.Series["Autos"].ChartType = SeriesChartType.Point;

            // Set marker size
            chart1.Series["Autos"].MarkerSize = 15;

            // Set marker shape
            chart1.Series["Autos"].MarkerStyle = MarkerStyle.Circle;

            chart1.Series["Autos"].MarkerImage = "car.png";

            chart1.BorderlineDashStyle = ChartDashStyle.Dot;
            chart1.BorderlineWidth = 2;
            chart1.BorderlineColor = System.Drawing.Color.Gray;

            return chart1;
        }
        #endregion

        private void changeLocation(LocationPoint newLocationPoint)
        {
            positions.RemoveAll(p => p.Identifier == newLocationPoint.Identifier);
            positions.Add(new LocationPoint() { Identifier = newLocationPoint.Identifier, X = newLocationPoint.X, Y = newLocationPoint.Y });

            drawLocations();
        }

        private void drawLocations()
        {
            chart.Series["Autos"].Points.Clear();
            foreach (var point in positions)
            {
                chart.Series["Autos"].Points.AddXY(point.X, point.Y);
            }
        }

        private void btnSubscribeStatement_Click(object sender, RoutedEventArgs e)
        {
            if (proxy != null)
            {
                proxy.SubscribeStatementAsync((listboxStatements.SelectedItem as Statement).Name);
            }
        }
        
        private void displayNotification(string notification)
        {
            if (this.notifications.Contains(notification) == false || this.cbShowDuplicates.IsChecked == true)
            {
                this.notifications.Add(notification);
                this.tbNotifications.AppendText(notification + "\n");
            }
        }

        private void btnUnsubscribeStatement_Click(object sender, RoutedEventArgs e)
        {
            if (proxy != null)
            {
                proxy.UnsubscribeStatementAsync((listboxStatements.SelectedItem as Statement).Name);
            }
        }
    }
}
