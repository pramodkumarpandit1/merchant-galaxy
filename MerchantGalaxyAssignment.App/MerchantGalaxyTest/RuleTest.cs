using MerchantGalaxyLib;
using MerchantGalaxyLib.Symbols;
using System.Collections.Generic;
using Xunit;

namespace MerchantGalaxyTest
{
    public class RuleTest
    {
        private Rule rule;
        public RuleTest()
        {
            rule = new Rule();
        }

        [Fact]
        public void Test_Get_Symbol_For_Operators_Keywords()
        {
            const string sentence = Keywords.Operators.Is;

            rule.Init(sentence);

            Symbol symbol = rule.GetNextSymbol();

            Assert.NotNull(symbol);
            Assert.Equal(SymbolKind.Operator, symbol.Kind);

        }

        [Fact]
        public void Test_Get_Symbol_For_Qualifiers_Keywords()
        {
            const string sentence = Keywords.Qualifiers.QueryCommandQualifier;

            rule.Init(sentence);

            Symbol symbol = rule.GetNextSymbol();

            Assert.NotNull(symbol);
            Assert.Equal(SymbolKind.QueryFilter, symbol.Kind);
        }
        [Fact]
        public void Test_Get_Symbol_For_RomanKeywords()
        {
            List<string> keywords = new List<string>
            {
                Keywords.RomanSymbols.I,
                Keywords.RomanSymbols.V,
                Keywords.RomanSymbols.X,
                Keywords.RomanSymbols.L,
                Keywords.RomanSymbols.C,
                Keywords.RomanSymbols.D,
                Keywords.RomanSymbols.M,
            };

            string sentence = string.Join(" ", keywords);

            rule.Init(sentence);

            Symbol symbol = rule.GetNextSymbol();
            int symbolsCount = 0;

            while (symbol != null)
            {
                Assert.Equal(SymbolKind.RomanSymbol, symbol.Kind);

                symbol = rule.GetNextSymbol();
                symbolsCount++;
            }

            Assert.Equal(keywords.Count, symbolsCount);
        }

        [Fact]
        public void Test_Get_Symbol_For_Statements_Keywords()
        {
            const string sentence = Keywords.Statements.How;

            rule.Init(sentence);

            Symbol symbol = rule.GetNextSymbol();

            Assert.NotNull(symbol);
            Assert.Equal(SymbolKind.Statement, symbol.Kind);
        }
        [Fact]
        public void Test_Get_Symbol_For_SubStatements_Keywords()
        {
            List<string> keywords = new List<string>
            {
                Keywords.SubStatements.Many,
                Keywords.SubStatements.Much
            };

            string sentence = string.Join(" ", keywords);

            rule.Init(sentence);

            Symbol symbol = rule.GetNextSymbol();
            int symbolsCount = 0;

            while (symbol != null)
            {
                Assert.Equal(SymbolKind.SubStatemant, symbol.Kind);

                symbol = rule.GetNextSymbol();
                symbolsCount++;
            }

            Assert.Equal(keywords.Count, symbolsCount);
        }

        [Fact]
        public void Tes_Get_Symbol_ForConstant()
        {
            const string sentence = "Test";

            rule.Init(sentence);

            Symbol symbol = rule.GetNextSymbol();

            Assert.Equal(SymbolKind.Constant, symbol.Kind);
        }

        [Fact]
        public void Test_Get_Symbol_For_categoryAndUnit()
        {
            const string sentence = "one Silver is 17 Rupee";

            rule.Init(sentence);

            rule.GetNextSymbol(); //skip one
            Symbol category = rule.GetNextSymbol();
            rule.GetNextSymbol(); //skip is
            rule.GetNextSymbol(); //skip 32
            Symbol unit = rule.GetNextSymbol();

            Assert.Equal(SymbolKind.Category, category.Kind);
            Assert.Equal(SymbolKind.Unit, unit.Kind);
        }

        [Fact]
        public void Test_Get_Symbol_For_CategoryAndUnit_Query()
        {
            const string sentence = "how many Rupee is one Silver ?";

            rule.Init(sentence);

            rule.GetNextSymbol(); //skip how
            rule.GetNextSymbol(); //skip many
            Symbol unit = rule.GetNextSymbol();
            rule.GetNextSymbol(); //skip is
            rule.GetNextSymbol(); //skip one
            Symbol category = rule.GetNextSymbol();

            Assert.Equal(SymbolKind.Category, category.Kind);
            Assert.Equal(SymbolKind.Unit, unit.Kind);
        }
    }
}
