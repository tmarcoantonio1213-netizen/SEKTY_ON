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
    /// Lógica de interacción para AgregarLabPage.xaml
    /// </summary>
    public partial class AgregarLabPage : Page
    {
        public AgregarLabPage()
        {
            InitializeComponent();
        }

        private void btnAgregarLab_Click(object sender, RoutedEventArgs e)
        {
            string nombreLab = txtNombreLab.Text;
            string ubicacionLab = txtEdificio.Text;

            if (string.IsNullOrWhiteSpace(nombreLab) || string.IsNullOrWhiteSpace(ubicacionLab))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            // Aquí iría la lógica para agregar el laboratorio a la base de datos

            MessageBox.Show("Laboratorio agregado exitosamente.");
            this.NavigationService.Navigate(new LaboratoriosPage());
        }
    }
}
