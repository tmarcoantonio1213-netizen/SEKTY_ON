using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media; // Para Brushes

namespace SEKTY_ON.Desing.Clases
{
    public class EstadosIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string estado = value?.ToString()?.ToUpper() ?? "";

            switch (estado)
            {
                case "OCUPADO":
                    return "/Desing/Iconos/estadoLaboratorio/clase.png";
                case "LIMPIEZA":
                    return "/Desing/Iconos/estadoLaboratorio/limpieza.png";
                case "MANTENIMIENTO":
                    return "/Desing/Iconos/estadoLaboratorio/mantenimiento.png";
                default:
                    return "/Desing/Iconos/estadoLaboratorio/libre.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}