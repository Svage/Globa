using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie2.model
{
    public class Products : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public int ID_product;
        public string Name;
        public int? price;
        public string seller_name;
        public int? rating;
        public bool IsSelected { get; set; }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public int ID_Product
        {
            get { return ID_product; }
            set
            {
                if (ID_product != value)
                {
                    ID_product = value;
                    OnPropertyChanged(nameof(ID_Product));
                }
            }
        }


        public string Name_Product
        {
            get { return Name; }
            set
            {
                if (Name != value)
                {
                    Name = value;
                    OnPropertyChanged(nameof(Name_Product));
                }
            }
        }

        public string Seller_Name
        {
            get { return seller_name; }
            set
            {
                if (seller_name != value)
                {
                    seller_name = value;
                    OnPropertyChanged(nameof(Seller_Name));
                }
            }
        }


        public int? Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public int? Rating
        {
            get { return rating; }
            set
            {
                if (rating != value)
                {
                    rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }



    }
}