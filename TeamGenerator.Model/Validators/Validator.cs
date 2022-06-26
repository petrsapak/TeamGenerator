using System;
using System.Linq;

namespace TeamGenerator.Model.Validators
{
    internal class Validator
    {
        internal static void ValidateString(string value)
        {
            ValidateNotNull(value);
            if (value == string.Empty || IsWhiteSpace(value))
                throw new ArgumentException("Empty or white space only strings are not allowed.");
        }

        private static bool IsWhiteSpace(string value)
        {
            return value.All(char.IsWhiteSpace);
        }

        internal static void ValidateDouble(double? value)
        {
            ValidateNotNull(value);
        }

        private static void ValidateNotNull(object value)
        {
            if (value is null)
                throw new ArgumentNullException("Null values are not allowed.");
        }
    }
}
