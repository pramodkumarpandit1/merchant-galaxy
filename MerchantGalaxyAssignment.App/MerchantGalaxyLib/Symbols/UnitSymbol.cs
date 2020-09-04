using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantGalaxyLib.Symbols
{
    public class UnitSymbol : Symbol
    {
        public double Factor { get; set; }

        public UnitSymbol(string name)
            : base(name, SymbolKind.Unit)
        {
        }
    }
}
