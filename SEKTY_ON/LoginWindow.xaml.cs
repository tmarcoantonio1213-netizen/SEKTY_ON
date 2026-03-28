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
using System.Net.Http;
using System.Text;
using System.Linq.Expressions;
using SEKTY_ON.Models;
using Newtonsoft.Json;

namespace SEKTY_ON
{
    /// <summary>
    /// Lógica de interacción para LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnVerContraseña_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPasswordVisible.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtPasswordVisible.Visibility = Visibility.Visible;
        }

        private void btnVerContraseña_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            OcultarContraseña();
        }

        private void OcultarContraseña()
        {
            txtPasswordVisible.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Focus();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }
            else
            {
                if (username.Length > 50)
                {
                    MessageBox.Show("El nombre de usuario debe tener con menos de 50 caracteres.");
                }

                if (password.Length != 8)
                {
                    MessageBox.Show("La contraseña debe poseer 8 caracteres.");
                }
            }

            // guardar datos en la base de datos
            var loginData = new { Nombre = username, Contraseña = password, Activado = true };

            try
            {
                using (var client = new HttpClient())
                {
                    string url = "https://localhost:7060/api/Responsables";

                    var json = JsonConvert.SerializeObject(loginData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();

                        Responsable usuarioLogueado = JsonConvert.DeserializeObject<Responsable>(responseBody);

                        MessageBox.Show("Inicio de sesión exitoso.");

                        MainWindow laboratoriosVentana = new MainWindow();
                        laboratoriosVentana.DataContext = usuarioLogueado;
                        laboratoriosVentana.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Error al iniciar al registrar: {response.StatusCode}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con el servidor: {ex.Message}");
                return;
            }
        }
    }
}
