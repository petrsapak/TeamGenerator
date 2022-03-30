using System;
using System.Globalization;
using System.Windows.Controls;

namespace TeamGenerator.Validators
{
    public class NumericValidator : ValidationRule
    {
        public Type ValidationType { get; set; }
        public string AdditionalParam { get; set; }

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
                    if (AdditionalParam == "PositiveOnly")
                    {
                        canConvert = intValue > 0;
                    }
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int32");
                case "Double":
                    canConvert = double.TryParse(strValue, out double doubleValue);
                    if (AdditionalParam == "PositiveOnly")
                    {
                        canConvert = doubleValue > 0;
                    }
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Double");
                case "Int64":
                    canConvert = long.TryParse(strValue, out long longValue);
                    if (AdditionalParam == "PositiveOnly")
                    {
                        canConvert = longValue > 0;
                    }
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of Int64");
                case "UInt64":
                    canConvert = ulong.TryParse(strValue, out ulong ulongValue);
                    if (AdditionalParam == "PositiveOnly")
                    {
                        canConvert = ulongValue > 0;
                    }
                    return canConvert ? new ValidationResult(true, null) : new ValidationResult(false, $"Input should be type of UInt64");
                default:
                    throw new InvalidCastException($"{ValidationType.Name} is not supported");
            }
        }
    }
}
