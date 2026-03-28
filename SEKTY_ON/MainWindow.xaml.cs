using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SEKTY_ON.Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            bool perfilActivo = false;

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


                        if (responsables != null)
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
                            irAlLogin();
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

        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
