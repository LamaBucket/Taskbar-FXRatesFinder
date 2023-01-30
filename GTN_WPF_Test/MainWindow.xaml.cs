using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using System.Windows;

namespace GTN_WPF_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string apiKey = "b38469fba4f73b54782a";
        private const string url = "https://free.currconv.com/api/v7/convert";
        private const string baseCurrencyName = "RUB";
        private const string targetCurrencryName = "USD";
        private static string FXRateName = $"{targetCurrencryName}_{baseCurrencyName}";

        public MainWindow()
        {
            InitializeComponent();
        }



        private void PerformRequestAndWriteData()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("apiKey", apiKey);
            queryString.Add("q", FXRateName);
            queryString.Add("compact", "ultra");

            UriBuilder uri = new UriBuilder(url);

            uri.Query = queryString.ToString();

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(uri.Uri);

                JObject jo = JObject.Load(new JsonTextReader(new StringReader(json)));

                decimal value = Convert.ToDecimal(jo.GetValue(FXRateName));

                this.textblock.Text = value.ToString();


                TaskbarIcon.Icon = GetIcon(value.ToString());
            }

        }


        public static Icon GetIcon(string text)
        {
            //Create bitmap, kind of canvas
            Bitmap bitmap = new Bitmap(32, 32);

            Icon icon = new Icon(@"E:\WIN\Development\GTN_WPF\GTN_WPF_Test\Images\hui.ico");
            System.Drawing.Font drawFont = System.Drawing.SystemFonts.DialogFont;
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            graphics.DrawIcon(icon, 0, 0);
            graphics.DrawString(text, drawFont, drawBrush, 1, 1);

            //To Save icon to disk
            bitmap.Save("icon.ico", System.Drawing.Imaging.ImageFormat.Icon);

            Icon createdIcon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());

            drawFont.Dispose();
            drawBrush.Dispose();
            graphics.Dispose();
            bitmap.Dispose();

            return createdIcon;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            PerformRequestAndWriteData();
            //Thread.Sleep(60000);
            //}
        }


    }
}
