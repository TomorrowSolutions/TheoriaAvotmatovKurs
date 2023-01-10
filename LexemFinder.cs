using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheoriaAvotmatov
{
    internal class LexemFinder
    {
        public static char[] razd = new char[] { '+', '-', '*', '/', ';', ':', '=', ',', '>', '<', '(', ')', '}', '{', '!' };
        public static string[] dividers = new string[] { "++", "--", "<=", ">=", "==", "!=" };
        Regex ASCIILettersOnly = new Regex(@"^[\P{L}A-Za-z]*$");
        string buffer = string.Empty;
        String text;

        public LexemFinder(String source)
        {
            text = source;
        }
        public List<WordType> find()
        {            
            List<WordType> words = new List<WordType>();
            if (ASCIILettersOnly.IsMatch(text))
            {
                for (int i = 0; i < text.Length; i++)
                {

                    if (Char.IsLetterOrDigit(text[i]))
                    {
                        if (Char.IsDigit(text[i]))
                        {
                            if (i >= text.Length - 1)
                            {
                                words.Add(new WordType(text[i].ToString(), "Literal"));
                            }
                            else
                            {
                                int numind = i;
                                string other = text.Substring(numind + 1);
                                var multiDigit = new List<int>();
                                multiDigit.Add((int)Char.GetNumericValue(text[i]));
                                int iter = 0;
                                foreach (char c2 in other)
                                {
                                    if (Char.IsDigit(c2))
                                    {
                                        multiDigit.Add((int)Char.GetNumericValue(c2));
                                        iter++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                words.Add(new WordType(String.Join("", multiDigit), "Literal"));
                                i += iter;
                            }
                        }
                        else if (Char.IsLetter(text[i]))
                        {
                            if (i >= text.Length - 1)
                            {
                                words.Add(new WordType(text[i].ToString(), "Identificator"));
                            }
                            else
                            {
                                int letterIndex = i;
                                string other = text.Substring(letterIndex + 1);
                                var word = new List<char>();
                                word.Add(text[i]);
                                int iter = 0;
                                foreach (char c2 in other)
                                {
                                    if (char.IsLetterOrDigit(c2))
                                    {
                                        word.Add(c2);
                                        iter++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                words.Add(new WordType(string.Join("", word), "Identificator"));
                                i += iter;
                            }
                        }
                    }
                    else if (text[i] == ' ' || text[i] == '\n' || text[i] == '\r' || text[i] == '\t')
                    {
                        continue;
                    }
                    else
                    {
                        if (i >= text.Length - 1)
                        {

                            foreach (char r in razd)
                            {
                                if (text[i] == r)
                                {
                                    words.Add(new WordType(text[i].ToString(), "Razdelitel"));
                                    break;
                                }
                            }

                        }
                        else
                        {
                            bool isFind = false;
                            foreach (char r in razd)
                            {
                                if (text[i] == r)
                                {
                                    int razdind = i;
                                    isFind = true;
                                    bool isDoubleDivider = false;
                                    bool isFindDuplicate = false;
                                    int iter = 0;
                                    buffer = text[i].ToString();
                                    foreach (char r2 in razd)
                                    {
                                        if (text[razdind + 1] == r2)
                                        {
                                            string dr = text[i].ToString() + text[razdind + 1].ToString();

                                            foreach (string d in dividers)
                                            {
                                                if (d == dr)
                                                {
                                                    buffer = dr;
                                                    isDoubleDivider = true;
                                                    iter++;
                                                    break;
                                                }
                                            }
                                            if (isDoubleDivider)
                                            {
                                                words.Add(new WordType(buffer, "Razdelitel"));
                                            }
                                            else
                                            {
                                                isFindDuplicate = true;
                                                iter++;
                                            }
                                            break;
                                        }
                                    }
                                    if (isFindDuplicate)
                                    {
                                        words.Add(new WordType(buffer, "Razdelitel"));
                                        buffer = text[razdind + 1].ToString();
                                        words.Add(new WordType(buffer, "Razdelitel"));
                                    }
                                    else if (!isDoubleDivider && !isFindDuplicate)
                                    {
                                        words.Add(new WordType(buffer, "Razdelitel"));
                                    }
                                    buffer = string.Empty;
                                    i += iter;
                                    break;
                                }
                            }
                            if (isFind == false)
                            {
                                char unk = text[i];
                                MessageBox.Show("Неизвестный символ!" + $"\"{unk}\"");
                                return null;
                            }
                        }
                    }
                }                
            }
            else
            {
                MessageBox.Show("Non-english letter");
                return null;
            }
            return words;

        }
    }
}
