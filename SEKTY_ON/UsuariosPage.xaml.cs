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

        public static readonly DependencyProperty IsAdminActiveProperty = DependencyProperty.Register(
            nameof(IsAdminActive), typeof(bool), typeof(UsuariosPage), new PropertyMetadata(false));

        public bool IsAdminActive
        {
            get => (bool)GetValue(IsAdminActiveProperty);
            set => SetValue(IsAdminActiveProperty, value);
        }

        public UsuariosPage()
        {
            InitializeComponent();

 
            this.DataContext = this;

            ValidarAdministrador();

            _ = VisualizarUsuarios();
        }

        private async Task VisualizarUsuarios()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Responsables";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Responsable> responsables = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Responsable>>(json);

                        if (responsables != null)
                        {
                            lstBxLaboratorios.ItemsSource = responsables;
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

        private async Task ValidarAdministrador()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Responsables";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Responsable> responsables = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Responsable>>(json);

                        var usuario = responsables.FirstOrDefault(r => r.Rol == "ADMINISTRADOR");

                        if (usuario != null && usuario.Activado == true)
                        {
                            rowAgregarUsuario.Height = new GridLength(70);
                            IsAdminActive = true; 
                        }
                        else
                        {
                            rowAgregarUsuario.Height = new GridLength(0);
                            IsAdminActive = false; 
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se puedo conectar a la base de datos");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private async void btnAgregarUsuario_Click(object sender, RoutedEventArgs e)
        {
            RegistroWindow ventanaRegistro = new RegistroWindow();
            ventanaRegistro.Show();
            Window.GetWindow(this)?.Close();
        }

        private async void btnEliminarCuenta_Click(object sender, RoutedEventArgs e)
        {
            Button botonPresionado = sender as Button;
            Responsable res = botonPresionado.DataContext as Responsable;

            if (res != null)
            {
                var result = MessageBox.Show($"¿Seguro que quieres eliminar a {res.Nombre}?",
                    "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    using (var client = new HttpClient())
                    {
                        string url = $"https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Responsables/{res.Id}";

                        HttpResponseMessage response = await client.DeleteAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Usuario eliminado correctamente.");

                            await VisualizarUsuarios();
                        }
                        else
                        {
                            MessageBox.Show($"Error al eliminar: {res.Nombre}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No se pudo obtener la información del Usuario.");
                return;
            }
        }

        private async void btnAbilitarCuenta_Click(object sender, RoutedEventArgs e)
        {
            Button botonPresionado = sender as Button;
            Responsable lab = botonPresionado.DataContext as Responsable;

            if (lab == null)
            {
                MessageBox.Show("No se pudo obtener información del usuario");
                return;
            }

   
            if (lab.Rol == "ADMINISTRADOR")
            {
                MessageBox.Show("No se puede cambiar el estado de una cuenta de Administrador.");
                return;
            }

            if (lab.Activado == false)
            {
                lab.Activado = null;
            }
 
            else if (lab.Activado == null)
            {
                lab.Activado = false;
            }
   
            else
            {
                lab.Activado = false;
            }

            using (var client = new HttpClient())
            {
                try
                {
                    string url = $"https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Responsables/{lab.Id}";

      
                    var json = JsonConvert.SerializeObject(lab);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {

                        await VisualizarUsuarios();
                    }
                    else
                    {
                        MessageBox.Show("No se guardaron los cambios en el servidor.");

                        await VisualizarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar con el servidor: {ex.Message}");
                }
            }
        }
    }
}
