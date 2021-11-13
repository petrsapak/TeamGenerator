using System;
using System.Globalization;
using System.Windows.Controls;

namespace TeamGenerator.Shell.Validators
{
    public class NumericValidator : ValidationRule
    {
        public Type ValidationType { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value cannot be coverted to string.");

            bool canConvert;

            switch (ValidationType.Name)
            {
                case "Boolean":
                    canConvert = bool.TryParse(strValue, out bool boolVal);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of boolean");
                case "Int32":
                    canConvert = int.TryParse(strValue, out int intValue);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int32");
                case "Double":
                    canConvert = double.TryParse(strValue, out double doubleValue);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");
                case "Int64":
                    canConvert = long.TryParse(strValue, out long longValue);
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int64");
                default:
                    throw new InvalidCastException($"{ValidationType.Name} is not supported");
            }
        }
    }
}
