using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Zadanie2.model;

namespace Zadanie2.ViewModel
{

    public class LoginView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Employee_Id
        {
            get { return Employee.ID_employee; }
            set
            {
                Employee.ID_employee = value;
                OnPropertyChanged(nameof(Employee_Id));
            }
        }
        public string Employee_Login
        {
            get { return Employee._employee_login; }
            set
            {
                Employee._employee_login = value;
                OnPropertyChanged(nameof(Employee_Login));
            }
        }
        public string Employee_Password
        {
            get { return Employee._employee_password; }
            set
            {
                Employee._employee_password = value;
                OnPropertyChanged(nameof(Employee_Password));
            }
        }

        public string HashedPassw
        {
            get { return Alltabs._hashedPassword; }
            set
            {
                Alltabs._hashedPassword = value;
                OnPropertyChanged(nameof(HashedPassw));
            }
        }

        public ICommand LoginCommand { get; }

        public LoginView()
        {
            LoginCommand = new RelayCommand(Login);
        }
        public int Employee_tag_id
        {
            get { return Employee._employee_tag_id; }
            set
            {
                Employee._employee_tag_id = value;
                OnPropertyChanged(nameof(Employee_tag_id));
            }
        }
        private void Login()
        {
            if (string.IsNullOrEmpty(Employee_Login))
            {
                MessageBox.Show("Введите Логин");
                return;
            }

            if (string.IsNullOrEmpty(Employee_Password))
            {
                MessageBox.Show("Введите Пароль");
                return;
            }


            string hashedInputPassword = HashPassword(Employee_Password);

            using (SqlConnection conn = new SqlConnection(Alltabs.request))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SqlCommand command = new SqlCommand(@"SELECT Employee_Password, Employee_tag_id FROM Employee WHERE Employee_Login = @Employee_Login", conn))
                        {
                            command.Parameters.AddWithValue("@Employee_Login", Employee_Login);
                            using (SqlDataReader reader = command.ExecuteReader())
                                if (reader.Read())
                                {

                                    string storedHashedPassword = reader.GetString(0);
                                    int Employee_tag_id = reader.GetInt32(1);

                                    reader.Close();

                                    if (storedHashedPassword != null && VerifyPassword(Employee_Login, hashedInputPassword))
                                    {
                                        Employee._employee_tag_id = Employee_tag_id;
                                        conn.Close();
                                        MessageBox.Show("Успешно: " + Employee_Login);
                                    }
                                    else
                                    {
                                        conn.Close();
                                        MessageBox.Show("Неверный Логин или Пароль");
                                    }

                                }
                                else
                                {
                                    conn.Close();
                                    MessageBox.Show("Неверный Логин или Пароль");
                                }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не удалось подключиться к бд");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при подключении к бд: " + ex.Message);
                }
            }
        }

            private string HashPassword(string employee_password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(employee_password));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashedBytes.Length; i++)
                    {
                        builder.Append(hashedBytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }

            private bool VerifyPassword(string employee_login, string inputPassword)
            {
                using (SqlConnection conn = new SqlConnection(Alltabs.request))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(@"SELECT Employee_Password FROM Сотрудники WHERE Employee_Login = @Employee_Login", conn))
                    {
                        command.Parameters.AddWithValue("@Employee_Login", employee_login);
                        string storedHashedPassword = (string)command.ExecuteScalar();

                        if (storedHashedPassword != null)
                        {

                            if (storedHashedPassword == inputPassword)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        
    }
}   