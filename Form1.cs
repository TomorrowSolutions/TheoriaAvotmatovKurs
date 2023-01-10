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
            LexemFinder lf = new LexemFinder(textBox1.Text);
            List<WordType> words = lf.find();
            if (words==null)
            {                
                return;
            }
            dataGridView1.Rows.Clear();
            foreach (var kvPair in words)
            {
                string[] row = new string[] { kvPair.getWord(), kvPair.getType() };
                dataGridView1.Rows.Add(row);
            }
            tabControl1.SelectedIndex = 1;
            expOneRazd = LexemFinder.razd;
            expTwoRazd = LexemFinder.dividers;
            expLex = words;
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
