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

            Enums.Colors color = Enums.Colors.Black;
            if (card.IsBlack)
                color = Enums.Colors.Black;
            else if (card.IsRed)
                color = Enums.Colors.Red;
            else if (card.IsBlue)
                color = Enums.Colors.Blue;
            else if (card.IsGreen)
                color = Enums.Colors.Green;
            else if (card.Type.ToLower() == "item")
                return new SolidColorBrush(Color.FromArgb(255, 82, 65, 39));

            FactionColorEnumToColorConverter cc = new FactionColorEnumToColorConverter();
            return cc.Convert(color, null, null, null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
