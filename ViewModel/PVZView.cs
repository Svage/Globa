using Zadanie2.model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System;

internal class PVZView : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ObservableCollection<PVZ> _pvz;

    public ObservableCollection<PVZ> PVZS
    {
        get { return _pvz; }
        set
        {
            if (_pvz != value)
            {
                _pvz = value;
                OnPropertyChanged(nameof(PVZS));
            }
        }
    }

    private void LoadProductsFromDatabase()
    {
        string connectionString = Alltabs.request;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT ID_PVZ, Location, last_month " +
                "FROM Пункты_выдачи";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                PVZS.Add(new PVZ
                {
                    ID_PVZ = Convert.ToInt32(reader["id_pvz"]),
                    Location = reader["location"].ToString(),
                    last_month = reader["last_month"].ToString(),

                });
            }

            reader.Close();
        }
    }
    public PVZView()
    {
        PVZS = new ObservableCollection<PVZ>();
        LoadProductsFromDatabase();
    }
}
