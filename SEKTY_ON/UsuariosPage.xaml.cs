using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SEKTY_ON.Models;

namespace SEKTY_ON
{
    /// <summary>
    /// Lógica de interacción para UsuariosPage.xaml
    /// </summary>
    public partial class UsuariosPage : Page
    {
        public UsuariosPage()
        {
            InitializeComponent();

            _ = VisualizarUsuarios();
        }

        private async Task VisualizarUsuarios()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "https://localhost:7060/api/Usuarios";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Usuario> usuarios = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Usuario>>(json);

                        if (usuarios != null)
                        {
                            lstBxLaboratorios.ItemsSource = usuarios;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se puede cargar los usuarios");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private async void btnAbilitarCuenta_Click(object sender, RoutedEventArgs e)
        {
            Button botonPresionado = sender as Button;
            Usuario lab = botonPresionado.DataContext as Usuario;

            if (lab != null)
            {
                lab.Abilitado = !lab.Abilitado;
            }
            else
            {
                MessageBox.Show("No se pudo obtener iformacion del usuario");
            }

            using (var client = new HttpClient())
            {
                string url = $"https://localhost:7060/api/Usuarios/{lab.Id}";

                var json = JsonConvert.SerializeObject(lab);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await VisualizarUsuarios();
                }
                else
                {
                    MessageBox.Show("No se guardaron los cambios.");
                    lab.Abilitado = !lab.Abilitado;
                }
            }
        }
    }
}
