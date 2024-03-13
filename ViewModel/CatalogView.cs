using System;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zadanie2.View;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Zadanie2.model;

namespace Zadanie2.ViewModel
{
    
        internal class CatalogView : INotifyPropertyChanged
        {

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public ICommand SaveChangesCommand { get; }
            public ICommand DeleteCommand { get; }

            private ObservableCollection<Products> _product;

            public ObservableCollection<Products> Products
            {
                get { return _product; }
                set
                {
                    if (_product != value)
                    {
                        _product = value;
                        OnPropertyChanged(nameof(Products));
                    }
                }
            }

            private void LoadProductsFromDatabase()
            {
                string connectionString = Alltabs.request;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT T.ID_product, S.seller_name, T.Name, T.rating, T.price " +
                        "FROM Product T " +
                        "JOIN [rating] PT ON T.rating = PT.rating " +
                        "JOIN [Sellers] S ON T.seller_name = S.seller_name "+
                        "JOIN [rating] CT ON PT.rating = CT.rating"; ;
                        
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Products.Add(new Products
                        {
                            ID_Product= Convert.ToInt32(reader["ID_product"]),
                            Name_Product = reader["Name"].ToString(),
                            Seller_Name = reader["seller_name"].ToString(),
                            Rating = reader["rating"] != DBNull.Value ? Convert.ToInt32(reader["rating"]) : (int?)null,
                            Price = reader["price"] != DBNull.Value ? Convert.ToInt32(reader["price"]) : (int?)null
                          
                        });
                    }

                    reader.Close();
                }
            }

            private void SaveChanges()
            {
                
                foreach (var product in Products)
                {
                    using (SqlConnection connection = new SqlConnection(Alltabs.request))
                    {
                        string query = $"UPDATE Product SET Nane = '{product.Name_Product}', price = {product.Price}, rating = {product.Rating} WHERE ID_Product = {product.ID_Product}";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        command.ExecuteNonQuery();

                    }
                }
              
            }

            private void DeleteSelectedProducts()
            {
   

                var selectedProducts = Products.Where(p => p.IsSelected).ToList();

                foreach (var product in selectedProducts)
                {
                    using (SqlConnection connection = new SqlConnection(Alltabs.request))
                    {
                        string query = $"DELETE FROM Товар WHERE ID_Product = {product.ID_Product}";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                
                foreach (var product in selectedProducts)
                {
                    Products.Remove(product);
                }

                MessageBox.Show("Выбранный товар был удален");
              
            }

            public CatalogView()
            {
                Products = new ObservableCollection<Products>();
                SaveChangesCommand = new RelayCommand(SaveChanges);
                DeleteCommand = new RelayCommand(DeleteSelectedProducts);
                LoadProductsFromDatabase();
            }
        }
    
}
