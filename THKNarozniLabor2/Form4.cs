using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THKNarozniLabor2
{
    public partial class Form4 : Form
    {
        Graphics g;
        int x = -1;
        int y = -1;
        bool moving = false;
        Pen pen;
        List<Image> History;

        List<Point> currentLine = new List<Point>();
        List<List<Point>> curves = new List<List<Point>>();
        int historyCounter;


       
        public Form4()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //pen = new Pen(Color.Black, 5);
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
           // moving = true;
           // x = e.X;
           // y = e.Y;
           // panel1.Cursor = Cursors.Cross;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {

            if (currentLine.Count > 1) curves.Add(currentLine.ToList());  // copy!!
            currentLine.Clear();
            panel1.Invalidate();

          //  moving = false;
          //  x = -1;
          //  y = -1;
          //  panel1.Cursor = Cursors.Default;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                currentLine.Add(e.Location);
                panel1.Invalidate();
            }
            //if(moving && x!=-1 && y!= -1)
            //{
            //    g.DrawLine(pen, new Point(x, y), e.Location);
            //    x = e.X;
            //    y = e.Y;
            //    currentLine.Add(e.Location);
            //    panel1.Invalidate();
            //}



        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (History.Count != 0 && historyCounter != 0)
            {
                panel1.BackgroundImage = new Bitmap(History[--historyCounter]);
            }
            else MessageBox.Show("История пуста", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (++historyCounter < History.Count)
            {
                historyCounter--;
                panel1.BackgroundImage = new Bitmap(History[++historyCounter]);
            }
            else
            {
                MessageBox.Show("История пуста", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    panel1.BackgroundImage = Image.FromFile(open.FileName);
                    panel1.BackgroundImageLayout = ImageLayout.Zoom;
                }


            } 

            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                Bitmap bmp = new Bitmap(this.panel1.Width, this.panel1.Height);
                this.panel1.DrawToBitmap(bmp, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
                bmp.Save(@"D:\TestDrawToBitmap.bmp", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Pen somePen = new Pen(Color.Black, 5.5f))
            {
                somePen.StartCap = somePen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                if (currentLine.Count > 1) e.Graphics.DrawCurve(somePen, currentLine.ToArray());
                foreach (List<Point> lp in curves)
                    if (lp.Count > 1) e.Graphics.DrawCurve(somePen, lp.ToArray());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            curves.Clear(); currentLine.Clear(); panel1.Invalidate();
        }
    }
}
