using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var laboratorios = new List<object>
            {
                new { Nombre = "Laboratorio de Redes", Ubicacion = "Edificio A" },
                new { Nombre = "Laboratorio de Cómputo 1", Ubicacion = "Edificio B" },
                new { Nombre = "Sala de Innovación Digital", Ubicacion = "Edificio C" },
                new { Nombre = "Laboratorio de Redes", Ubicacion = "Edificio A" },
                new { Nombre = "Laboratorio de Cómputo 1", Ubicacion = "Edificio B" },
                new { Nombre = "Sala de Innovación Digital", Ubicacion = "Edificio C" },
                new { Nombre = "Laboratorio de Redes", Ubicacion = "Edificio A" },
                new { Nombre = "Laboratorio de Cómputo 1", Ubicacion = "Edificio B" },
                new { Nombre = "Sala de Innovación Digital", Ubicacion = "Edificio C" },
                new { Nombre = "Laboratorio de Redes", Ubicacion = "Edificio A" },
                new { Nombre = "Laboratorio de Cómputo 1", Ubicacion = "Edificio B" },
                new { Nombre = "Sala de Innovación Digital", Ubicacion = "Edificio C" },
                new { Nombre = "Laboratorio de Redes", Ubicacion = "Edificio A" }
            };

            lstBxLaboratorios.ItemsSource = laboratorios;
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
            MainWindow ventanaUsuarios = new MainWindow();
            ventanaUsuarios.Show();
            this.Close();
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            UsuariosWindow ventanaUsuarios = new UsuariosWindow();
            ventanaUsuarios.Show();
            this.Close();
        }
    }
}
