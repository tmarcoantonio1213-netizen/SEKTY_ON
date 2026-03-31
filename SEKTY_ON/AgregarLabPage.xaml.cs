using Newtonsoft.Json;
using SEKTY_ON.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SEKTY_ON
{
    /// <summary>
    /// Lógica de interacción para AgregarLabPage.xaml
    /// </summary>
    public partial class AgregarLabPage : Page
    {
        public AgregarLabPage()
        {
            InitializeComponent();
        }

        private async void btnAgregarLab_Click(object sender, RoutedEventArgs e)
        {
            string nombreLab = txtNombreLab.Text;
            string edificioLab = txtEdificio.Text;
            int edificio;

            if (string.IsNullOrWhiteSpace(nombreLab) || string.IsNullOrWhiteSpace(edificioLab))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }
            else
            {
                if (nombreLab.Length > 100)
                {
                    MessageBox.Show("Nombre de laboratorio: No ingreses mas de 100 caracteres.");
                    return;
                }
                if (int.TryParse(edificioLab, out int numeroEdificio) != true)
                {
                    MessageBox.Show("Numero de edificio: Solo ingresa numeros enteros");
                    return;
                }
                else
                {
                    if (numeroEdificio > 100)
                    {
                        MessageBox.Show("Numero de edificio: No ingreses valores mayores a 100");
                        return;
                    }
                    edificio = numeroEdificio;
                }

            }

            var laboratorio = new { Nombre = nombreLab, Edificio = edificio, EstadoId = 1, Abierto = false };

            try
            {
                using (var client = new HttpClient())
                {
                    string url = "https://localhost:7060/api/Laboratorios";

                    var json = JsonConvert.SerializeObject(laboratorio);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();

                        Laboratorio usuarioLogueado = JsonConvert.DeserializeObject<Laboratorio>(responseBody);

                        MessageBox.Show("Laboratorio agregado exitosamente.");

                        this.NavigationService.Navigate(new LaboratoriosPage());
                    }
                    else
                    {
                        var errorBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error al agregar laboratorio: {errorBody}");
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
