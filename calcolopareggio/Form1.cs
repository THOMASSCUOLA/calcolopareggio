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
       
        private void button1_Click(object sender, EventArgs e)
        {
            // Clear previous results
            dataGridView1.Rows.Clear();

            // valori iniziali
            double q = 0.0;
            double d = 90 - 4 * q;
            double o = 10 + Math.Pow(q, 3) / 100.0;

            double dIniziale = 90;
            double oIniziale = 10 ;

            dataGridView1.Rows.Add(q, d, o);

            // -------------------------------
            // FASE 1: ricerca precisa del punto di pareggio
            // -------------------------------

            double step = 1.0;      // passo grande all'inizio
            double minStep = 0.0001;
            bool superato = false;

            int safetyLimit = 500000;
            int counter = 0;

            while (!superato && counter < safetyLimit)
            {
                counter++;

                q += step;

                d = 90 - 4 * q;
                o = 10 + Math.Pow(q, 3) / 100.0;

            // Add initial row
            dataGridView1.Rows.Add(q, d, o);

                double diff = o - d;   // positivo = o sopra d

                // Riduzione dinamica step per precisione crescente
                if (Math.Abs(diff) < 10) step = 0.1;
                if (Math.Abs(diff) < 1) step = 0.01;
                if (Math.Abs(diff) < 0.1) step = 0.001;
                if (Math.Abs(diff) < 0.01) step = minStep;

                // ---> se superato, passa alla FASE 2
                if (o >= d)
                {
                    superato = true;
                }
            }

            // -------------------------------
            // FASE 2: arrivo fino a ~13.9 poi passo diretto a 14 e continuo +1
            // -------------------------------

            double qLimit = 13.9;      // limite prima di forzare a 14
            double qTarget = Math.Ceiling(q);  // di solito sarà 14 se sei a 13.x

            // 1) Incrementi piccoli SOLO finché q < 13.9
            while (q < qLimit && counter < safetyLimit)
            {
                counter++;

                q += 0.1;     // passo piccolo
                d = 90 - 4 * q;
                o = 10 + Math.Pow(q, 3) / 100.0;

                dataGridView1.Rows.Add(q, d, o);
            }

            // 2) Appena supero 13.9 → salto diretto al successivo intero
            q = qTarget;  // esempio: si fissa a 14
                d = 90 - 4 * q;
                o = 10 + Math.Pow(q, 3) / 100.0;
                dataGridView1.Rows.Add(q, d, o);

            // 3) Ora incremento regolare di 1 come volevi
            while (d > oIniziale && counter < safetyLimit)
                {
                counter++;

                q += 1;   // passo regolare da 1

                d = 90 - 4 * q;
                o = 10 + Math.Pow(q, 3) / 100.0;

                dataGridView1.Rows.Add(q, d, o);
            }



        }

    }
}
