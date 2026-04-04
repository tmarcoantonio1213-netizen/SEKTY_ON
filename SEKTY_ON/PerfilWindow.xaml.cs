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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SEKTY_ON
{
    /// <summary>
    /// Lógica de interacción para PerfilWindow.xaml
    /// </summary>
    public partial class PerfilWindow : Window
    {
        public PerfilWindow()
        {
            InitializeComponent();

            _ = BuscarPerfil();
        }

        private async Task BuscarPerfil()
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "https://localhost:7060/api/Responsables";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Responsable> responsables = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Responsable>>(json);


                        var usuarioActivo = responsables.FirstOrDefault(r => r.Activado == true);
                        this.DataContext = usuarioActivo;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al validar el estado del perfil: {ex.Message}");
                }
            }
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            MainWindow ventanaLaboratorios = new MainWindow();
            ventanaLaboratorios.Show();
            this.Close();
        }

        private async void btnActualizarDatos_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string correo = txtCorreo.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(correo))
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
            }

            Responsable resp = this.DataContext as Responsable;

            if (resp == null)
            {
                MessageBox.Show("No se encontró la sesión del usuario.");
                return;
            }

            var result = MessageBox.Show($"¿Seguro de actualizar los datos?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                resp.Nombre = username;
                resp.Correo = correo;

                using (var client = new HttpClient())
                {
                    try
                    {
                        string url = $"https://localhost:7060/api/Responsables/{resp.Id}";

                        var json = JsonConvert.SerializeObject(resp);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PutAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Los datos se han actualizado exitosamente.");
                        }
                        else
                        {
                            string errorDetalle = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"No se guardaron los cambios.\nError: {response.StatusCode}: {errorDetalle}");
                            this.DataContext = null;
                            this.DataContext = resp;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error de red: {ex.Message}");
                    }

                }
            }


        }

        private async void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Responsable resp = this.DataContext as Responsable;

            if (resp == null)
            {
                MessageBox.Show("No se encontró la sesión del usuario.");
                return;
            }

            var result = MessageBox.Show($"¿Seguro que quieres cerrar sesión?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                resp.Activado = false;

                using (var client = new HttpClient())
                {
                    try
                    {
                        string url = $"https://localhost:7060/api/Responsables/{resp.Id}";

                        var json = JsonConvert.SerializeObject(resp);
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
                            MessageBox.Show($"No se guardaron los cambios.\nError: {response.StatusCode}: {errorDetalle}");
                            resp.Activado = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error de red: {ex.Message}");
                        resp.Activado = true;
                    }
                    
                }
            }
        }
    }
}
