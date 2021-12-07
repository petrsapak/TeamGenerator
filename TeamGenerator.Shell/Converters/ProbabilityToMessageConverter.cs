using System;
using System.Globalization;
using System.Windows.Data;

namespace TeamGenerator.Shell.Converters
{
    public class ProbabilityToMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"Estimated chance of winning: {value}%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
