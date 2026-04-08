using System;
using System.Globalization;
using System.Windows.Data;

namespace SEKTY_ON.Desing.Clases
{
    public class AbiertoIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool estaAbierto = (bool)value;

            return estaAbierto
                ? "/Desing/Iconos/estadoPuerta/puertaAbierta.png"
                : "/Desing/Iconos/estadoPuerta/puertaCerrada.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
