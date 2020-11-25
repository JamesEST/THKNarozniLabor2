using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THKNarozniLabor2
{
    public partial class Form3 : Form
    {
        bool drawing;
        int historyCounter;
        GraphicsPath currentPath;
        Point oldLocation;
        Pen currentPen;
        Color historyColor;
        List<Image> History;
        Form2 newForm = new Form2();
        public Form3()
        {
            InitializeComponent();
            drawing = false;
            currentPen = new Pen(Color.Black);
            historyColor = currentPen.Color;
            Console.WriteLine(historyColor);
            currentPen.Width = trackBar1.Value;
            History = new List<Image>();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
           
           
        }

        private void PictureBoxChild_MouseUp(object sender, MouseEventArgs e)
        {

        }
          

        public void ColorPen(Color color)
        {
            currentPen.Color = color;
        }

        private void PictureBoxChild_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void PictureBoxChild_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

       

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void renoToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void solidToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            currentPen.DashStyle = DashStyle.Solid;
            //solidToolStripMenuItem.Checked = true;
            //dotToolStripMenuItem.Checked = false;
            //dashDotToolStripMenuItem.Checked = false;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentPen.Width = trackBar1.Value;
        }
    }
}
