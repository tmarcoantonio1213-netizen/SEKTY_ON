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
    /// Lógica de interacción para LaboratoriosPage.xaml
    /// </summary>
    public partial class LaboratoriosPage : Page
    {
        public LaboratoriosPage()
        {
            InitializeComponent();

            // Cargar datos de laboratorios (esto se haría normalmente desde la base de datos)

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

        private void btnAgregarLaboratorio_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AgregarLabPage());
        }
    }
}
