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
    /// Lógica de interacción para PeticionesPage.xaml
    /// </summary>
    public partial class PeticionesPage : Page
    {
        public PeticionesPage()
        {
            InitializeComponent();

            _ = VisualizarPeticiones();
        }

        private async Task VisualizarPeticiones()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Peticiones";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Peticion> peticiones = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Peticion>>(json);

                        if (peticiones != null)
                        {
                            lstBxPeticiones.ItemsSource = peticiones;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se puede cargar las peticiones");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
