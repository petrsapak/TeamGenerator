using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TeamGenerator.Converters
{
    internal class ReduceStringLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueAsString = value.ToString();
            int parameterAsInt = int.Parse((string)parameter);

            if (valueAsString.Length > parameterAsInt)
            {
                valueAsString = valueAsString.Substring(0, parameterAsInt);

                valueAsString = RemoveSuffix(valueAsString, " ");
                valueAsString = RemoveSuffix(valueAsString, ",");
                valueAsString += "...";
            }

            return valueAsString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string RemoveSuffix(string value, string suffix)
        {
            if (value.EndsWith(suffix))
            {
                return value = value.Substring(0, value.Length - suffix.Length);
            }

            return value;
        }
    }
} 