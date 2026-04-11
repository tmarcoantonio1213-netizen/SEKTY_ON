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

namespace SEKTY_ON
{
    public partial class PeticionesPage : Page
    {
        private List<Peticion> todasLasPeticiones = new List<Peticion>();
        private string urlApi = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Peticiones";

        public PeticionesPage()
        {
            InitializeComponent();
            _ = VisualizarPeticiones();
        }

        private async Task VisualizarPeticiones()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"{urlApi}/Peticiones");
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        todasLasPeticiones = JsonConvert.DeserializeObject<List<Peticion>>(json);
                        lvPeticiones.ItemsSource = todasLasPeticiones;
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private async void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var p = (Peticion)((Button)sender).DataContext;
            txtMensajeOcupado.Visibility = Visibility.Collapsed;

            using (var client = new HttpClient())
            {
                try
                {
                    var resLab = await client.GetAsync($"{urlApi}/Laboratorios/{p.LaboratorioId}");
                    if (resLab.IsSuccessStatusCode)
                    {
                        string content = await resLab.Content.ReadAsStringAsync();
                        
                        if (content.ToLower().Contains("\"abierto\":false"))
                        {
                            txtMensajeOcupado.Visibility = Visibility.Visible;
                            return;
                        }
                    }

                    p.Estatus = true;
                    await Actualizar(p);
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private async void BtnRechazar_Click(object sender, RoutedEventArgs e)
        {
            var p = (Peticion)((Button)sender).DataContext;
            txtMensajeOcupado.Visibility = Visibility.Collapsed;
            p.Estatus = false;
            await Actualizar(p);
        }

        private async Task Actualizar(Peticion p)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(p);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync($"{urlApi}/Peticiones/{p.Id}", content);
                await VisualizarPeticiones();
            }
        }

        private void FiltroEstado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvPeticiones == null || todasLasPeticiones == null) return;

            ComboBoxItem item = (ComboBoxItem)cbFiltro.SelectedItem;
            if (item == null) return;

            string filtro = item.Content.ToString();

            if (filtro == "Pendientes")
                lvPeticiones.ItemsSource = todasLasPeticiones.Where(x => x.Estatus == null).ToList();
            else if (filtro == "Aprobadas")
                lvPeticiones.ItemsSource = todasLasPeticiones.Where(x => x.Estatus == true).ToList();
            else if (filtro == "Rechazadas")
                lvPeticiones.ItemsSource = todasLasPeticiones.Where(x => x.Estatus == false).ToList(); 
            else
                lvPeticiones.ItemsSource = todasLasPeticiones;
        }
    }
}