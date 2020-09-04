using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MerchantGalaxyLib
{
    public static class RomanToDecimalConverter
    {
        public class InvalidRomanNumberException : Exception
        {
        }
        public class InvalidRomanSymbolException : InvalidRomanNumberException
        {
        }
        public static int Convert(string romanNumber)
        {
            if (!IsValid(romanNumber))
                throw new InvalidRomanNumberException();

            var values = romanNumber.Select(GetSymbolValue).ToArray();
            var result = 0;

            for (var i = 0; i < values.Length; i++)
            {
                var current = values[i];
                var next = i + 1 < values.Length ? values[i + 1] : 0;
                current = next > current ? -current : current;
                result += current;
            }

            return result;
        }

        private static bool IsValid(string romanNumber)
        {
            var match = Regex.Match(romanNumber, @"^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$");
            return match.Success;
        }

        private static int GetSymbolValue(char symbol)
        {
            switch (symbol)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    throw new InvalidRomanSymbolException();
            }
        }
    }
}
