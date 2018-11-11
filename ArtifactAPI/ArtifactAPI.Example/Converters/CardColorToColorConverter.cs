using ArtifactAPI.Models;
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
    class CardColorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GenericCard card = value is GenericCard ? (GenericCard)value : null;
            if (card == null)
                return null;

            if (card.IsBlack)
                return new SolidColorBrush(Color.FromArgb(255, 52, 52, 52));
            else if (card.IsRed)
                return new SolidColorBrush(Color.FromArgb(255, 81, 15, 29));
            else if (card.IsBlue)
                return new SolidColorBrush(Color.FromArgb(255, 26, 68, 93));
            else if (card.IsGreen)
                return new SolidColorBrush(Color.FromArgb(255, 37, 87, 49));
            else if (card.Type.ToLower() == "item")
                return new SolidColorBrush(Color.FromArgb(255, 82, 65, 39));

            throw new NotImplementedException("Not implemented color!");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
