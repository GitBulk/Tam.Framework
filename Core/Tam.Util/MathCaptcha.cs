using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Util
{
    public abstract class MathCaptcha
    {
        /// <summary>
        /// image/jpeg
        /// </summary>
        public const string JpegType = "image/jpeg";

        /// <summary>
        /// image/gif
        /// </summary>
        public const string GifType = "image/gif";

        private const float V = 4F;

        public int FirstNumber { get; set; }

        public int SecondNumber { get; set; }

        public int WidthCaptchaImage { get; set; }

        public int HeightCaptchaImage { get; set; }

        public MathCaptcha()
            : this(174, 40)
        {
        }

        public MathCaptcha(int widthCaptchaImage, int heightCaptchaImage)
        {
            this.WidthCaptchaImage = widthCaptchaImage;
            this.HeightCaptchaImage = heightCaptchaImage;
        }

        public abstract int GetResult();

        public abstract string Print();

        private IList<FontFamily> Fonts = new List<FontFamily>
                        {
                            new FontFamily("Times New Roman"),
                            new FontFamily("Arial")
                        };

        public virtual Bitmap GenerateBitmap(string text)
        {
            var random = new Random();
            //Randomly choose the font name.
            FontFamily familyName = this.Fonts[random.Next(this.Fonts.Count)];
            // Create a new 32-bit bitmap image.
            int Width = this.WidthCaptchaImage;
            int Height = this.HeightCaptchaImage;
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, Width, Height);

                // Fill in the background.
                using (var brush = new HatchBrush(HatchStyle.SmallConfetti,
                    Color.LightGray, Color.Blue))
                {
                    g.FillRectangle(brush, rect);

                    // Set up the text format.
                    var format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip,
                        Trimming = StringTrimming.None
                    };

                    format.SetMeasurableCharacterRanges(new[] { new CharacterRange(0, text.Length) });

                    // Set up the text font.
                    RectangleF size;
                    float fontSize = rect.Height + 1;
                    Font font = null;
                    // Adjust the font size until the text fits within the image.
                    do
                    {
                        if (font != null)
                        {
                            font.Dispose();
                        }
                        fontSize--;
                        font = new Font(familyName, fontSize, FontStyle.Bold);
                        size = g.MeasureCharacterRanges(text, font, rect, format)[0].GetBounds(g);
                    } while (size.Width > rect.Width || size.Height > rect.Height);
                    // Check http://stackoverflow.com/questions/2292812/font-in-graphicspath-addstring-is-smaller-than-usual-font on why we have to convert to em
                    // Create a path using the text and warp it randomly.
                    var path = new GraphicsPath();
                    path.AddString(text, font.FontFamily, (int)font.Style, g.DpiY * font.Size / 72, rect, format);
                    PointF[] points =
                    {
                        new PointF(
                            random.Next(rect.Width)/V,
                            random.Next(rect.Height)/V),
                        new PointF(
                            rect.Width - random.Next(rect.Width)/V,
                            random.Next(rect.Height)/V),
                        new PointF(
                            random.Next(rect.Width)/V,
                            rect.Height - random.Next(rect.Height)/V),
                        new PointF(
                            rect.Width - random.Next(rect.Width)/V,
                            rect.Height - random.Next(rect.Height)/V)
                    };
                    var matrix = new Matrix();
                    matrix.Translate(0F, 0F);
                    path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

                    // Draw the text.
                    using (var hatchBrush = new HatchBrush(
                        HatchStyle.LargeConfetti,
                        Color.LightGray,
                        Color.White))
                    {
                        g.FillPath(hatchBrush, path);

                        // Add some random noise.
                        int m = Math.Max(rect.Width, rect.Height);
                        for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
                        {
                            int x = random.Next(rect.Width);
                            int y = random.Next(rect.Height);
                            int w = random.Next(m / 50);
                            int h = random.Next(m / 50);
                            g.FillEllipse(hatchBrush, x, y, w, h);
                        }
                    }
                    // Clean up.
                    font.Dispose();
                }
                // Set the image.
                return bitmap;
            } // end using Graphics;
        }// end Method Generate
    }

}
