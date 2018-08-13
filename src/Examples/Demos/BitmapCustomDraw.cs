﻿/*
* MIT License
*
* Copyright (c) 2009-2018 Jingwood, unvell.com. All right reserved.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System;
using System.Drawing;
using unvell.D2DLib.WinForm;

namespace unvell.D2DLib.Examples.Demos
{
	public partial class BitmapCustomDraw : D2DForm
	{
		public BitmapCustomDraw()
		{
			InitializeComponent();

			// create two dummy GDI bitmaps and convert them to Direct2D device bitmap
			
			// to create transparent bitmap, specify the pixel format to 32bppPArgb
			gdiBmp1 = new Bitmap(1024, 1024, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			using (Graphics g = Graphics.FromImage(gdiBmp1))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
				g.DrawString("This is GDI+ bitmap layer 1", new Font(this.Font.FontFamily, 48), Brushes.Black, 10, 10);
			}
			d2dbmp1 = this.Device.CreateBitmapFromGDIBitmap(gdiBmp1);


			// to create transparent bitmap, specify the pixel format to 32bppPArgb
			gdiBmp2 = new Bitmap(1024, 1024, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			using (Graphics g = Graphics.FromImage(gdiBmp2))
			{
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				using (var p = new Pen(Color.Blue, 3))
				{
					g.DrawRectangle(p, 200, 200, 400, 200);
				}

				using (var p = new Pen(Color.Red, 5))
				{
					g.DrawEllipse(p, 350, 250, 400, 350);
				}

				g.DrawString("This is GDI+ bitmap layer 2", new Font(this.Font.FontFamily, 24), Brushes.Green, 350, 400);
			}
			d2dbmp2 = this.Device.CreateBitmapFromGDIBitmap(gdiBmp2);


			// create one Direct2D device bitmap
			bmpGraphics = this.Device.CreateBitmapGraphics(1024, 1024);
			bmpGraphics.BeginRender();
			bmpGraphics.FillRectangle(170, 790, 670, 80, new D2DColor(0.4f, D2DColor.Black));
			bmpGraphics.DrawText("This is Direct2D device bitmap", D2DColor.Goldenrod, new Font(this.Font.FontFamily, 36), 180, 800);
			bmpGraphics.EndRender();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (d2dbmp1 == null) Close();
		}

		private Bitmap gdiBmp1, gdiBmp2;
		private D2DBitmap d2dbmp1, d2dbmp2;
		private D2DBitmapGraphics bmpGraphics;
		private static readonly Random rand = new Random();

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			// draw some random rectangles using hardware acceleration
			for (int i = 0; i < 100; i++)
			{
				g.FillRectangle(rand.Next(ClientRectangle.Width), rand.Next(ClientRectangle.Height),
					rand.Next(400) + 50, rand.Next(200) + 50,
					new D2DColor((float)rand.NextDouble() * 0.5f + 0.5f, (float)rand.NextDouble() * 0.5f + 0.5f, 
					(float)rand.NextDouble() * 0.5f + 0.5f, (float)rand.NextDouble() * 0.5f + 0.5f));
			}

			// draw GDI+ bitmaps using hardware acceleration
			g.DrawBitmap(d2dbmp1, this.ClientRectangle);
			g.DrawBitmap(d2dbmp2, this.ClientRectangle);

			// draw Direct2D bitmap
			g.DrawBitmap(bmpGraphics, this.ClientRectangle);
		}
	}
}