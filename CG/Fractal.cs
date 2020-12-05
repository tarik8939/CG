using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

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
        }
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;
        
        protected override void OnPaint(PaintEventArgs e) {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            
            // Color myColor = Color.FromArgb(50,0,160,255);
            Color myColor = System.Drawing.Color.FromArgb(50,160,255);
            SolidBrush myBrush = new SolidBrush(myColor); 
            
            e.Graphics.FillRectangle(myBrush, rc);

        }
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x84) {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption) {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip) {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }
        private void label1_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            double zoom = 1, moveX = 0, moveY = 0;
            double newRe, newIm, oldRe, oldIm;
            double h = pictureBox1.Height;
            double w = pictureBox1.Width;
            double cRe, cIm;
            cRe = -0.70176;
            cIm = -0.3842;
            // cRe = 0.27334;
            // cIm = 0.00742;
            Bitmap bmp = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            for (int x = 0; x < pictureBox1.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    newRe = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX;
                    newIm = (y - h / 2) / (0.5 * zoom * h) + moveY;
                    // newRe = (double) (x - (pictureBox1.Width / 2)) / (double) (pictureBox1.Width / 4); 
                    // newIm= (double) (y - (pictureBox1.Height / 2)) / (double) (pictureBox1.Height / 4);
                    int it = 0;
                    do
                    {
                        it++;
                        //Запоминаем значение предыдущей итерации
                        oldRe = newRe;
                        oldIm = newIm;
 
                        // в текущей итерации вычисляются действительная и мнимая части 
                        newRe = oldRe * oldRe - oldIm * oldIm + cRe;
                        newIm = 2 * oldRe * oldIm + cIm;
                        if ( (newRe * newRe + newIm * newIm)  > 4) 
                        {
                            break;
                        }

                    } while (it < 100 );
                    bmp.SetPixel(x,y,Color.FromArgb(255, (it * 235) % 255, (it * 94) % 255, (it * 54) % 255));
                }

                pictureBox1.Image = bmp;
            }
        }
        private void Fractal_Load(object sender, EventArgs e)
        {
            
        }
    }

    
    public class Numbers
    {
        public double a;
        public double b;

        public Numbers(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public void Sqr()
        {
            double tmp = (a * a) - (b * b);
            b = 2.0d * a * b;
            a = tmp;
        }

        public double Magn()
        {
            return Math.Sqrt((a * a) + (b * b));
        }

        public void Add(Numbers c)
        {
            a += c.a;
            b += c.b;
        }
        

    }
}