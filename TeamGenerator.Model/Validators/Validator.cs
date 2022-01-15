using System;

namespace TeamGenerator.Model.Validators
{
    internal class Validator
    {
        internal static void ValidateString(string value)
        {
            ValidateNotNull(value);
            if (value == string.Empty || IsWhiteSpace(value)) ;
                //throw new ArgumentException("Empty or white space only strings are not allowed.");
        }

        private static bool IsWhiteSpace(string value)
        {
            for(int i = 0; i < value.Length; i++)
            {
                if(!Char.IsWhiteSpace(value[i])) return false;
            }

            return true;
        }

        internal static void ValidateDouble(double? value)
        {
            ValidateNotNull(value);
        }

        private static void ValidateNotNull(object value)
        {
            if(value == null)
                throw new ArgumentNullException("Null values are not allowed.");
        }
    }
}
