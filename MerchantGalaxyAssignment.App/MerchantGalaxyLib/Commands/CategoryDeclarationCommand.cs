using MerchantGalaxyLib.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantGalaxyLib.Commands
{
    public class CategoryDeclarationCommand : Command
    {
        public CategoryDeclarationCommand(IReadOnlyList<Symbol> symbols) : base(symbols)
        {
        }
    }
}
