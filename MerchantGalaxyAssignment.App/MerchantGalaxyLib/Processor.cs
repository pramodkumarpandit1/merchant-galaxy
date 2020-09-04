using MerchantGalaxyLib.Commands;
using MerchantGalaxyLib.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantGalaxyLib
{
    public class Processor
    {
        public Dictionary<ConstantSymbol, RomanSymbol> ConstantsDictionary { get; private set; }
        public Dictionary<CategorySymbol, List<UnitSymbol>> CategoriesDictionary { get; private set; }

        public Processor()
        {
            ConstantsDictionary = new Dictionary<ConstantSymbol, RomanSymbol>();
            CategoriesDictionary = new Dictionary<CategorySymbol, List<UnitSymbol>>();
        }

        public CommandResult ExecuteCommand(ConstantDeclarationCommand declaration)
        {
            var constantSymbol = (ConstantSymbol)declaration.Symbols.Single(s => s is ConstantSymbol);

            if (ConstantsDictionary.ContainsKey(constantSymbol))
                throw new DuplicatedDeclarationException();

            var romanSymbol = (RomanSymbol)declaration.Symbols.Single(s => s is RomanSymbol);

            ConstantsDictionary.Add(constantSymbol, romanSymbol);

            return new CommandResult
            {
                ResultText = String.Format("Information Registred: \"{0}\"", declaration),
                Sucess = true
            };
        }

        public CommandResult ExecuteCommand(CategoryDeclarationCommand declaration)
        {
            var Category = (CategorySymbol)declaration.Symbols.Single(s => s is CategorySymbol);
            var unit = (UnitSymbol)declaration.Symbols.Single(s => s is UnitSymbol);
            var value = declaration.Symbols.Single(s => s.Kind == SymbolKind.ValueDefinition).ToDouble();

            unit.Factor = ComputeUnitFactor(declaration.Symbols.OfType<ConstantSymbol>(), value);

            if (!CategoriesDictionary.ContainsKey(Category))
                CategoriesDictionary.Add(Category, new List<UnitSymbol>());

            if (!CategoriesDictionary[Category].Contains(unit))
                CategoriesDictionary[Category].Add(unit);
            else
                throw new DuplicatedDeclarationException();

            return new CommandResult
            {
                ResultText = String.Format("Information Registred: \"{0}\"", declaration),
                Sucess = true
            };
        }

        public CommandResult ExecuteCommand(QueryCommand query)
        {
            var queryType = query.Symbols.Single(s => s.Kind == SymbolKind.SubStatemant).Name;

            string messageText;

            var constants = query.Symbols.OfType<ConstantSymbol>().ToList();
            var value = GetDecimalValue(constants);
            var constantsName = string.Join(" ", constants.Select(c => c.ToString()));

            if (queryType == Keywords.SubStatements.Much)
                messageText = string.Format("{0} is {1}", constantsName, value);

            else
            {
                var Category = (CategorySymbol)query.Symbols.Single(s => s is CategorySymbol);
                var unit = (UnitSymbol)query.Symbols.Single(s => s is UnitSymbol);

                value *= CategoriesDictionary[Category].Find(u => u.Equals(unit)).Factor;

                messageText = string.Format("{0} {1} is {2} {3}", constantsName, Category, value, unit);
            }

            return new CommandResult
            {
                ResultText = messageText,
                Sucess = true
            };

        }
        public double ComputeUnitFactor(IEnumerable<ConstantSymbol> constants, double value)
        {
            return value / GetDecimalValue(constants);
        }

        private double GetDecimalValue(IEnumerable<ConstantSymbol> constants)
        {
            var romanSymbols = new StringBuilder();

            constants.Select(c => ConstantsDictionary[c])
                     .ToList().ForEach(r => romanSymbols.Append(r));

            var romanNumber = romanSymbols.ToString();

            return RomanToDecimalConverter.Convert(romanNumber);
        }

        
    }
}
