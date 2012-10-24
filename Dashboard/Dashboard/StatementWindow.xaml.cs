using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using CEP.Dashboard.StatementManagerService;

namespace CEP.Dashboard
{
    /// <summary>
    /// Interaktionslogik für StatementWindow.xaml
    /// </summary>
    public partial class StatementWindow : Window
    {
        StatementManagerServiceClient proxy;

        private Data data;

        public StatementWindow(Data data) : this()
        {
            this.data = data;
            this.DataContext = data;
        }

        public StatementWindow()
        {
            InitializeComponent();
        }

        private void createProxy()
        {
            Debug.WriteLine("Created Proxy");

            proxy = new StatementManagerService.StatementManagerServiceClient();
        }

        private void ButtonGetStatements_Click(object sender, RoutedEventArgs e)
        {
            this.createProxy();
            var statements = proxy.GetStatements();
            data.Statements.Clear();
            foreach (var statement in statements)
            {
                data.Statements.Add(new Statement() { Name = statement.Key, CQL = statement.Value });
            }
        }
    }
}
