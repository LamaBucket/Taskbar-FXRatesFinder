using System.Collections.Generic;
using System.Drawing;

namespace GTN_WPF.Model
{
    public static class SystemExtensions
    {
        public static List<T> Append<T>(this List<T> list, T obj)
        {
            list.Add(obj);
            return list;
        }

        public static Icon GenerateIcon(this string content, Font? font = null, Color? textColor = null)
        {
            using (Bitmap bitmap = new Bitmap(32, 32))
            {
                bitmap.MakeTransparent();

                Font _font = font ?? new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                Color _textColor = textColor ?? SystemConstants.DefaultColor;

                Brush textBrush = new SolidBrush(_textColor);

                StringFormat format = new StringFormat();

                PointF point = new PointF(16, 16);

                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawString(content, _font, textBrush, point, format);

                    graphics.Flush();
                }

                font?.Dispose();
                _font.Dispose();

                textBrush.Dispose();

                format.Dispose();

                return Icon.FromHandle(bitmap.GetHicon());
            }
        }
    }
}
