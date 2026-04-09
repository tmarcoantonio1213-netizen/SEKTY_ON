using Newtonsoft.Json;
using SEKTY_ON.Models;
using System;
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
    public partial class LoginWindow : Window
    {

        private readonly string urlApi = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Responsables";

        public LoginWindow()
        {
            InitializeComponent();
        }

        #region Lógica de Visibilidad de Contraseña


        private void btnVerContraseña_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPasswordVisible.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtPasswordVisible.Visibility = Visibility.Visible;
        }


        private void btnVerContraseña_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            txtPasswordVisible.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Focus();
        }

        #endregion

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string identidad = txtIdentidad.Text.Trim();
            string password = txtPassword.Password.Trim();
            bool esNombreUsuario;

            if (string.IsNullOrWhiteSpace(identidad) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Aviso");
                return;
            }

            // Validación de formato
            string patronCorreo = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(identidad, patronCorreo))
            {
                if (identidad.Length > 50) { MessageBox.Show("Usuario demasiado largo."); return; }
                esNombreUsuario = true;
            }
            else
            {
                if (identidad.Length > 100) { MessageBox.Show("Correo demasiado largo."); return; }
                if (!identidad.EndsWith("@gmail.com") && !identidad.EndsWith("@uptcamac.edu.mx"))
                {
                    MessageBox.Show("Dominio de correo no permitido.");
                    return;
                }
                esNombreUsuario = false;
            }

            if (password.Length != 8)
            {
                MessageBox.Show("La contraseña debe tener exactamente 8 caracteres.");
                return;
            }

            await ValidarUsuario(password, identidad, esNombreUsuario);
        }

        private async Task ValidarUsuario(string password, string identidad, bool esNombreUsuario)
        {
            byte[] hashingresado;
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                hashingresado = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(urlApi);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Responsable> responsables = JsonConvert.DeserializeObject<List<Responsable>>(json);


                        Responsable usuario = esNombreUsuario
                            ? responsables.FirstOrDefault(r => r.Nombre == identidad)
                            : responsables.FirstOrDefault(r => r.Correo == identidad);

                        if (usuario != null)
                        {

                            if (usuario.Contraseña.SequenceEqual(hashingresado))
                            {
                                await IniciarSesion(usuario);
                            }
                            else
                            {
                                MessageBox.Show("Contraseña incorrecta.", "Error de acceso");
                            }
                        }
                        else
                        {
                            MessageBox.Show("El usuario no existe.", "Error");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener datos de la API.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar con el servidor: {ex.Message}");
                }
            }
        }

        private async Task IniciarSesion(Responsable usuario)
        {

            if (usuario.Activado == null)
            {
                MessageBox.Show("Tu cuenta no ha sido autorizada para iniciar sesión.", "Acceso denegado");
                return;
            }

            usuario.Activado = true; // Marcamos como activo/sesión iniciada

            using (var client = new HttpClient())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync($"{urlApi}/{usuario.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Sesión validada, pero no se pudo actualizar el estado en el servidor.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error de red al actualizar estado: {ex.Message}");
                }
            }
        }


        private void btnOlvidastePassword_Click(object sender, RoutedEventArgs e)
        {
            RecoveryWindow recovery = new RecoveryWindow();
            recovery.ShowDialog();
        }
    }
}