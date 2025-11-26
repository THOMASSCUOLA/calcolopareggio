using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace calcolopareggio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("quantità", "quantità");
            dataGridView1.Columns.Add("domanda", "domanda");
            dataGridView1.Columns.Add("offerta", "offerta");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int yyy=0;
            dataGridView1.Rows.Clear();

           
            double a = double.Parse(txtdomandafissa.Text);        // domanda costante
            double b = double.Parse(txtcoefficenteDomanda.Text);  // coeff domanda

            double c = double.Parse(txtoffertafissa.Text);        // offerta costante
            double k = double.Parse(txtcoefficenteofferta.Text);  // coeff offerta (CORRETTO)
            int exp = int.Parse(txtcoefficenteEsponente.Text);    // esponente offerta

            // valori iniziali
            double q = 0.0;
            double step = 0.01;

            double d = a - b * q;
            double o = c + k * Math.Pow(q, exp);

            dataGridView1.Rows.Add(
     Math.Round(q, 3),
     Math.Round(d, 3),
     Math.Round(o, 3)
 );


            // =============================
            // CERCA PUNTO DI EQUILIBRIO
            // =============================

            bool equilibrioTrovato = false;
            double equilibrioQ = 0, equilibrioD = 0, equilibrioO = 0;

            double qPrec = 0, dPrec = 0, oPrec = 0; // valori precedenti
            int safety = 500000;
            int count = 0;

            while (count < safety)
            {
                count++;
                q += step;
                d = a - b * q;
                o = c + k * Math.Pow(q, exp);

                dataGridView1.Rows.Add(
                    Math.Round(q, 3),
                    Math.Round(d, 3),
                    Math.Round(o, 3));

                if (!equilibrioTrovato && o >= d)
                {
                    // Interpolazione lineare tra ultimo punto e punto corrente
                    double t = (dPrec - oPrec) / ((o - oPrec) - (d - dPrec));
                    equilibrioQ = qPrec + t * (q - qPrec);
                    equilibrioD = a - b * equilibrioQ;
                    equilibrioO = c + k * Math.Pow(equilibrioQ, exp);

                    equilibrioTrovato = true;
                    break;
                }

                // memorizzo valori precedenti
                qPrec = q;
                dPrec = d;
                oPrec = o;
            }

            // =============================
            // AGGIUNGO PUNTI DOPO EQUILIBRIO
            // =============================
            for (int i = 0; i < 50; i++)
            {
                q += step;
                d = a - b * q;
                o = c + k * Math.Pow(q, exp);

                dataGridView1.Rows.Add(
                    Math.Round(q, 3),
                    Math.Round(d, 3),
                    Math.Round(o, 3));
            }

            // =============================
            // MOSTRA PUNTO DI EQUILIBRIO
            // =============================
            if (equilibrioTrovato)
            {
                yyy++;
                MessageBox.Show(
                    $"Punto di equilibrio trovato:\n" +
                    $"q = {Math.Round(equilibrioQ, 5)}\n" +
                    $"d = {Math.Round(equilibrioD, 5)}\n" +
                    $"o = {Math.Round(equilibrioO, 5)}",
                    "Equilibrio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }


            // =============================
            // GRAFICO
            // =============================
            chart1.Series["Domanda"].Points.Clear();
            chart1.Series["Offerta"].Points.Clear();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value == null) continue;

                double qVal = Convert.ToDouble(row.Cells[0].Value);
                double dVal = Convert.ToDouble(row.Cells[1].Value);
                double oVal = Convert.ToDouble(row.Cells[2].Value);

                chart1.Series["Domanda"].Points.AddXY(qVal, dVal);
                chart1.Series["Offerta"].Points.AddXY(qVal, oVal);
            }

            chart1.ChartAreas[0].AxisX.Title = "quantità";
            chart1.ChartAreas[0].AxisY.Title = "prezzo";
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0.##";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0.##";

            chart1.Titles.Clear();
            chart1.Titles.Add("aggiornamento sw");

            // =============================
            // PUNTO DI EQUILIBRIO (pallino rosso)
            // =============================

            if (equilibrioTrovato)
            {
                if (chart1.Series.IndexOf("Equilibrio") != -1)
                {
                    chart1.Series.RemoveAt(chart1.Series.IndexOf("Equilibrio"));
                }

                var serieEquilibrio = chart1.Series.Add("Equilibrio");
                serieEquilibrio.ChartType = SeriesChartType.Point;
                serieEquilibrio.MarkerStyle = MarkerStyle.Circle;
                serieEquilibrio.MarkerSize = 7;
                serieEquilibrio.Color = Color.Red;

                serieEquilibrio.Points.AddXY(equilibrioQ, equilibrioD);

                serieEquilibrio.Points[0].Label =
                    $"q={Math.Round(equilibrioQ, 5)}\n" +
                    $"d={Math.Round(equilibrioD, 5)}\n" +
                    $"o={Math.Round(equilibrioO, 5)}";

                serieEquilibrio.Points[0].LabelForeColor = Color.Black;
            }
        }
    }
}
