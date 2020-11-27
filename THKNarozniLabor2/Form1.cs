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
        bool drawing = false;
        int historyCounter;
        GraphicsPath currentPath;
        Point oldLocation;
        Pen currentPen;
        Color historyColor;
        List<Image> History;
        Form2 newForm;

        string StylePen = "Line";
        Rectangle currRect;
        Point endPoint;
        Point startPoint;
        public Form1()
        {
            this.KeyPreview = true;
            InitializeComponent();
            design();
            currentPen = new Pen(Color.Black);
            historyColor = currentPen.Color;
            History = new List<Image>();
        }
        private void design()
        {
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
        }
        private void designhide()
        {
            if (panel5.Visible == true)
                panel5.Visible = false;
            if (panel6.Visible == true)
                panel6.Visible = false;
            if (panel7.Visible == true)
                panel7.Visible = false;
        }
        private Color CheckColor()
        {
            return newForm.GetColor();
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
                activeform.Hide();
            activeform = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panel2.Controls.Add(childForm);
            panel2.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            designshow(panel5);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            designshow(panel6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //OpenPanelChild(new Form2());
            try
            {
                newForm = new Form2();
                newForm.Show();
            }
            catch (Exception)
            {
                newForm.Dispose();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //new
            
            newForm = new Form2();
            trackBar1.Visible = true;
            label1.Visible = true;
            History.Clear();
            historyCounter = 0;
            Bitmap pic = new Bitmap(750, 500);
            PictureBoxChild.Image = pic;
            History.Add(new Bitmap(PictureBoxChild.Image));

            Graphics g = Graphics.FromImage(PictureBoxChild.Image);
            g.Clear(Color.White);
            g.DrawImage(PictureBoxChild.Image, 0, 0, 750, 500);

            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //open
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|GIF Image|*.gif|PNG Image|*.png";
            OP.Title = "Open an Image File";
            OP.FilterIndex = 1;

            if (OP.ShowDialog() != DialogResult.Cancel)
            {
                PictureBoxChild.Image = new Bitmap(OP.FileName);
                PictureBoxChild.SizeMode = PictureBoxSizeMode.AutoSize;
                panel2.AutoScroll = true;

                //pictureBox2.Load(OP.FileName);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveImage();
        }
        private void SaveImage()
        {
            //save
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
        private void button5_Click(object sender, EventArgs e)
        {
            //exit
            this.Close();
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

            currentPen.Color = historyColor;
            try
            {
                currentPath = new GraphicsPath();
                currentPath.Dispose();
            }
            catch { };
        }

        private void PictureBoxChild_MouseDown(object sender, MouseEventArgs e)
        {
            
            startPoint = new Point(e.X, e.Y);
           
            if (PictureBoxChild.Image == null)
            {
                button2_Click(sender, e);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                if (StylePen == "Line")
                {
                    drawing = true;
                    oldLocation = e.Location;
                    currentPath = new GraphicsPath();
                    currentPen.Color = CheckColor();
                    historyColor = currentPen.Color;
                }
                else if (StylePen == "Square")
                {
                    drawing = true;
                    currRect = new Rectangle();
                    currRect.X = startPoint.X;
                    currRect.Y = startPoint.Y;
                }
               
            }
            if (e.Button == MouseButtons.Right)
            {
                historyColor = currentPen.Color;
                drawing = true;
                oldLocation = e.Location;
                currentPath = new GraphicsPath();
                currentPen.Color = System.Drawing.Color.White;
            }
        }
        
        private void PictureBoxChild_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = e.X.ToString() + ", " + e.Y.ToString();
            if (drawing)
            {
                //Drawing Line
                if (StylePen == "Line")
                {
                    Graphics g = Graphics.FromImage(PictureBoxChild.Image);
                    currentPath.AddLine(oldLocation, e.Location);
                    g.DrawPath(currentPen, currentPath);
                    oldLocation = e.Location;
                    
                    g.Dispose();
                }
                //Drawing square
                else if (StylePen == "Square")
                {
                    endPoint = new Point(e.X, e.Y);
                    currRect.X = Math.Min(startPoint.X, endPoint.X);
                    currRect.Y = Math.Min(startPoint.Y, endPoint.Y);
                    int maxLength = Math.Max(Math.Abs(startPoint.X - endPoint.X), Math.Abs(startPoint.Y - endPoint.Y));
                    currRect.Width = maxLength;
                    currRect.Height = maxLength;
                    Graphics g = Graphics.FromImage(PictureBoxChild.Image);
                    currentPen.Color = CheckColor();
                    g.DrawRectangle(new Pen(Brushes.Red), currRect.X, currRect.Y, currRect.Width, currRect.Height);
                }
                
                PictureBoxChild.Invalidate();

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UndoBind();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            RenoBind();
        }

        private void UndoBind()
        {
            //undo
            if (History.Count != 0 && historyCounter != 0)
            {
                PictureBoxChild.Image = new Bitmap(History[--historyCounter]);
            }
            else MessageBox.Show("История пуста", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void RenoBind()
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Z))
            {
                UndoBind();
            }
            else if(keyData == (Keys.Control | Keys.Shift | Keys.Z))
            {
                RenoBind();
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                SaveImage();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            currentPen.Width = trackBar1.Value;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            designshow(panel7);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            StylePen = "Square";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            StylePen = "Line";
            currentPen.DashStyle = DashStyle.Solid;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            StylePen = "Line";
            currentPen.DashStyle = DashStyle.Dash;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            StylePen = "Line";
            currentPen.DashStyle = DashStyle.Dot;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            StylePen = "Line";
            currentPen.DashStyle = DashStyle.DashDotDot;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            OpenPanelChild(new Form4());
            trackBar1.Visible = false;
            label1.Visible = false;
        }

        private void PictureBoxChild_Click(object sender, EventArgs e)
        {

        }
    }
}
