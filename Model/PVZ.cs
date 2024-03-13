using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie2.model
{
    public class PVZ
    {
        public int ID_pvz;
        public string Location;
        public int number_employees;
        public int Rating;
        public string last_month;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsSelected { get; set; }

        public int ID_PVZ
        {
            get { return ID_pvz; }
            set
            {
                if (ID_pvz != value)
                {
                    ID_pvz = value;
                    OnPropertyChanged(nameof(ID_PVZ));
                }
            }
        }

        public string Location_PVZ
        {
            get { return Location; }
            set
            {
                if (Location != value)
                {
                    Location = value;
                    OnPropertyChanged(nameof(Location_PVZ));
                }
            }
        }

        public int Number_employees
        {
            get { return number_employees; }
            set
            {
                if (number_employees != value)
                {
                    number_employees = value;
                    OnPropertyChanged(nameof(Number_employees));
                }
            }
        }

        public int Rating_PVZ
        {
            get { return Rating; }
            set
            {
                if (Rating != value)
                {
                    Rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }
    }
}
