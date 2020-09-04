using System;
using System.Collections.Generic;
using System.Text;

namespace MerchantGalaxyLib.Symbols
{
    public class Symbol
    {
        public SymbolKind Kind { get; private set; }
        public string Name { get; private set; }

        public Symbol(String name, SymbolKind kind)
        {
            Name = name;
            Kind = kind;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            return obj != null
                && obj.GetType() == GetType()
                && ((Symbol)obj).Name == Name;
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return Name;
        }

        public double ToDouble()
        {
            double result;
            if (!double.TryParse(Name, out result))
                throw new Exception("Symbol is not a valid double");

            return result;
        }
    }
}
