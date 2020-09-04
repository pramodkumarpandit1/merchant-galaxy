using MerchantGalaxyLib.Symbols;
using System.Collections.Generic;

namespace MerchantGalaxyLib.Commands
{
    public class ConstantDeclarationCommand : Command
    {
        public ConstantDeclarationCommand(IReadOnlyList<Symbol> symbols) : base(symbols)
        {
        }
    }
}