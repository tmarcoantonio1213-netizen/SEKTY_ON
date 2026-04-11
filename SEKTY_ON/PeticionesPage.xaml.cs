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
        private string urlApi = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api";

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
                        ActualizarVistaConFiltro();
                    }
                }
                catch (Exception ex) { Console.WriteLine("Error API: " + ex.Message); }
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
                        string jsonLimpio = content.Replace(" ", "").Replace("\n", "").Replace("\r", "");

                        if (!jsonLimpio.Contains("\"estadoId\":1"))
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
                try
                {
                    p.Laboratorio = null;
                    p.Usuario = null;

                    var json = JsonConvert.SerializeObject(p);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync($"{urlApi}/Peticiones/{p.Id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        await VisualizarPeticiones();
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Error al actualizar: " + error);
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void FiltroEstado_SelectionChanged(object sender, SelectionChangedEventArgs e) => ActualizarVistaConFiltro();

        private void ActualizarVistaConFiltro()
        {
            if (lvPeticiones == null || todasLasPeticiones == null) return;
            if (cbFiltro.SelectedItem is ComboBoxItem item)
            {
                string filtro = item.Content.ToString();
                switch (filtro)
                {
                    case "Pendientes": lvPeticiones.ItemsSource = todasLasPeticiones.Where(x => x.Estatus == null).ToList(); break;
                    case "Aprobadas": lvPeticiones.ItemsSource = todasLasPeticiones.Where(x => x.Estatus == true).ToList(); break;
                    case "Rechazadas": lvPeticiones.ItemsSource = todasLasPeticiones.Where(x => x.Estatus == false).ToList(); break;
                    default: lvPeticiones.ItemsSource = todasLasPeticiones.ToList(); break;
                }
            }
        }
    }
}