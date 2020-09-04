using MerchantGalaxyLib.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantGalaxyLib
{
    public class Rule
    {
        private string[] _arguments;
        private int _currentReadPosition;

        public void Init(string commandText)
        {
            _arguments = LoadArguments(commandText);
            _currentReadPosition = 0;
        }

        public Symbol GetNextSymbol()
        {
            Symbol ret;

            if (_arguments.Length == _currentReadPosition)
                return null;

            var lastArg = _arguments[_currentReadPosition];

            switch (lastArg)
            {
                case Keywords.Operators.Is:
                    ret = new Symbol(lastArg, SymbolKind.Operator);
                    break;
                case Keywords.Qualifiers.QueryCommandQualifier:
                    ret = new Symbol(lastArg, SymbolKind.QueryFilter);
                    break;
                case Keywords.RomanSymbols.I:
                case Keywords.RomanSymbols.V:
                case Keywords.RomanSymbols.X:
                case Keywords.RomanSymbols.L:
                case Keywords.RomanSymbols.C:
                case Keywords.RomanSymbols.D:
                case Keywords.RomanSymbols.M:
                    ret = new RomanSymbol(lastArg);
                    break;
                case Keywords.Statements.How:
                    ret = new Symbol(lastArg, SymbolKind.Statement);
                    break;
                case Keywords.SubStatements.Many:
                case Keywords.SubStatements.Much:
                    ret = new Symbol(lastArg, SymbolKind.SubStatemant);
                    break;
                default:
                    ret = GetDeclarationSymbol(lastArg);
                    break;
            }
            _currentReadPosition++;
            return ret;
        }

        private Symbol GetDeclarationSymbol(string lastArg)
        {
            double doubleTest;
            if (double.TryParse(lastArg, out doubleTest))
                return new Symbol(lastArg, SymbolKind.ValueDefinition);

            var previousArg = SeekPrevious();

            if (previousArg == null)
                return new ConstantSymbol(lastArg);

            var nextArg = SeekNext();

            if (nextArg == null || (IsSubstatement(previousArg) && IsOperator(nextArg)))
                return new UnitSymbol(lastArg);

            if (!IsSubstatement(previousArg) && IsOperator(nextArg))
                return new CategorySymbol(lastArg);

            if (IsQueryQualifier(nextArg) && IsMensurableCommand())
                return new CategorySymbol(lastArg);

            return new ConstantSymbol(lastArg);
        }

        #region Aux Methods
        private static string[] LoadArguments(string commandTex)
        {
            return commandTex.Split(' ');
        }

        private string SeekNext()
        {
            return _currentReadPosition == _arguments.Length - 1 ? null : _arguments[_currentReadPosition + 1];
        }

        private string SeekPrevious()
        {
            return _currentReadPosition == 0 ? null : _arguments[_currentReadPosition - 1];
        }

        private static bool IsOperator(string arg)
        {
            return arg == Keywords.Operators.Is;
        }

        private static bool IsSubstatement(string arg)
        {
            return arg == Keywords.SubStatements.Many
                   || arg == Keywords.SubStatements.Much;
        }

        private static bool IsQueryQualifier(string arg)
        {
            return arg == Keywords.Qualifiers.QueryCommandQualifier;
        }

        private bool IsMensurableCommand()
        {
            return _arguments.Contains(Keywords.SubStatements.Many);
        }
        #endregion
    }
}
