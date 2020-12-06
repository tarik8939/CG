using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Numerics;

namespace CG
{
    public partial class Fractal : Form
    {


        public Fractal()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            size = pictureBox1.Size;
        }

        private const int cGrip = 16; // Grip size
        private const int cCaption = 32; // Caption bar height;

        private double hx = 0, hy = 0, x_, y_, n = 0;
        private Size size;
        private double ScaleArea = 4.5;
        private double SizeArea = 4.5;


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

        private void label1_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void DrowshX()
        {
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;

            const int maxiter = 100;
            const double zoom = 0.45;
            const int moveX = 0;
            const int moveY = 0;
            const double cX = 0;
            const double cY = 0;
            double zx, zy, tmp;
            int i;
            int red = readRed();
            int green = readBlue();
            int blue = readBlue();
            var colors = (from c in Enumerable.Range(0, 256)
                select Color.FromArgb((c >> 5) * 36, (c >> 3 & 7) * 36, (c & 3) * 85)).ToArray();
            size = pictureBox1.Size;
            var bitmap = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                x_ = (hx-SizeArea/2)+x*(SizeArea/size.Width);
                for (int y = 0; y < h; y++)
                {
                    y_ = (hy-SizeArea/2)+y*(SizeArea/size.Height);
                    zx = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX;
                    zy = 1.0 * (y - h / 2) / (0.5 * zoom * h) + moveY;
                    Complex c = new Complex(cX, cY);
                    Complex z = new Complex(x_, y_);
                    i = maxiter;
                    while (Complex.Abs(z) < 4 && i > 1)
                    {
                        z = Complex.Sinh(z * z);
                        z += c;
                        i -= 1;
                    }

                    // bitmap.SetPixel(x, y, colors[i]);
                    bitmap.SetPixel(x, y, Color.FromArgb(255, (i * red) % 255, (i * green) % 255, (i * blue) % 255));
                }
            }

            pictureBox1.Image = bitmap;
            //bmp.SetPixel(x,y,Color.FromArgb(255, (it * 15) % 255, (it * 9) % 255, (it * 9) % 255));
        }

        public void DrowchX()
        {
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            const double zoom = 0.5;
            const int maxiter = 100;
            const int moveX = 0;
            const int moveY = 0;
            const double cX = 0;
            const double cY = 0;
            int red = readRed();
            int green = readBlue();
            int blue = readBlue();
            double zx, zy, tmp;
            int i;
            size = pictureBox1.Size;
            var colors = (from c in Enumerable.Range(0, 256)
                select Color.FromArgb((c >> 5) * 36, (c >> 3 & 7) * 36, (c & 3) * 85)).ToArray();

            var bitmap = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                x_ = (hx-SizeArea/2)+x*(SizeArea/size.Width);
                for (int y = 0; y < h; y++)
                {
                    y_ = (hy-SizeArea/2)+y*(SizeArea/size.Height);
                    zx = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX;
                    zy = 1.0 * (y - h / 2) / (0.5 * zoom * h) + moveY;
                    Complex c = new Complex(cX, cY);
                    Complex z = new Complex(x_, y_);
                    i = maxiter;
                    while (Complex.Abs(z) < 4 && i > 1)
                    {
                        z = Complex.Cosh(z * z);
                        z += c;
                        i -= 1;
                    }

                    //bitmap.SetPixel(x, y, colors[i]);
                    bitmap.SetPixel(x, y, Color.FromArgb(255, (i * red) % 255, (i * green) % 255, (i * blue) % 255));
                    
                }
            }

            pictureBox1.Image = bitmap;
            MessageBox.Show(hx.ToString());
        }

        public void DrowSinxCosX()
        {
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            const int maxiter = 100;
            const double zoom = 0.5;
            const int moveX = 0;
            const int moveY = 0;
            const double cX = 0;
            const double cY = 0;
            int red = readRed();
            int green = readBlue();
            int blue = readBlue();
            double zx, zy, tmp;
            int i;
            size = pictureBox1.Size;

            var colors = (from c in Enumerable.Range(0, 256)
                select Color.FromArgb((c >> 5) * 36, (c >> 3 & 7) * 36, (c & 3) * 85)).ToArray();

            var bitmap = new Bitmap(w, h);
            for (int x = 0; x < size.Width; x++)
            {
                x_ = (hx-SizeArea/2)+x*(SizeArea/size.Width);
                for (int y = 0; y < size.Height; y++)
                {
                    y_ = (hy-SizeArea/2)+y*(SizeArea/size.Height);
                    zx = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX;
                    zy = 1.0 * (y - h / 2) / (0.5 * zoom * h) + moveY;
                    Complex c = new Complex(cX, cY);
                    Complex z = new Complex(x_, y_);
                    i = maxiter;
                    while (Complex.Abs(z) < 4 && i > 1)
                    {
                        z = Complex.Cos(z) * Complex.Sin(z);
                        z += c;
                        i -= 1;
                    }
                    //bitmap.SetPixel(x, y, colors[i]);
                    bitmap.SetPixel(x, y, Color.FromArgb(255, (i * red) % 255, (i * green) % 255, (i * blue) % 255));
                    //bitmap.SetPixel(x,y, i < 100 ? Color.FromArgb(i,i,i) : Color.FromArgb(255,255,255));
                }
            }

            pictureBox1.Image = bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                hx=0;
                hx=0;
                SizeArea=4.5;
                DrowshX();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                hx=0;
                hx=0;
                SizeArea=4.5;
                DrowchX();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                hx=0;
                hx=0;
                SizeArea=4.5;
                DrowSinxCosX();
            }
        }

        public void Drow()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                DrowshX();
                
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                DrowchX();
                
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                DrowSinxCosX();
                
            }
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X,
                y = e.Y;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    hx = (hx - SizeArea / 2) + x * (SizeArea / size.Width);
                    hy = (hy - SizeArea / 2) + y * (SizeArea / size.Width);
                    SizeArea /= ScaleArea;
                    //button1_Click(null, null);
                    Drow();
                    break;
                case MouseButtons.Right:
                    x_ = (hx - SizeArea / 2) + x * (SizeArea / size.Width);
                    y_ = (hy - SizeArea / 2) + y * (SizeArea / size.Width);
                    SizeArea *= ScaleArea;
                    //button1_Click(null, null);
                    Drow();
                    break;
                default:
                    break;
            }
        }

        public int readRed()
        {
            return (int) numericUpDown1.Value;
        }
        public int readGreen()
        {
            return (int) numericUpDown2.Value;
        }
        public int readBlue()
        {
            return (int) numericUpDown3.Value;
        }
    }
}