using MerchantGalaxyLib.Symbols;
using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantGalaxyLib.Commands
{
    public abstract class Command
    {
        public IReadOnlyList<Symbol> Symbols { get; set; }

        protected Command(IReadOnlyList<Symbol> symbols)
        {
            Symbols = symbols;
        }

        public override string ToString()
        {
            return string.Join(" ", Symbols);
        }
    }
}
