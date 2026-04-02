using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace SEKTY_ON.Desing.Clases
{
    internal class AbilitarCuentaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool estaAbilitado = value is bool b && b;

            return estaAbilitado ? "Abilitado" : "Desabilitado";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
