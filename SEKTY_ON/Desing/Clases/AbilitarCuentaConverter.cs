using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace SEKTY_ON.Desing.Clases
{
    public class AbilitarCuentaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? estaAbilitado = value as bool?;

            if (estaAbilitado == true)
            {
                return "Habilitado";
            }
            else if (estaAbilitado == false)
            {
                return "Habilitado";
            }
            else
            {
                return "Deshabilitado";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
