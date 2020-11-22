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
            History.Clear();
            historyCounter = 0;
            Bitmap pic = new Bitmap(750, 500);
            PictureBoxChild.Image = pic;
            History.Add(new Bitmap(PictureBoxChild.Image));

            Graphics g = Graphics.FromImage(PictureBoxChild.Image);
            g.Clear(Color.White);
            g.DrawImage(PictureBoxChild.Image, 0, 0, 750, 500);

            if (PictureBoxChild.Image != null && historyCounter != 0)
            {
                var result = MessageBox.Show("Сохранить текушие изображение перед тем как создать новое?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: toolStripButton3_Click(sender, e); break;
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
                        this.PictureBoxChild.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.PictureBoxChild.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.PictureBoxChild.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        this.PictureBoxChild.Image.Save(fs, ImageFormat.Png);
                        break;
                }
                fs.Close();
            }
        }

        private void PictureBoxChild_MouseUp(object sender, MouseEventArgs e)
        {
            History.Add(new Bitmap(PictureBoxChild.Image));
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

        private void PictureBoxChild_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = e.X.ToString() + ", " + e.Y.ToString();
            if (drawing)
            {
                Graphics g = Graphics.FromImage(PictureBoxChild.Image);
                currentPath.AddLine(oldLocation, e.Location);
                g.DrawPath(currentPen, currentPath);
                oldLocation = e.Location;
                g.Dispose();
                PictureBoxChild.Invalidate();

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            OP.Title = "Open an Image File";
            OP.FilterIndex = 1;

            if (OP.ShowDialog() != DialogResult.Cancel)
            {
                PictureBoxChild.Image = new Bitmap(OP.FileName);

                //pictureBox2.Load(OP.FileName);
            }



            PictureBoxChild.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void PictureBoxChild_MouseDown(object sender, MouseEventArgs e)
        {
            if (PictureBoxChild.Image == null)
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

       

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (History.Count != 0 && historyCounter != 0)
            {
                PictureBoxChild.Image = new Bitmap(History[--historyCounter]);
            }
            else MessageBox.Show("История пуста", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void renoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (++historyCounter < History.Count)
            {
                historyCounter--;
                PictureBoxChild.Image = new Bitmap(History[++historyCounter]);
            }
            else
            {
                MessageBox.Show("История пуста", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
