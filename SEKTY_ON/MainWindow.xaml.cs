using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SEKTY_ON.Models;
using System.Net.Http;

namespace SEKTY_ON
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _ = ValidarYRefrescarPerfil();
        }

        private async Task ValidarYRefrescarPerfil()
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


                        if (responsables != null && responsables.Count > 0)
                        {
                            var usuarioActivo = responsables.FirstOrDefault(r => r.Activado == true);

                            if (usuarioActivo != null)
                            {
                                this.DataContext = usuarioActivo;
                            }
                            else
                            {
                                irAlLogin();
                            }
                        }
                        else
                        {
                            irAlRegistro();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al validar el estado del perfil: {ex.Message}");
                }

                
                MainFrame.Navigate(new LaboratoriosPage());
            }
        }

        private void irAlLogin()
        {
            LoginWindow ventanaLogin = new LoginWindow();
            ventanaLogin.Show();
            this.Close();
        }

        private void irAlRegistro()
        {
            RegistroWindow ventanaRegistro = new RegistroWindow();
            ventanaRegistro.Show();
            this.Close();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (colMenu.Width.Value == 230)
            {
                colMenu.Width = new GridLength(0);
            }
            else
            {
                colMenu.Width = new GridLength(230);
            }
        }

        private void btnLaboratorios_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LaboratoriosPage());
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsuariosPage());
        }

        private void btnUsuariosMoviles_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsuriosMovilesPage());
        }

        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            PerfilWindow ventanaPerfil = new PerfilWindow();
            ventanaPerfil.Show();
            this.Close();
        }

        private void btnPeticiones_Click(object sender, RoutedEventArgs e)
        {
          
            MainFrame.Navigate(new PeticionesPage());
        }
    }
    
}