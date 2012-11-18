using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEP.Dashboard
{
    public class CarInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //private double? averageSpeed;
        //public double? AverageSpeed
        //{
        //    get
        //    {
        //        return averageSpeed;
        //    }
        //    set
        //    {
        //        averageSpeed = value;
        //        OnPropertyChanged("AverageSpeed");
        //    }
        //}

        private string identifier;
        public string Identifier
        {
            get
            {
                return identifier;
            }
            set
            {
                identifier = value;
                OnPropertyChanged("Identifier");
            }
        }

        private double? pressure;
        public double? Pressure
        {
            get
            {
                return pressure;
            }
            set
            {
                pressure = Rounding.Round(value);
                OnPropertyChanged("Pressure");
            }
        }

        private double? speed;
        public double? Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = Rounding.Round(value);
                OnPropertyChanged("Speed");
            }
        }

        private double? x;
        public double? X
        {
            get
            {
                return x;
            }
            set
            {
                x = Rounding.Round(value);
                OnPropertyChanged("X");
            }
        }

        private double? y;
        public double? Y
        {
            get
            {
                return y;
            }
            set
            {
                y = Rounding.Round(value);
                OnPropertyChanged("Y");
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
