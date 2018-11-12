using ArtifactAPI.Enums;
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
            GenericCard card = value is GenericCard ? (GenericCard)value : null;
            if (card == null)
                return null;

            switch (card.Type)
            {
                case CardType.Creep:
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_creep.png", UriKind.Absolute));
                case CardType.Item:
                    {
                        return ConvertFromSubtype(card.SubType);
                    }
                case CardType.Armor:
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_armor.png", UriKind.Absolute));
                case CardType.Health:
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_health.png", UriKind.Absolute));
                case CardType.Improvement:
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_improvement.png", UriKind.Absolute));
                case CardType.Spell:
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_spell.png", UriKind.Absolute));
                case CardType.Weapon:
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

        private object ConvertFromSubtype(CardType subType)
        {
            switch (subType)
            {
                case CardType.Armor:
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_armor.png", UriKind.Absolute));
                case CardType.Weapon:
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_weapon.png", UriKind.Absolute));
                case CardType.Accessory:
                    //return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_improvement.png", UriKind.Absolute));
                    return new BitmapImage(new Uri("pack://application:,,,/ArtifactAPI.Example;component/Images/card_type_health.png", UriKind.Absolute));
            }
            throw new NotImplementedException($"Not implemented '{subType}' sub type!");
        }
    }
}
