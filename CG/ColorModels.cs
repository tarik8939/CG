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
            var bitmap = new Bitmap(pic.Width, pic.Height);
            
            for (int i = 0; i < pic.Width; i++)
            {
                for (int j = 0; j < pic.Height; j++)
                {
                    var r = (int)pic.GetPixel(i, j).R;
                    var g = (int)pic.GetPixel(i, j).G;
                    var b = (int)pic.GetPixel(i, j).B;
                    double h, l, s;
                    RgbToHsl(r, g, b, out h, out l, out s);
                    if (h >= 50.0 && h <= 70.0)
                    {
                        s = (double)hScrollBar1.Value / 100;
                        
                    }
                    else if (h >= 225.0 && h <= 255.0) {s = (double)hScrollBar1.Value / 100;}
                    pic.SetPixel(i, j, HslToRgb(h, s, l));
                }
            }

            pictureBox1.Image = pic;
            CMYKis();
            HSLis();
        }
        public void ChangeDistrict()
        {
            richTextBox1.Clear();
            var size = pictureBox1.Size;
            var pic = (Bitmap)pictureBox1.Image;
            var bitmap = new Bitmap(pic.Width, pic.Height);
            
            for (int i = lx; i < rx; i++)
            {
                for (int j = ly; j < ry; j++)
                {
                    var r = (int)pic.GetPixel(i, j).R;
                    var g = (int)pic.GetPixel(i, j).G;
                    var b = (int)pic.GetPixel(i, j).B;
                    double h, l, s;
                    RgbToHsl(r, g, b, out h, out l, out s);
                    if (h >= 50.0 && h <= 70.0)
                    {
                        s = (double)hScrollBar1.Value / 100;
                        pic.SetPixel(i, j, HslToRgb(h, s, l));
                        
                    }
                    else if (h >= 225.0 && h <= 255.0)
                    {
                        s = (double)hScrollBar1.Value / 100;
                        pic.SetPixel(i, j, HslToRgb(h, s, l));
                    }
                    
                }
            }

            pictureBox1.Image = pic;
            CMYKis();
            HSLis();
        }
        public void RgbToHsl(int r, int g, int b, out double h, out double l, out double s)
        {

            double _r = (r / 255.0);
            double _g = (g / 255.0);
            double _b = (b / 255.0);

            double min = Math.Min(Math.Min(_r, _g), _b);
            double max = Math.Max(Math.Max(_r, _g), _b);
            double delta = max - min;

            l = (max + min) / 2;

            if (delta == 0)
            {
                h = 0;
                s = 0.0;
            }
            else
            {
                s = (l <= 0.5) ? (delta / (max + min)) : (delta / (2 - max - min));

                double hue;

                if (_r == max)
                {
                    hue = ((_g - _b) / 6) / delta;
                }
                else if (_g == max)
                {
                    hue = (1.0 / 3) + ((_b - _r) / 6) / delta;
                }
                else
                {
                    hue = (2.0 / 3) + ((_r - _g) / 6) / delta;
                }

                if (hue < 0)
                    hue += 1;
                if (hue > 1)
                    hue -= 1;

                h = (hue * 360);
            }
        }

        public  Color HslToRgb(double h, double s, double l)
        {
            double r = 0, g = 0, b = 0;
            if (l != 0)
            {
                if (s == 0)
                    r = g = b = l;
                else
                {
                    double temp2;
                    double hue = h / 360;
                    if (l < 0.5)
                        temp2 = l * (1.0 + s);
                    else
                        temp2 = l + s - (l * s);

                    double temp1 = 2.0 * l - temp2;

                    r = GetColorComponent(temp1, temp2, hue + (1.0 / 3.0));
                    g = GetColorComponent(temp1, temp2, hue);
                    b = GetColorComponent(temp1, temp2, hue - (1.0 / 3.0));
                }
            }

            return Color.FromArgb((int) (255 * r), (int) (255 * g), (int) (255 * b));
        }

        private double GetColorComponent(double temp1, double temp2, double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0; 
            else if (temp3 > 1.0)
                temp3 -= 1.0;

            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
           
            else
                return temp1;
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