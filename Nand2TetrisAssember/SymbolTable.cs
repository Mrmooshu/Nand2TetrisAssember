using System;
using System.Collections;

namespace Nand2TetrisAssember
{
    class SymbolTable
    {
        private Hashtable table;

        public SymbolTable()
        {
            table = new Hashtable();
        }

        public void addEntry(String symbol, int address)
        {
            table.Add(symbol, address);
        }

        public bool contains(String symbol)
        {
            return table.ContainsKey(symbol);
        }

        public int getAddress(String symbol)
        {
            return (int)table[symbol];
        }
    }
}
