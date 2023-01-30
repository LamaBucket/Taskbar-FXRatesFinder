using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace GTN_WPF_Test
{
    internal static class Extensions
    {
        public static Bitmap CopyToSquareCanvas(this Bitmap sourceBitmap, Color canvasBackground)
        {
            int maxSide = sourceBitmap.Width > sourceBitmap.Height ? sourceBitmap.Width : sourceBitmap.Height;


            Bitmap bitmapResult = new Bitmap(maxSide, maxSide, PixelFormat.Format32bppArgb);


            using (Graphics graphicsResult = Graphics.FromImage(bitmapResult))
            {
                graphicsResult.Clear(canvasBackground);


                int xOffset = (sourceBitmap.Width - maxSide) / 2;
                int yOffset = (sourceBitmap.Height - maxSide) / 2;


                graphicsResult.DrawImage(sourceBitmap, new System.Drawing.Point(xOffset, xOffset));
            }


            return bitmapResult;
        }


        public static Icon CreateIcon(this Bitmap sourceBitmap, IconSizeDimensions iconSize)
        {
            Bitmap squareCanvas = sourceBitmap.CopyToSquareCanvas(Color.Transparent);
            squareCanvas = (Bitmap)squareCanvas.GetThumbnailImage((int)iconSize, (int)iconSize, null, new IntPtr());


            Icon iconResult = Icon.FromHandle(squareCanvas.GetHicon());


            return iconResult;
        }

        public enum IconSizeDimensions
        {
            IconSize16x16Pixels = 16,
            IconSize24x24Pixels = 24,
            IconSize32x32Pixels = 32,
            IconSize48x48Pixels = 48,
            IconSize64x64Pixels = 64,
            IconSize96x96Pixels = 96,
            IconSize128x128Pixels = 128
        }
    }
}
