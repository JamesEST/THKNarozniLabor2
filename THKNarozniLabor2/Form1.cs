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
        bool drawing;
        int historyCounter;
        GraphicsPath currentPath;
        Point oldLocation;
        Pen currentPen;
        Color historyColor;
        List<Image> History;
        Form2 newForm = new Form2();
        public Form1()
        {
            InitializeComponent();
            drawing = false;
            currentPen = new Pen(Color.Black);
            historyColor = currentPen.Color;
            Console.WriteLine(historyColor);
            currentPen.Width = trackBar1.Value;
            History = new List<Image>();
            design();
        }
        private void design()
        {
            panel5.Visible = false;
            panel6.Visible = false;
        }
        private void designhide()
        {
            if (panel5.Visible == true)
                panel5.Visible = false;
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            History.Clear();
            historyCounter = 0;
            Bitmap pic = new Bitmap(750, 500);
            pictureBox2.Image = pic;
            History.Add(new Bitmap(pictureBox2.Image));

            Graphics g = Graphics.FromImage(pictureBox2.Image);
            g.Clear(Color.White);
            g.DrawImage(pictureBox2.Image, 0, 0, 750, 500);

            if (pictureBox2.Image != null && historyCounter != 0)
            {
                var result = MessageBox.Show("Сохранить текушие изображение перед тем как создать новое?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: saveToolStripMenuItem_Click(sender, e); break;
                }
            }
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentPen.Width = trackBar1.Value;
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox2.Image == null)
            {
                MessageBox.Show("Создай сначало новый файл", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();
                currentPen.Color = CheckColor();
                historyColor = currentPen.Color;
            }
            if (e.Button == MouseButtons.Right)
            {
                historyColor = currentPen.Color;
                drawing = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();

                Console.WriteLine(historyColor);
                currentPen.Color = System.Drawing.Color.White;

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            savefile.Title = "Save an Image File";
            savefile.FilterIndex = 4;
            savefile.ShowDialog();

            if (savefile.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)savefile.OpenFile();

                switch (savefile.FilterIndex)
                {
                    case 1:
                        this.pictureBox2.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.pictureBox2.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.pictureBox2.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        this.pictureBox2.Image.Save(fs, ImageFormat.Png);
                        break;
                }
                fs.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            History.Add(new Bitmap(pictureBox2.Image));
            historyCounter++;

            if (historyCounter > 10)
            {
                History.RemoveAt(0);
                historyCounter--;
            }

            drawing = false;
            currentPen.Color = CheckColor();
            currentPen.Color = historyColor;
            try
            {
                currentPath = new GraphicsPath();
                currentPath.Dispose();
            }
            catch { };
        }

        private Color CheckColor()
        {
            return newForm.GetColor();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = e.X.ToString() + ", " + e.Y.ToString();
            if (drawing)
            {
                Graphics g = Graphics.FromImage(pictureBox2.Image);
                currentPath.AddLine(oldLocation, e.Location);
                g.DrawPath(currentPen, currentPath);
                oldLocation = e.Location;
                g.Dispose();
                pictureBox2.Invalidate();

            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (History.Count != 0 && historyCounter != 0)
            {
                pictureBox2.Image = new Bitmap(History[--historyCounter]);
            }
            else MessageBox.Show("История пуста", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void renoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (++historyCounter < History.Count)
            {
                historyCounter--;
                pictureBox2.Image = new Bitmap(History[++historyCounter]);
            }
            else
            {
                MessageBox.Show("История пуста", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void solidToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            currentPen.DashStyle = DashStyle.Solid;
            solidToolStripMenuItem.Checked = true;
            dotToolStripMenuItem.Checked = false;
            dashDotToolStripMenuItem.Checked = false;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            History.Clear();
            historyCounter = 0;
            Bitmap pic = new Bitmap(750, 500);
            pictureBox2.Image = pic;
            History.Add(new Bitmap(pictureBox2.Image));

            Graphics g = Graphics.FromImage(pictureBox2.Image);
            g.Clear(Color.White);
            g.DrawImage(pictureBox2.Image, 0, 0, 750, 500);

            if (pictureBox2.Image != null && historyCounter != 0)
            {
                var result = MessageBox.Show("Сохранить текушие изображение перед тем как создать новое?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: saveToolStripMenuItem_Click(sender, e); break;
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            savefile.Title = "Save an Image File";
            savefile.FilterIndex = 4;
            savefile.ShowDialog();

            if (savefile.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)savefile.OpenFile();

                switch (savefile.FilterIndex)
                {
                    case 1:
                        this.pictureBox2.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.pictureBox2.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.pictureBox2.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        this.pictureBox2.Image.Save(fs, ImageFormat.Png);
                        break;
                }
                fs.Close();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            OP.Title = "Open an Image File";
            OP.FilterIndex = 1;

            if (OP.ShowDialog() != DialogResult.Cancel)
            {
                pictureBox2.Image = new Bitmap(OP.FileName);
                
                //pictureBox2.Load(OP.FileName);
            }
                

           
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newForm = new Form2();
            newForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            designshow(panel5);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenPanelChild(new Form2());
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
