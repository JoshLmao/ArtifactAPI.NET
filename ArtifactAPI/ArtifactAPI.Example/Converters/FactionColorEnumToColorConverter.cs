using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ArtifactAPI.Example.Converters
{
    public class FactionColorEnumToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enums.Colors color = (Enums.Colors)value;
            switch(color)
            {
                case Enums.Colors.Black:
                    return new SolidColorBrush(Color.FromArgb(255, 52, 52, 52));
                case Enums.Colors.Blue:
                    return new SolidColorBrush(Color.FromArgb(255, 26, 68, 93));
                case Enums.Colors.Green:
                    return new SolidColorBrush(Color.FromArgb(255, 37, 87, 49));
                case Enums.Colors.Red:
                    return new SolidColorBrush(Color.FromArgb(255, 81, 15, 29));
                default:
                    throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
