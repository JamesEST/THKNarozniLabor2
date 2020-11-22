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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            design();
        }
        private void design()
        {
            panel6.Visible = false;
        }
        private void designhide()
        {
            
            if (panel6.Visible == true)
                panel6.Visible = false;
        }
        private void designshow(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                designhide();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }
        private Form activeform = null;
        private void OpenPanelChild(Form childForm)
        {
            if (activeform != null)
                activeform.Close();
            activeform = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel2.Controls.Add(childForm);
            panel2.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenPanelChild(new Form3());
        }

        

        private void button10_Click(object sender, EventArgs e)
        {

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            designshow(panel6);
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenPanelChild(new Form2());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenPanelChild(new Form4());
        }
    }
}
