using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TheoriaAvotmatov
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = File.ReadAllText("example.txt");
        }
        char[] expOneRazd;
        string[] expTwoRazd;
        List<WordType> expLex;
        private void button2_Click(object sender, EventArgs e)
        {
            char[] razd = new char[] { '+', '-', '*', '/', ';', ':', '=', ',', '>', '<', '(', ')', '}', '{','!'};
            string[] dividers = new string[] { "++", "--", "<=", ">=","==","!=" };
            Regex ASCIILettersOnly = new Regex(@"^[\P{L}A-Za-z]*$");
            string buffer = string.Empty;            
            List<WordType> words = new List<WordType>();
            if (ASCIILettersOnly.IsMatch(textBox1.Text))
            {
                for(int i=0;i<textBox1.Text.Length;i++)
                {
                    
                    if (Char.IsLetterOrDigit(textBox1.Text[i]))
                    {
                        if (Char.IsDigit(textBox1.Text[i]))
                        {
                            if (i >=textBox1.Text.Length-1)
                            {
                                words.Add(new WordType(textBox1.Text[i].ToString(), "Literal"));
                            }
                            else
                            {
                                int numind= i;
                                string other = textBox1.Text.Substring(numind + 1);
                                var multiDigit = new List<int>();
                                multiDigit.Add((int)Char.GetNumericValue(textBox1.Text[i]));
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
                        else if (Char.IsLetter(textBox1.Text[i]))
                        {
                            if (i >= textBox1.Text.Length - 1)
                            {
                                words.Add(new WordType(textBox1.Text[i].ToString(), "Identificator"));
                            }
                            else
                            {
                                int letterIndex = i;
                                string other = textBox1.Text.Substring(letterIndex + 1);
                                var word = new List<char>();
                                word.Add(textBox1.Text[i]);
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
                                i+=iter;
                            }
                        }                        
                    }
                    else if(textBox1.Text[i] ==' '|| textBox1.Text[i] == '\n' || textBox1.Text[i] == '\r' || textBox1.Text[i] == '\t')
                    {
                        continue;
                    }
                    else
                    {
                        if (i >= textBox1.Text.Length-1)
                        {
                            
                            foreach (char r  in razd)
                            {
                                if (textBox1.Text[i]==r)
                                {
                                    words.Add(new WordType(textBox1.Text[i].ToString(), "Razdelitel"));
                                    break;
                                }
                            }
                           
                        }
                        else
                        {
                            bool isFind = false;
                            foreach (char r in razd)
                            {
                                if (textBox1.Text[i] == r)
                                {
                                    int razdind=i;
                                    isFind = true;
                                    bool isDoubleDivider = false;
                                    bool isFindDuplicate = false;
                                    int iter = 0;
                                    buffer= textBox1.Text[i].ToString();
                                    foreach (char r2 in razd)
                                    {
                                        if (textBox1.Text[razdind+1]==r2)
                                        {                                            
                                            string dr = textBox1.Text[i].ToString() + textBox1.Text[razdind + 1].ToString();
                                            
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
                                        buffer = textBox1.Text[razdind + 1].ToString();
                                        words.Add(new WordType(buffer, "Razdelitel"));
                                    }
                                    else if (!isDoubleDivider && !isFindDuplicate)
                                    {
                                        words.Add(new WordType(buffer, "Razdelitel"));
                                    }                                    
                                    buffer = string.Empty;                                                                                                           
                                    i +=iter;
                                    break;
                                }                                
                            }
                            if (isFind == false)
                            {
                                char unk = textBox1.Text[i];
                                MessageBox.Show("Неизвестный символ!"+$"\"{unk}\"");
                                return;
                            }
                        }                        
                    }
                }
                dataGridView1.Rows.Clear();                
                foreach (var kvPair in words)
                {
                    string[] row = new string[] { kvPair.getWord(), kvPair.getType() };
                    dataGridView1.Rows.Add(row);
                }
                tabControl1.SelectedIndex = 1;
                expOneRazd = razd;
                expTwoRazd = dividers;
                expLex = words;                
            }
            else
            {
                MessageBox.Show("Non-english letter");
                return;
            }

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.InitialDirectory = @"H:\projects\СSharp\TheoriaAvotmatov\bin\Debug";
            ofd.Title = "Выберите файл";
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClassifyLexem cl = new ClassifyLexem(expOneRazd, expTwoRazd, expLex);
            List<WordType> classificated= cl.classificate();
            dataGridView2.Rows.Clear();//keywords
            dataGridView3.Rows.Clear();//onerazd
            dataGridView4.Rows.Clear();//tworazd
            dataGridView5.Rows.Clear();//literals
            dataGridView6.Rows.Clear();//variables
            dataGridView7.Rows.Clear();//classificated
            foreach (string k in ClassifyLexem.keyWords)
            {
                string[] row = new string[] { Array.IndexOf(ClassifyLexem.keyWords,k).ToString(), k };
                dataGridView2.Rows.Add(row);
            }
            foreach (char r in expOneRazd)
            {
                string[] row = new string[] { Array.IndexOf(expOneRazd, r).ToString(), r.ToString() };
                dataGridView3.Rows.Add(row);
            }
            foreach (string dr in expTwoRazd)
            {
                string[] row = new string[] { Array.IndexOf(expTwoRazd, dr).ToString(), dr};
                dataGridView4.Rows.Add(row);
            }
            foreach (int l in ClassifyLexem.literals)
            {
                string[] row = new string[] { ClassifyLexem.literals.IndexOf(l).ToString(), l.ToString() };
                dataGridView5.Rows.Add(row);
            }
            foreach (string v in ClassifyLexem.variables)
            {
                string[] row = new string[] { ClassifyLexem.variables.IndexOf(v).ToString(), v };
                dataGridView6.Rows.Add(row);
            }
            foreach (var kvPair in classificated)
            {
                string[] row = new string[] { kvPair.getWord(), kvPair.getType() };
                dataGridView7.Rows.Add(row);
            }
            tabControl1.SelectedIndex = 2;
        }
    }
}
