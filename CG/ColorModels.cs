using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace CG
{
    public partial class ColorModels : Form
    {
        public ColorModels()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private int lx = 0, ly = 0;
        private int rx =0, ry =0;
        
        
        private const int cGrip = 16; // Grip size
        private const int cCaption = 32; // Caption bar height;
        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr) 2; // HTCAPTION
                    return;
                }

                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr) 17; // HTBOTTOMRIGHT
                    return;
                }
            }

            base.WndProc(ref m);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);

            // Color myColor = Color.FromArgb(50,0,160,255);
            Color myColor = System.Drawing.Color.FromArgb(50, 160, 255);
            SolidBrush myBrush = new SolidBrush(myColor);

            e.Graphics.FillRectangle(myBrush, rc);

        }

        private void label1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BTN_open_Click(object sender, EventArgs e)
        {
            Bitmap image;
            openFileDialog1.Filter = "All jpg files(*.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                pictureBox1.Image = (Image) new Bitmap(openFileDialog1.FileName);
            }

            var pic = (Bitmap)pictureBox1.Image;
            richTextBox2.Text += $"Top left-{0},{0}\nTop right-{pic.Width},{0}\nBottom left-{0},{pic.Height}\n" +
                                 $"Bottom right{pic.Width},{pic.Height}";



        }
        private void button1_Click(object sender, EventArgs e)
        {
            Change();
        }

        public void Change()
        {
            richTextBox1.Clear();
            var size = pictureBox1.Size;
            var pic = (Bitmap)pictureBox1.Image;
            double h, l, s;
            var bitmap = new Bitmap(pic.Width, pic.Height);
            
            for (int i = 0; i < pic.Width; i++)
            {
                for (int j = 0; j < pic.Height; j++)
                {
                     var r = pic.GetPixel(i, j).R;
                     var g = pic.GetPixel(i, j).G;
                     var b = pic.GetPixel(i, j).B;
                     RgbToHsl(r, g, b, out h, out l, out s);
                     HslToRgb( h, l,  s, ref r,ref g,ref b);
                     bitmap.SetPixel(i, j, Color.FromArgb(255, (int)r, (int)g, (int)b));
                }
            }

            pictureBox1.Image = bitmap;
            CMYKis();
            HSLis();
        }
        public void ChangeDistrict()
        {
            richTextBox1.Clear();
            var size = pictureBox1.Size;
            var pic = (Bitmap)pictureBox1.Image;
            double h, l, s;
            var bitmap = new Bitmap(rx-lx, ry-ly);
            for (int i = lx; i < rx ; i++)
            {
                for (int j = ly; j < ry ; j++)
                {
                    var r = pic.GetPixel(i, j).R;
                    var g = pic.GetPixel(i, j).G;
                    var b = pic.GetPixel(i, j).B;
                    RgbToHsl(r, g, b, out h, out l, out s);
                    HslToRgb(h, l, s, ref r, ref g, ref b);
                    pic.SetPixel(i, j, Color.FromArgb(255, (int)r, (int)g, (int)b));
                }
            }

            pictureBox1.Image = pic;
            CMYKis();
            HSLis();
        }
        
        public void RgbToHsl(byte r, byte g, byte b,  out double h,  out double l,  out double s)
        {
            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = r / 255.0;
            double double_g = g / 255.0;
            double double_b = b / 255.0;

            // Get the maximum and minimum RGB components.
            double max = double_r;
            if (max < double_g) max = double_g;
            if (max < double_b) max = double_b;

            double min = double_r;
            if (min > double_g) min = double_g;
            if (min > double_b) min = double_b;

            double diff = max - min;
            l = (max + min) / 2;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5) s = diff / (max + min);
                else s = diff / (2 - max - min);

                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;

                if (double_r == max) h = b_dist - g_dist;
                else if (double_g == max) h = 2 + r_dist - b_dist;
                else h = 4 + g_dist - r_dist;

                h = h * 60;
                if (h < 0) h += 360;
            }
        }
        public void HslToRgb(double h, double l, double s, ref byte r, ref byte g, ref byte b)
        {
            double lielyL = (hScrollBar1.Value);
            lielyL /= 100;
            double p2;
            if (lielyL <= 0.5) p2 = lielyL * (1 + s);
            else p2 = lielyL + s - lielyL * s;

            double p1 = 2 * lielyL - p2;
            double double_r, double_g, double_b;
            if (s == 0)
            {
                double_r = lielyL;
                double_g = lielyL;
                double_b = lielyL;
            }
            else
            {
                double_r = QqhToRgb(p1, p2, h + 120);
                double_g = QqhToRgb(p1, p2, h);
                double_b = QqhToRgb(p1, p2, h - 120);
            }

            // Convert RGB to the 0 to 255 range.
            r = (byte)(double_r * 255.0);
            g = (byte)(double_g * 255.0);
            b = (byte)(double_b * 255.0);
        }
        private double QqhToRgb(double q1, double q2, double hue)
        {
            if (hue > 360) hue -= 360;
            else if (hue < 0) hue += 360;

            if (hue < 60) return q1 + (q2 - q1) * hue / 60;
            if (hue < 180) return q2;
            if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
            return q1;
        }



        private void CMYKis()
        {
            var pic = (Bitmap)pictureBox1.Image;
            var r = pic.GetPixel(lx, ly).R;
            var g = pic.GetPixel(lx, ly).G;
            var b = pic.GetPixel(lx, ly).B;
            double black = Math.Min(1.0 - r / 255.0,Math.Min(1.0-g/255.0,1.0- b /255.0));
            double cyan = (1.0 - (r / 255.0) - black) / (1.0 - black);
            double magenta = (1.0 - (g / 255.0) - black) / (1.0 - black);
            double yellow = (1.0 - (b / 255.0) - black) / (1.0 - black);
            richTextBox1.Text += $"CMYK:\n C: {Math.Round(cyan*100,0)}%\nM: {Math.Round(magenta*100,0)}%\n" +
                                 $"Y: {Math.Round(yellow*100,0)}%\nK: {Math.Round(black*100,0)}%\n";
        }
        private void HSLis()
        {
            var pic = (Bitmap)pictureBox1.Image;
            var r = pic.GetPixel(lx, ly).R;
            var g = pic.GetPixel(lx, ly).G;
            var b = pic.GetPixel(lx, ly).B;
            double h = 0;
            double s = 0;
            double l = 0;
            RgbToHsl(r,g,b,out h,out l,out s);
            richTextBox1.Text += $"HSL:\n H: {(int)h}%\nS: {(int)s*100}%\n" +
                                 $"L: {Math.Round(l*100,0)}%\n";
        }
        
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = hScrollBar1.Value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            hScrollBar1.Value = Convert.ToInt32(textBox1.Text);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            rx = e.X;
            ry = e.Y;
            ChangeDistrict();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lx = e.X;
            ly = e.Y;
        }
    }
}