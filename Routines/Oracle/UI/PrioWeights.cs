using System.Globalization;
using System.Linq;
using Oracle.Healing;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Oracle.UI
{
    public partial class PrioWeights : Form
    {
        private static PrioWeights instance = new PrioWeights();

        public static void Display()
        {
            if (instance == null || instance.IsDisposed)
                instance = new PrioWeights();
            if (!instance.Visible)
            {
                instance.Show();
                instance.timer1.Start();
            }
        }

        private PrioWeights()
        {
            InitializeComponent();
            paint();
        }

        private void paint()
        {
            this.Controls.Clear();

            var W = 400;
            var dy = 20;

            var y = 0;
            var wieght = OracleHealTargeting.weightsPG.OrderByDescending(w => w.Item1); 

            var col1 = 10;
            var col2 = (int)(col1 + (W - 20.0) * 0.6);
            //var col3 = (int)(col2 + (W - 20.0) * 0.4 / 2.0);

            foreach (var x in wieght)
            {
                var color = x.Item1 <= 0 ? Color.DarkGreen : Color.Red;

                var w = Math.Round(x.Item1,0);

                var A = new Label();
                A.Text = w.ToString(CultureInfo.InvariantCulture);
                A.ForeColor = color;
                A.Size = new Size(col2 - col1, dy);
                A.TextAlign = ContentAlignment.MiddleLeft;
                A.Location = new Point(col1, y);

                var B = new Label();
                B.Text = x.Item2.Name;
                B.ForeColor = color;
                B.Size = new Size(100, dy);
                B.TextAlign = ContentAlignment.MiddleLeft;
                B.Location = new Point(col2, y);

                //var C = new Label();
                //C.Text = x.Item1 > 200 ? "Up" : "Down";
                //C.ForeColor = color;
                //C.Size = new Size(100, dy);
                //C.TextAlign = ContentAlignment.MiddleLeft;
                //C.Location = new Point(col3, y);

                this.Controls.Add(A);
                this.Controls.Add(B);
                //this.Controls.Add(C);

                y += dy;
            }

            this.ClientSize = new Size(W, y + 80);
            this.MinimumSize = this.ClientSize;
            this.MaximumSize = this.ClientSize;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            paint();
        }

    }
}