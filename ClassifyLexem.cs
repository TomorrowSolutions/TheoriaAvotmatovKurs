using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheoriaAvotmatov
{
    
    internal class ClassifyLexem
    {
        char[] oneRazd;
        string[] dubRazd;
        List<WordType> lexems;
        public static string[] keyWords = new string[] {"int","main","double","float","for","if"};
        public static List<int> literals = new List<int>();
        public static List<string> variables = new List<string>();
        List<WordType> result = new List<WordType>();
        public ClassifyLexem(char[] odinrazd, string[] dubrazd, List<WordType> impLexems )
        {
            oneRazd = odinrazd;
            dubRazd = dubrazd;
            lexems = impLexems;
        }
        public List<WordType> classificate()
        {            
            foreach (var kvpair in lexems)
            {
                switch (kvpair.getType())
                {
                    case "Literal":
                        {                            
                            int.TryParse(kvpair.getWord(), out int n);
                            bool isFind = false;
                            foreach (int l in literals)
                            {
                                if (n == l) { 
                                    isFind = true;                                    
                                    result.Add(new WordType("literals", literals.IndexOf(n).ToString()));
                                    break; 
                                }
                            }
                            if (!isFind)
                            {
                                literals.Add(n);
                                result.Add(new WordType("literals",literals.IndexOf(n).ToString()));
                            }
                            break;
                        }
                    case "Razdelitel":
                        {
                            bool isFind = false;
                            foreach (char r in oneRazd)
                            {
                                
                                if (r.ToString() == kvpair.getWord())
                                {
                                    isFind = true;
                                    result.Add(new WordType("oneRazd", Array.IndexOf(oneRazd, r).ToString()));
                                    break;
                                }
                                
                            }
                            if (!isFind)
                            {
                                foreach (string dr in dubRazd)
                                {
                                    if (dr==kvpair.getWord())
                                    {
                                        isFind = true;
                                        result.Add(new WordType("dubRazd", Array.IndexOf(dubRazd, kvpair.getWord()).ToString()));
                                    }
                                }
                            }
                            break ;
                        }
                    case "Identificator":
                        {
                            bool isFind = false;
                            foreach (string kWord in keyWords)
                            {
                                if (kWord==kvpair.getWord())
                                {
                                    isFind = true;
                                    result.Add(new WordType("keyWords", Array.IndexOf(keyWords, kvpair.getWord()).ToString()));
                                    break ;
                                }
                            }
                            if (!isFind)
                            {
                                foreach (string v in variables)
                                {
                                    if (v==kvpair.getWord())
                                    {
                                        isFind=true;
                                        result.Add(new WordType("variables", variables.IndexOf(kvpair.getWord()).ToString()));
                                        break;
                                    }
                                }
                                if (!isFind)
                                {
                                    variables.Add(kvpair.getWord());
                                    result.Add(new WordType("variables", variables.IndexOf(kvpair.getWord()).ToString()));
                                    break;
                                }
                            }
                            break;
                        }                        
                    default:
                        break;
                }
            }            
            return result;
        }
    }
}
