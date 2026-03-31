using Newtonsoft.Json;
using SEKTY_ON.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Lógica de interacción para LaboratoriosPage.xaml
    /// </summary>
    public partial class LaboratoriosPage : Page
    {
        public LaboratoriosPage()
        {
            InitializeComponent();

            _ = VisualizarLaboratorios();

            // Cargar datos de laboratorios (esto se haría normalmente desde la base de datos)

            
        }

        private async Task VisualizarLaboratorios()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    string url = "https://localhost:7060/api/Laboratorios";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Laboratorio> laboratorios = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Laboratorio>>(json);

                        if (laboratorios != null)
                        {
                            lstBxLaboratorios.ItemsSource = laboratorios;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudieron cargar los Laboratorios.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private async void btnAbrirPuerta_Click(object sender, RoutedEventArgs e)
        {
            Button botonPresionado = sender as Button;
            Laboratorio lab = botonPresionado.DataContext as Laboratorio;

            if (lab != null)
            {
                lab.Abierto = !lab.Abierto;
            }
            else
            {
                MessageBox.Show("No se pudo obtener la información del laboratorio.");
            }

            using (var client = new HttpClient())
            {
                string url = $"https://localhost:7060/api/Laboratorios/{lab.Id}";

                var json = JsonConvert.SerializeObject(lab);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await VisualizarLaboratorios();
                }
                else
                {
                    MessageBox.Show("No se guardaron los cambios.");
                    lab.Abierto = !lab.Abierto;
                }
            }
        }

        private async void btnEliminarLaboratorio_Click(object sender, RoutedEventArgs e)
        {
            Button botonPresionado = sender as Button;
            Laboratorio lab = botonPresionado.DataContext as Laboratorio;

            if (lab != null)
            {
                MessageBox.Show($"¿Seguro que quieres eliminar el laboratorio {lab.Nombre}?",
                    "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                using (var client = new HttpClient())
                {
                    string url = $"https://localhost:7060/api/Laboratorios/{lab.Id}";

                    HttpResponseMessage response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Laboratorio eliminado correctamente.");

                        await VisualizarLaboratorios();
                    }
                    else
                    {
                        MessageBox.Show($"Error al eliminar: {lab.Nombre}");
                    }
                }
            }
            else
            {
                MessageBox.Show("No se pudo obtener la información del laboratorio.");
                return;
            }

        }


        private void btnAgregarLaboratorio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AgregarLabPage());
        }
    }
}
