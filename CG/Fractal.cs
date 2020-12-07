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
        private Color color = Color.Blue;


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
            const int maxiter = 10;
            const double cX = 0;
            const double cY = 0;
            int i;
            int red = color.R;
            int green = color.G;
            int blue = color.B;
            size = pictureBox1.Size;
            var bitmap = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                x_ = (hx-SizeArea/2)+x*(SizeArea/size.Width);
                for (int y = 0; y < h; y++)
                {
                    y_ = (hy-SizeArea/2)+y*(SizeArea/size.Height);
                    Complex c = new Complex(cX, cY);
                    Complex z = new Complex(x_, y_);
                    i = maxiter;
                    while (Complex.Abs(z) < 4 && i > 1)
                    {
                        z = Complex.Sinh(z * z);
                        z += c;
                        i -= 1;
                    }
                    bitmap.SetPixel(x, y, Color.FromArgb(255, (i * red) % 255, (i * green) % 255, (i * blue) % 255));
                }
            }
            pictureBox1.Image = bitmap;
        }

        public void DrowchX()
        {
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            const int maxiter = 10;
            const double cX = 0;
            const double cY = 0;
            int red = color.R;
            int green = color.G;
            int blue = color.B;
            int i;
            size = pictureBox1.Size;
            var bitmap = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                x_ = (hx-SizeArea/2)+x*(SizeArea/size.Width);
                for (int y = 0; y < h; y++)
                {
                    y_ = (hy-SizeArea/2)+y*(SizeArea/size.Height);
                    Complex c = new Complex(cX, cY);
                    Complex z = new Complex(x_, y_);
                    i = maxiter;
                    while (Complex.Abs(z) < 4 && i > 1)
                    {
                        z = Complex.Cosh(z * z);
                        z += c;
                        i -= 1;
                    }
                    bitmap.SetPixel(x, y, Color.FromArgb(255, (i * red) % 255, (i * green) % 255, (i * blue) % 255));
                }
            }
            pictureBox1.Image = bitmap;
        }

        public void DrowSinxCosX()
        {
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            const int maxiter = 10;
            const double cX = 0;
            const double cY = 0;
            int red = color.R;
            int green = color.G;
            int blue = color.B;
            int i;
            size = pictureBox1.Size;
            var bitmap = new Bitmap(w, h);
            for (int x = 0; x < size.Width; x++)
            {
                x_ = (hx-SizeArea/2)+x*(SizeArea/size.Width);
                for (int y = 0; y < size.Height; y++)
                {
                    y_ = (hy-SizeArea/2)+y*(SizeArea/size.Height);
                    Complex c = new Complex(cX, cY);
                    Complex z = new Complex(x_, y_);
                    i = maxiter;
                    while (Complex.Abs(z) < 4 && i > 1)
                    {
                        z = Complex.Cos(z) * Complex.Sin(z);
                        z += c;
                        i -= 1;
                    }
                    bitmap.SetPixel(x, y, Color.FromArgb(255, (i * red) % 255, (i * green) % 255, (i * blue) % 255));
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
                    Drow();
                    break;
                case MouseButtons.Right:
                    x_ = (hx - SizeArea / 2) + x * (SizeArea / size.Width);
                    y_ = (hy - SizeArea / 2) + y * (SizeArea / size.Width);
                    SizeArea *= ScaleArea;
                    Drow();
                    break;
                default:
                    break;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog()==DialogResult.OK)
            {
                color=colorDialog1.Color;
            }
        }
    }
}