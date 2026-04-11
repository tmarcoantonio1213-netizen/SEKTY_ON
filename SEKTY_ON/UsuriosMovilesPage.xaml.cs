using Newtonsoft.Json;
using SEKTY_ON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SEKTY_ON
{
    /// <summary>
    /// Lógica de interacción para UsuriosMovilesPage.xaml
    /// </summary>
    public partial class UsuriosMovilesPage : Page
    {
        public UsuriosMovilesPage()
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
                    string url = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Usuarios";
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);

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

        private void btnAgregarUsuario_Click(object sender, RoutedEventArgs e)
        {
            RegistroWindow ventanaRegistro = new RegistroWindow();
            ventanaRegistro.Show();
            Window.GetWindow(this)?.Close();
        }

        private async void btnAbilitarCuenta_Click(object sender, RoutedEventArgs e)
        {
            Button botonPresionado = sender as Button;
            Usuario user = botonPresionado?.DataContext as Usuario;

            if (user == null)
            {
                MessageBox.Show("No se pudo obtener información del usuario");
                return;
            }


            if (user.Abilitado == true)
            {

                user.Abilitado = false;
            }
            else if (user.Abilitado == false)
            {

                user.Abilitado = null;
            }
            else
            {

                user.Abilitado = true;
            }

            using (var client = new HttpClient())
            {
                try
                {
                    string url = $"https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Usuarios/{user.Id}";
                    var json = JsonConvert.SerializeObject(user);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {

                        await VisualizarUsuarios();
                    }
                    else
                    {
                        MessageBox.Show("No se pudieron guardar los cambios en el servidor.");
                        await VisualizarUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error de conexión: {ex.Message}");
                }
            }
        }
    }

   
    public class AbilitarCuentaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? estado = (bool?)value;

            if (estado == true) return "Habilitado";
            if (estado == false) return "Deshabilitado";
            return "Pendiente"; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}