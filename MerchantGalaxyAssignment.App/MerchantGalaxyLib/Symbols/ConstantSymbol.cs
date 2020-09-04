using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantGalaxyLib.Symbols
{
    public class ConstantSymbol : Symbol
    {
        public ConstantSymbol(string name)
            : base(name, SymbolKind.Constant)
        { }
    }
}
