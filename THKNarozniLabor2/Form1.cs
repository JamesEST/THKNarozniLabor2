using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap pic = new Bitmap(750, 500);
            pictureBox2.Image = pic;

            if(pictureBox2.Image != null)
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

        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if(pictureBox2.Image == null)
            {
                MessageBox.Show("Создай сначало новый файл");
                return;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            savefile.Title = "Save an Image File";
            savefile.FilterIndex = 4;
            savefile.ShowDialog();

            if(savefile.FileName != "")
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
    }
}
