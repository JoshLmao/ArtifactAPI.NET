using ArtifactAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ArtifactAPI.Example.Converters
{
    class DisplayCostByTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GenericCard card = value is GenericCard ? (GenericCard)value : null;
            if (card == null)
                return null;

            if (card.Type == Enums.CardType.Item)
            {
                return card.GoldCost;
            }
            else
            {
                return card.ManaCost;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
