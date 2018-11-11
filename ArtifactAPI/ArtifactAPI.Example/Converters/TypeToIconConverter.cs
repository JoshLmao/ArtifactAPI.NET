using ArtifactAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ArtifactAPI.Example.Converters
{
    class TypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GenericCard card = (GenericCard)value;
            string type = (string)card.Type;
            switch (type.ToLower())
            {
                case "creep":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_creep.png", UriKind.Absolute));
                case "item":
                    {
                        return ConvertFromSubtype(card.SubType);
                    }
                case "armor":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_armor.png", UriKind.Absolute));
                case "health":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_health.png", UriKind.Absolute));
                case "improvement":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_improvement.png", UriKind.Absolute));
                case "spell":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_spell.png", UriKind.Absolute));
                case "weapon":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_weapon.png", UriKind.Absolute));

                default:
                    return null;
            }

            throw new NotImplementedException("");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object ConvertFromSubtype(string subType)
        {
            if (string.IsNullOrEmpty(subType))
                return null;

            switch (subType.ToLower())
            {
                case "armor":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_armor.png", UriKind.Absolute));
                case "weapon":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_weapon.png", UriKind.Absolute));
                case "accessory":
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_improvement.png", UriKind.Absolute));
            }
            throw new NotImplementedException($"Not implemented '{subType}' sub type!");
        }
    }
}
