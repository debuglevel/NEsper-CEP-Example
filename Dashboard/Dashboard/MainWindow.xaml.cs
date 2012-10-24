using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CEP.Dashboard
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Data data = new Data();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_StatementManager(object sender, RoutedEventArgs e)
        {
            (new StatementWindow(data)).Show();
        }

        private void Button_Information(object sender, RoutedEventArgs e)
        {
            (new InformationWindow(data)).Show();
        }
    }
}
