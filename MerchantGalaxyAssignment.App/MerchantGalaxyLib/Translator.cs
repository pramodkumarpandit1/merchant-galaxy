using MerchantGalaxyLib.Commands;
using MerchantGalaxyLib.Symbols;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MerchantGalaxyLib
{
    public class Translator
    {
        private readonly Rule _rule;
        public Processor Processor { get; private set; }

        public Translator()
        {
            _rule = new Rule();
            Processor = new Processor();           
        }

        public CommandResult ParseAndExecute(string commandText)
        {
            try
            {
                _rule.Init(commandText);

                Command command = Parse();

                if (command is ConstantDeclarationCommand)
                {
                    return Processor.ExecuteCommand(command as ConstantDeclarationCommand);
                }

                if (command is CategoryDeclarationCommand)
                {
                    return Processor.ExecuteCommand(command as CategoryDeclarationCommand);
                }

                if (command is QueryCommand)
                {
                    return Processor.ExecuteCommand(command as QueryCommand);
                }
            }
            catch (DuplicatedDeclarationException)
            {
                return new CommandResult { ResultText = $"You already have ", Sucess = false  };
            }
            catch (Exception)
            {
                return new CommandResult { ResultText = @"I have no idea what you are talking about", Sucess = false };
            }

            throw new NotSupportedException(@"Command Not Supported");
        }

        private Command Parse()
        {
            IReadOnlyList<Symbol> symbols = GetSymbolsList();

            if (IsConstantDeclaration(symbols))
            {
                return new ConstantDeclarationCommand(symbols);
            }

            if (IsCategoryDeclaration(symbols))
            {
                return new CategoryDeclarationCommand(symbols);
            }

            if (IsQueryCommand(symbols))
            {
                return new QueryCommand(symbols);
            }

            throw new ParsingException();
        }

        private static bool IsQueryCommand(IReadOnlyList<Symbol> symbols)
        {
            return symbols.First().Kind == SymbolKind.Statement
                   && symbols[1].Kind == SymbolKind.SubStatemant
                   && symbols.Any(s => s.Kind == SymbolKind.Constant)
                   && symbols.Any(s => s.Kind == SymbolKind.Operator)
                   && symbols.Last().Kind == SymbolKind.QueryFilter;
        }

        private static bool IsCategoryDeclaration(IReadOnlyCollection<Symbol> symbols)
        {
            return symbols.First().Kind == SymbolKind.Constant
                   && symbols.Any(s => s.Kind == SymbolKind.Category)
                   && symbols.Any(s => s.Kind == SymbolKind.Operator)
                   && symbols.Any(s => s.Kind == SymbolKind.ValueDefinition)
                   && symbols.Last().Kind == SymbolKind.Unit;
        }

        private static bool IsConstantDeclaration(IReadOnlyCollection<Symbol> symbols)
        {
            return symbols.Count == 3
                   && symbols.First().Kind == SymbolKind.Constant
                   && symbols.Any(s => s.Kind == SymbolKind.Operator)
                   && symbols.Last().Kind == SymbolKind.RomanSymbol;
        }

        private IReadOnlyList<Symbol> GetSymbolsList()
        {
            List<Symbol> symbols = new List<Symbol>();

            Symbol lastSymbol = _rule.GetNextSymbol();

            while (lastSymbol != null)
            {
                symbols.Add(lastSymbol);
                lastSymbol = _rule.GetNextSymbol();
            }

            return new ReadOnlyCollection<Symbol>(symbols);
        }

    }
}
