using ArtifactAPI.Enums;
using ArtifactAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ArtifactAPI.Example.Converters
{
    public class LanguageConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            Languages languages = value[0] as Languages;
            if (languages == null)
                return null;

            Language lang = (value[1] as MainWindow).SetLanguage;
            return languages.GetTranslation(lang);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
