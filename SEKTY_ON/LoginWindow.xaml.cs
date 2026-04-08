using Newtonsoft.Json;
using SEKTY_ON.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
            string identidad = txtIdentidad.Text.Trim();
            string password = txtPassword.Password.Trim();

            bool medio;

            if (string.IsNullOrWhiteSpace(identidad) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }
            else
            {
                string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(identidad, patronCorreo))
                {
                    if (identidad.Length > 50)
                    {
                        MessageBox.Show("Nombre de usuario: No ingreses mas de 50 caracteres.");
                        return;
                    }
                    medio = true;
                }
                else
                {
                    if (identidad.Length > 100)
                    {
                        MessageBox.Show("Correo electronico: No ingreses mas de 100 caracteres.");
                        return;
                    }
                    if (!identidad.EndsWith("@gmail.com") && !identidad.EndsWith("@uptcamac.edu.mx"))
                    {
                        MessageBox.Show("Solo se permiten correos de dominios conocidos.");
                        return;
                    }
                    medio = false;
                }

                if (password.Length != 8)
                {
                    MessageBox.Show("Contraseña: Debe poseer solo 8 caracteres.");
                    return;
                }

                await ValidarUsuario(password, identidad, medio);
            }
        }

        private async Task ValidarUsuario(string password, string identidad, bool medio)
        {
            byte[] hashingresado;

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                hashingresado = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            using (var client = new HttpClient())
            {
                try
                {
                    string url = "https://localhost:7060/api/Responsables";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Responsable> responsables = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Responsable>>(json);

                        if (medio == true)
                        {
                            var usuario = responsables.FirstOrDefault(r => r.Nombre == identidad);

                            if (usuario != null)
                            {
                                if (usuario.Contraseña.SequenceEqual(hashingresado))
                                {
                                    IniciarSesion(usuario);
                                }
                                else
                                {
                                        string hashDb = BitConverter.ToString(usuario.Contraseña).Replace("-", "");
                                        string hashIngresado = BitConverter.ToString(hashingresado).Replace("-", "");

                                        MessageBox.Show($"ERROR DE VALIDACIÓN:\n\n" +
                                                        $"DB: {hashDb}\n\n" +
                                                        $"INGRESADO: {hashIngresado}");
                                    
                                }
                            }
                            else
                            {
                                MessageBox.Show("El usuario no existe");
                            }
                        }
                        else
                        {
                            var usuario = responsables.FirstOrDefault(r => r.Correo == identidad);

                            if (usuario != null)
                            {
                                if (usuario.Contraseña.SequenceEqual(hashingresado))
                                {
                                    IniciarSesion(usuario);
                                }
                                else
                                {
                                    MessageBox.Show("Contraseña incorrecta");
                                }
                            }
                            else
                            {
                                MessageBox.Show("El usuario no existe");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al validar el Contraseña: {ex.Message}");
                }
            }
        }

        private async Task IniciarSesion(Responsable usuario)
        {
            usuario.Activado = true;

            using (var client = new HttpClient())
            {
                try
                {
                    string url = $"https://localhost:7060/api/Responsables/{usuario.Id}";

                    var json = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MainWindow ventanaLaboratorios = new MainWindow();
                        ventanaLaboratorios.Show();
                        this.Close();
                    }
                    else
                    {
                        string errorDetalle = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"No se pudo iniciar sesion.\nError: {response.StatusCode}: {errorDetalle}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error de red: {ex.Message}");
                }

            }
        }

        private string ToHex(byte[] bytes) => BitConverter.ToString(bytes).Replace("-", "");
    }
}
