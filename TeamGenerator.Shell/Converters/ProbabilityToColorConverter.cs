using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TeamGenerator.Shell.Converters
{
    internal class ProbabilityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color defaultForegroudColor = (Color)ColorConverter.ConvertFromString("#B1AFB3");
            if (value == null)
                return defaultForegroudColor;

            if (int.TryParse(value.ToString(), out int probability))
            {
                if (probability == 0)
                    return defaultForegroudColor;
                else if (probability == 50)
                    return Brushes.LightGoldenrodYellow;
                else if (probability > 50)
                    return Brushes.LightGreen;
                else
                    return Brushes.PaleVioletRed;
            }

            return defaultForegroudColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
