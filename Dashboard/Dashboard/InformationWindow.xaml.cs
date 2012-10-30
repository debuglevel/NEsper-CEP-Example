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
        SimulationInformationServiceCallback callback;

        Data data;

        Chart chart;
        List<LocationPoint> positions = new List<LocationPoint>();

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
            callback.LocationChanged += this.ChangePosition;

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
            //Debug.WriteLine("done: "+result);
        }

        private void btnCreateChart_Click(object sender, RoutedEventArgs e)
        {
            WindowsFormsHost host = this.windowsFormsHost;
            var wfButton = new System.Windows.Forms.Button();
            wfButton.Text = "Windows Forms Button";

            host.Child = wfButton;


            var chart1 = createChart();
            this.chart = chart1;

            host.Child = chart1;
        }

        private static Chart createChart()
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
            //series1.MarkerImage = "C:\\Users\\marc\\Desktop\\source\\nesper\\cep\\car.png";
            series1.Name = "Autos";
            chart1.Series.Add(series1);
            chart1.Size = new System.Drawing.Size(400, 400);
            chart1.TabIndex = 1;
            chart1.Text = "chart1";





            // Set point chart type
            chart1.Series["Autos"].ChartType = SeriesChartType.Point;

            // Enable data points labels
            //chart1.Series["Series1"].IsValueShownAsLabel = true;
            //chart1.Series["Series1"]["LabelStyle"] = "Center";

            // Set marker size
            chart1.Series["Autos"].MarkerSize = 15;

            // Set marker shape
            chart1.Series["Autos"].MarkerStyle = MarkerStyle.Circle;
            // Set to 3D
            //chart1.ChartAreas[ [strChartArea].Area3DStyle.Enable3D = true;

            //chart1.Series["Autos"].MarkerImage = "car.png";
            return chart1;
        }

        public void ChangePosition(LocationPoint newLocationPoint)
        {
            positions.RemoveAll(p => p.Identifier == newLocationPoint.Identifier);
            positions.Add(new LocationPoint() { Identifier = newLocationPoint.Identifier, X = newLocationPoint.X, Y = newLocationPoint.Y });

            drawPositions();
        }

        private void drawPositions()
        {
            chart.Series["Autos"].Points.Clear();
            foreach (var point in positions)
            {
                chart.Series["Autos"].Points.AddXY(point.X, point.Y);
            }
        }
    }
}
