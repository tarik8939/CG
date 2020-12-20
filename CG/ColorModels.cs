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

            Change();

        }

        public void Change()
        {
            var size = pictureBox1.Size;
            var pic = (Bitmap)pictureBox1.Image;
            byte r, g, b;
            float c;
            float m;
            float y;
            float k;
            var l = 0.5;
            var list = new List<float>();
            
            
            for (int i = 0; i < size.Width; i++)
            {
                for (int j = 0; j < size.Height; j++)
                {
                    var rez = RGBtoCMYK(i, j);
                     //MessageBox.Show($"{rez[0]}/{rez[1]}/{rez[2]}");
                     //pic.SetPixel(i, j, Color.FromArgb(255, rez[0], rez[1], rez[2]));
                    
                }
            }
        }

        private List<byte> RGBtoCMYK(int x, int y)
        {
            var list = new List<double>();
            var pic = (Bitmap)pictureBox1.Image;
            var r = pic.GetPixel(x, y).R;
            var g = pic.GetPixel(x, y).G;
            var b = pic.GetPixel(x, y).B;
            double black = Math.Min(1.0 - r / 255.0,Math.Min(1.0-g/255.0,1.0- b /255.0));
            double cyan = (1.0 - (r / 255.0) - black) / (1.0 - black);
            double magenta = (1.0 - (g / 255.0) - black) / (1.0 - black);
            double yellow = (1.0 - (b / 255.0) - black) / (1.0 - black);
            list.Add(cyan);
            list.Add(magenta);
            list.Add(yellow);
            return CMYKtoRGB(cyan,magenta,yellow,black);
        }

        private List<byte> CMYKtoRGB(double cyan, double magenta, double yellow, double black)
        {
            var list = new List<byte>();
            var color = Convert.ToDouble(textBox1.Text);
            byte red = Convert.ToByte((1 - Math.Min(1, cyan * (1 - black) + black)) * 255);
            byte green = Convert.ToByte((1 - Math.Min(1, magenta * (1 - black) + black)) * 255);
            // byte blue = Convert.ToByte((1 - Math.Min(1, yellow * (1 - black) + black)) * 255);
            byte blue = Convert.ToByte((1 - Math.Min(1, color * (1 - black) + black)) * 255);
            //byte blue = Convert.ToByte(color);
            list.Add(red);
            list.Add(green);
            list.Add(blue);
            //MessageBox.Show($"{red}/{green}/{blue}");
            return list;
        }
    }
}