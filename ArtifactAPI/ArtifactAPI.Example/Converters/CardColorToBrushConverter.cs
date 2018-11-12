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
    public class CardColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enums.CardColor color = (Enums.CardColor)value;
            switch(color)
            {
                case Enums.CardColor.Black:
                    return new SolidColorBrush(Color.FromArgb(255, 52, 52, 52));
                case Enums.CardColor.Blue:
                    return new SolidColorBrush(Color.FromArgb(255, 26, 68, 93));
                case Enums.CardColor.Green:
                    return new SolidColorBrush(Color.FromArgb(255, 37, 87, 49));
                case Enums.CardColor.Red:
                    return new SolidColorBrush(Color.FromArgb(255, 81, 15, 29));
                case Enums.CardColor.None:
                    return new SolidColorBrush(Color.FromArgb(255, 82, 65, 39)); //Is an item
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
