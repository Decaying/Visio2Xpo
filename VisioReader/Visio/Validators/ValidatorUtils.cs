using System;
using System.Text.RegularExpressions;

namespace cvo.buyshans.Visio2Xpo.Communication.Visio.Validators
{
    public static class ValidatorUtils
    {
        private const String StartsWithNumberPattern = "^[0-9]";
        private const String NoSpecialCharactersPattern = "^[a-zA-Z0-9_]*$";

        public static Boolean StartsWithNumber(this String source)
        {
            return Regex.Match(source, StartsWithNumberPattern).Success;
        }

        public static Boolean ContainsSpecialCharacters(this String source)
        {
            return !Regex.Match(source, NoSpecialCharactersPattern).Success;
        }
    }
}