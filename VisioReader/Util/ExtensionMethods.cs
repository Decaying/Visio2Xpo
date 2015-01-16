using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace cvo.buyshans.Visio2Xpo.Communication.Util
{
    public static class ExtensionMethods
    {
        public static String FindInFormula(this String formula, String function)
        {
            var functionPattern = @"^" + function + @"(.*)$";
            const String remainPattern = @"^[^\(]*\((.*)\)$";

            var results = Regex.Matches(formula, functionPattern).OfType<Match>().ToList();
            var remain = Regex.Matches(formula, remainPattern).OfType<Match>().ToList();

            if (results.Any())
            {
                return results.First().Value;
            }

            if (remain.Any())
            {
                return remain.First().Groups[1].Value.FindInFormula(function);
            }

            return null;
        }
    }
}