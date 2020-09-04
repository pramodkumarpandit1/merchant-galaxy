using MerchantGalaxyLib;
using MerchantGalaxyLib.Symbols;
using Xunit;

namespace MerchantGalaxyTest
{
    public class TranslatorTest
    {
        [Fact]
        public void Test_Constant_Symbol()
        {
            Translator translator = new Translator();
            translator.ParseAndExecute("glob is I");
            ConstantSymbol globSymbol = new ConstantSymbol("glob");
            RomanSymbol iSymbol = new RomanSymbol("I");

            Assert.True(translator.Processor.ConstantsDictionary.ContainsKey(globSymbol));
            Assert.Equal(iSymbol, translator.Processor.ConstantsDictionary[globSymbol]);
        }

        [Fact]
        public void Test_Category_for_Symbol()
        {
            Translator translator = new Translator();
            translator.ParseAndExecute("glob is I");
            translator.ParseAndExecute("glob glob Silver is 34 Credits");

            CategorySymbol silverSymbol = new CategorySymbol("Silver");
            UnitSymbol creditsSymbol = new UnitSymbol("Credits");
            bool isContains = translator.Processor.CategoriesDictionary[silverSymbol].Contains(creditsSymbol);
            Assert.True(translator.Processor.CategoriesDictionary.ContainsKey(silverSymbol));
            Assert.True(isContains);
            Assert.Equal(17, translator.Processor.CategoriesDictionary[silverSymbol].Find(s => s.Equals(creditsSymbol)).Factor);
        }

        [Fact]
        public void Compute_Simple_Input()
        {
            Translator translator = new Translator();
            translator.ParseAndExecute("glob is I");
            translator.ParseAndExecute("pish is X");
            translator.ParseAndExecute("tegj is L");

            MerchantGalaxyLib.Commands.CommandResult result = translator.ParseAndExecute("how much is pish tegj glob glob ?");
            Assert.Equal("pish tegj glob glob is 42", result.ResultText);
        }

        [Fact]
        public void Compute_Unit_Traslator()
        {
            Translator translator = new Translator();
            translator.ParseAndExecute("glob is I");
            translator.ParseAndExecute("prok is V");
            translator.ParseAndExecute("pish is X");
            translator.ParseAndExecute("tegj is L");

            translator.ParseAndExecute("glob glob Silver is 34 Credits");
            translator.ParseAndExecute("glob prok Gold is 57800 Credits");
            translator.ParseAndExecute("pish pish Iron is 3910 Credits");

            MerchantGalaxyLib.Commands.CommandResult resultSilver = translator.ParseAndExecute("how many Credits is glob prok Silver ?");
            MerchantGalaxyLib.Commands.CommandResult resultGold = translator.ParseAndExecute("how many Credits is glob prok Gold ?");
            MerchantGalaxyLib.Commands.CommandResult resultIron = translator.ParseAndExecute("how many Credits is glob prok Iron ?");

            Assert.Equal("glob prok Silver is 68 Credits", resultSilver.ResultText);
            Assert.Equal("glob prok Gold is 57800 Credits", resultGold.ResultText);
            Assert.Equal("glob prok Iron is 782 Credits", resultIron.ResultText);
        }

        [Fact]
        public void Test_Invalid_Queries()
        {
            Translator translator = new Translator();

            MerchantGalaxyLib.Commands.CommandResult result = translator.ParseAndExecute("how much flower is a flowerbukey bukey if a flowrbukey is flower bukey ?");

            Assert.Equal("I have no idea what you are talking about", result.ResultText);
        }
    }
}
