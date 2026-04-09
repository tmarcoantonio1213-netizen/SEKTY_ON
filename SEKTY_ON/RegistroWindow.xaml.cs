using Newtonsoft.Json;
using SEKTY_ON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SEKTY_ON
{
    /// <summary>
    /// Lógica de interacción para RegistroWindow.xaml
    /// </summary>
    public partial class RegistroWindow : Window
    {
        public RegistroWindow()
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

        private async void btnRegristrarse_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string password = txtPassword.Password.Trim();

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

            // se valida el primer usuario para ver si se le asigna el rol de administrador
            bool crearAdmin = await ValidarAdministrador();
            string rolAsignado = crearAdmin ? "ADMINISTRADOR" : "USUARIO";

            // convierto pasword a hash
            byte[] passwordHash;
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                passwordHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            bool activado = false;
            if (rolAsignado == "ADMINISTRADOR")
            {
                activado = true;
            }

            // guardar datos en la base de datos
            var loginData = new { Nombre = username, Contraseña = passwordHash, Activado = activado, Correo = correo, Rol = rolAsignado };

            try
            {
                using (var client = new HttpClient())
                {
                    string url = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Responsables";

                    var json = JsonConvert.SerializeObject(loginData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();

                        Responsable usuarioLogueado = JsonConvert.DeserializeObject<Responsable>(responseBody);

                        MessageBox.Show("Cuenta registrada con exitoso.");

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

        private async Task<bool> ValidarAdministrador()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string url = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Responsables";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Responsable> responsables = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Responsable>>(json);


                        return responsables != null && responsables.Count == 0;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"No se pudo conectar a la base de datos");
                    return false;
                }
            }
        }
    }
}
