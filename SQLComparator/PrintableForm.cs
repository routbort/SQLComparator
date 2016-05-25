using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SQLComparator
{
    public class PrintableForm : System.Windows.Forms.Form
    {

        private System.Drawing.Printing.PrintDocument pd;
        private Bitmap formImage;
    
        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "BitBlt", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        private static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, System.Int32 dwRop);

        public void Print()
        {
            base.Refresh();
            pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
            pd.DefaultPageSettings.Landscape = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            GetFormImage();
            pd.Print();
        }

        private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int x = e.MarginBounds.X + (int)System.Math.Floor(Convert.ToDecimal(e.MarginBounds.Width - formImage.Width) / 2);
            int y = e.MarginBounds.Y + (int)System.Math.Floor(Convert.ToDecimal(e.MarginBounds.Height - formImage.Height) / 2);
            e.Graphics.DrawImage(formImage, x, y);
            e.HasMorePages = false;
        }

        private void GetFormImage()
        {
            Graphics g = this.CreateGraphics();
            Size s = this.Size;
            formImage = new Bitmap(s.Width, s.Height, g);
            Graphics mg = Graphics.FromImage(formImage);
            IntPtr dc1 = g.GetHdc();
            IntPtr dc2 = mg.GetHdc();
            int widthDiff = (this.Width - this.ClientRectangle.Width);
            int heightDiff = (this.Height - this.ClientRectangle.Height);
            int borderSize = widthDiff / 2;
            int heightTitleBar = heightDiff - borderSize;
            BitBlt(dc2, 0, 0, this.ClientRectangle.Width + widthDiff, this.ClientRectangle.Height + heightDiff, dc1, 0 - borderSize, 0 - heightTitleBar, 13369376);
            g.ReleaseHdc(dc1);
            mg.ReleaseHdc(dc2);
        }
    }

} //end of root namespace