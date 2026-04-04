using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net.Http;
using SEKTY_ON.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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
            string correo = txtCorreo.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(correo))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }
            else
            {
                if (username.Length > 50)
                {
                    MessageBox.Show("Nombre de usuario: No ingreses mas de 50 caracteres.");
                    return;
                }

                if (correo.Length > 100)
                {
                    MessageBox.Show("Correo electronico: No ingreses mas de 100 caracteres.");
                    return;
                }
                else
                {
                    string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                    if (!Regex.IsMatch(correo, patronCorreo))
                    {
                        MessageBox.Show("Por favor, ingrese un correo electrónico válido (ejemplo@dominio.com).");
                        return;
                    }

                    if (!correo.EndsWith("@gmail.com") && !correo.EndsWith("@uptcamac.edu.mx"))
                    {
                        MessageBox.Show("Solo se permiten correos de dominios conocidos.");
                        return;
                    }
                }
                

                if (password.Length != 8)
                {
                    MessageBox.Show("Contraseña: Debe poseer solo 8 caracteres.");
                    return;
                }
            }

            // guardar datos en la base de datos
            var loginData = new { Nombre = username, Contraseña = password, Activado = true, Correo = correo };

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
