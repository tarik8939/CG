﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG
{
    public partial class Form1 : Form
    {
        public Form1() {
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

            Color myColor = Color.FromArgb(50,0,160,255);
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
    

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Form Country = new Form();
            // Country.ShowDialog();
            Fractal form = new Fractal();
            form.Show();

        }

        private void label3_Click(object sender, EventArgs e)
        {
            ColorModels form = new ColorModels();
            form.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            AffineTrans form = new AffineTrans();
            form.Show();
        }
    }
}