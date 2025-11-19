using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calcolopareggio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Initialize columns for dataGridView
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("q", "q");
            dataGridView1.Columns.Add("d", "d");
            dataGridView1.Columns.Add("o", "o");
        }
        public static int d = 90 - 4 * q;
        public static double o = 10 + Math.Pow(q, 3) / 100.0;
        public static int q = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            // Clear previous results
            dataGridView1.Rows.Clear();
            int diniziale = 90 - 4 * q;
            double oiniziale = 10 + Math.Pow(q, 3) / 100.0;


            // compute initial d and o



            // Add initial row
            dataGridView1.Rows.Add(q, d, o);

            // If initially equal or d > o, stop immediately
            //if (d >= o)
            //{
            //    return;
            //}

            // iterate incrementing q by1 until d > o
            // set a safety limit to avoid infinite loops
            int safetyLimit = 10000;
            while (q < safetyLimit)
            {
                q++;
                d = 90 - 4 * q;
                o = 10 + Math.Pow(q, 3) / 100.0;
                dataGridView1.Rows.Add(q, d, o);
                if (d <= oiniziale)
                {
                    break;
                }
            }
        }
    }
}
