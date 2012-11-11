using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CEP.Dashboard
{
    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Data()
        {
            this.individualAverageSpeed = new ObservableCollection<KeyValuePair<string, double?>>();
            this.statements = new ObservableCollection<Statement>();
            this.cars = new ObservableCollection<CarInfo>();
        }

        ObservableCollection<KeyValuePair<string, double?>> individualAverageSpeed;
        public ObservableCollection<KeyValuePair<string, double?>> IndividualAverageSpeed
        {
            get { return individualAverageSpeed; }
            set
            {
                individualAverageSpeed = value;
                OnPropertyChanged("IndividualAverageSpeed");
            }
        }

        private double? overallAverageSpeed;
        public double? OverallAverageSpeed
        {
            get { return overallAverageSpeed; }
            set
            {
                overallAverageSpeed = value;
                OnPropertyChanged("OverallAverageSpeed");
            }
        }

        private ObservableCollection<Statement> statements;
        public ObservableCollection<Statement> Statements
        {
            get { return statements; }
            set
            {
                statements = value;
                OnPropertyChanged("Statements");
            }
        }

        private ObservableCollection<CarInfo> cars;
        public ObservableCollection<CarInfo> Cars
        {
            get { return cars; }
            set
            {
                cars = value;
                OnPropertyChanged("Cars");
            }
        }

        

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
