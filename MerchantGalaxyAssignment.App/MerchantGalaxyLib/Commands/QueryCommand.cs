using MerchantGalaxyLib.Symbols;
using System.Collections.Generic;

namespace MerchantGalaxyLib.Commands
{
    public class QueryCommand : Command
    {
        public QueryCommand(IReadOnlyList<Symbol> symbols) : base(symbols)
        {
           
        }

    }
}
