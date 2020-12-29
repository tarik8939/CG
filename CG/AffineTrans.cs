using System;
using System.Drawing;
using System.Windows.Forms;

namespace CG
{
    public partial class AffineTrans : Form
    {
        //Пикселей в одном делении оси
        const int PIX_IN_ONE = 15;
        //Длина стрелки
        const int ARR_LEN = 10;
        public AffineTrans()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            pictureBox1.Paint += pictureBox1_Paint_1;

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

        private PaintEventArgs test;
        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
    {
        int w = pictureBox1.ClientSize.Width / 2;
        int h = pictureBox1.ClientSize.Height / 2;
        //Смещение начала координат в центр PictureBox
        e.Graphics.TranslateTransform(w, h);
        DrawXAxis(new Point(-w, 0), new Point(w, 0), e.Graphics);
        DrawYAxis(new Point(0, h), new Point(0, -h), e.Graphics);
        //Центр координат
        e.Graphics.FillEllipse(Brushes.Red, -2, -2, 4, 4);
        test = e;
    }
 
    //Рисование оси X
    private void DrawXAxis(Point start, Point end, Graphics g)
    {
        //Деления в положительном направлении оси
        for (int i = PIX_IN_ONE; i < end.X - ARR_LEN; i += PIX_IN_ONE)
        {
            g.DrawLine(Pens.Black, i, -5, i, 5);
            DrawText(new Point(i, 5), (i / PIX_IN_ONE).ToString(), g);
        }
        //Деления в отрицательном направлении оси
        for (int i = -PIX_IN_ONE; i > start.X; i -= PIX_IN_ONE)
        {
            g.DrawLine(Pens.Black, i, -5, i, 5);
            DrawText(new Point(i, 5), (i / PIX_IN_ONE).ToString(), g);
        }
        //Ось
        g.DrawLine(Pens.Black, start, end);
        //Стрелка
        g.DrawLines(Pens.Black, GetArrow(start.X, start.Y, end.X, end.Y, ARR_LEN));
    }
 
    //Рисование оси Y
    private void DrawYAxis(Point start, Point end, Graphics g)
    {
        //Деления в отрицательном направлении оси
        for (int i = PIX_IN_ONE; i < start.Y; i += PIX_IN_ONE)
        {
            g.DrawLine(Pens.Black, -5, i, 5, i);
            DrawText(new Point(5, i), (-i / PIX_IN_ONE).ToString(), g, true);
        }
        //Деления в положительном направлении оси
        for (int i = -PIX_IN_ONE; i > end.Y + ARR_LEN; i -= PIX_IN_ONE)
        {
            g.DrawLine(Pens.Black, -5, i, 5, i);
            DrawText(new Point(5, i), (-i / PIX_IN_ONE).ToString(), g, true);
        }
        //Ось
        g.DrawLine(Pens.Black, start, end);
        //Стрелка
        g.DrawLines(Pens.Black, GetArrow(start.X, start.Y, end.X, end.Y, ARR_LEN));
        
        // g.DrawLine(Pens.Black, -5, -2, 5, 2);
    }
 
    //Рисование текста
    private void DrawText(Point point, string text, Graphics g, bool isYAxis = false)
    {
        var f = new Font(Font.FontFamily, 6);
        var size = g.MeasureString(text, f);
        var pt = isYAxis
            ? new PointF(point.X + 1, point.Y - size.Height / 2)
            : new PointF(point.X - size.Width / 2, point.Y + 1);
        var rect = new RectangleF(pt, size);
        g.DrawString(text, f, Brushes.Black, rect);
    }
 
    //Вычисление стрелки оси
    private static PointF[] GetArrow(float x1, float y1, float x2, float y2, float len = 10, float width = 4)
    {
        PointF[] result = new PointF[3];
        //направляющий вектор отрезка
        var n = new PointF(x2 - x1, y2 - y1);
        //Длина отрезка
        var l = (float)Math.Sqrt(n.X * n.X + n.Y * n.Y);
        //Единичный вектор
        var v1 = new PointF(n.X / l, n.Y / l);
        //Длина стрелки
        n.X = x2 - v1.X * len;
        n.Y = y2 - v1.Y * len;
        result[0] = new PointF(n.X + v1.Y * width, n.Y - v1.X * width);
        result[1] = new PointF(x2, y2);
        result[2] = new PointF(n.X - v1.Y * width, n.Y + v1.X * width);
        return result;
    }

    public void Draw(Graphics g)
    {
        
    }


    private void button1_Click(object sender, EventArgs e)
    {
        
    }
    
    
    
    
    }
}