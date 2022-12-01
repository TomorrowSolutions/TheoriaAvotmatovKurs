using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheoriaAvotmatov
{
    internal class WordType
    {
        string Word;
        string Type;
        public WordType(string word, string type)
        {
            Word = word;
            Type = type;
        }
        public string getWord()
        {
            return Word;
        }
        public string getType()
        {
            return Type;
        }
    }
}
