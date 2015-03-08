using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Tam.Util
{
    public class PlusCaptcha : MathCaptcha
    {
        //public const string JpegType = "image/jpeg";

        //public const string GifType = "image/gif";

        public PlusCaptcha()
            : base()
        {
        }

        public PlusCaptcha(int widthCaptchaImage, int heightCaptchaImage)
            : base(widthCaptchaImage, heightCaptchaImage)
        {
        }

        public PlusCaptcha(HidingMathCaptchaType hidingType, int firstNumber, int secondNumber)
            : base()
        {
            if (this.FirstNumber < 0 || this.FirstNumber > 90)
            {
                throw new Exception("First number must be between 0 and 10.");
            }
            if (this.SecondNumber < 0 || this.SecondNumber > 90)
            {
                throw new Exception("Second number must be between 0 and 10.");
            }

            this.FirstNumber = firstNumber;
            this.SecondNumber = secondNumber;
            this.HidingType = hidingType;
        }

        public override int GetResult()
        {
            int total = this.FirstNumber + this.SecondNumber;
            if (HidingType == HidingMathCaptchaType.FirstNumber)
            {
                return total - this.SecondNumber;
            }
            else if (HidingType == HidingMathCaptchaType.SecondNumber)
            {
                return total - this.FirstNumber;
            }
            else
            {
                return total;
            }
        }

        public HidingMathCaptchaType HidingType { get; set; }

        public override string Print()
        {
            string firstString = this.FirstNumber.ToString();
            if (HidingType == HidingMathCaptchaType.FirstNumber)
            {
                firstString = "?";
            }
            string secondString = this.SecondNumber.ToString();
            if (HidingType == HidingMathCaptchaType.SecondNumber)
            {
                secondString = "?";
            }
            string total = (this.FirstNumber + this.SecondNumber).ToString();
            if (HidingType == HidingMathCaptchaType.Result)
            {
                total = "?";
            }
            return string.Format("{0} + {1} = {2}", firstString, secondString, total);
        }

        private static byte[] CreateErrorBitmap()
        {
            using (var errorBmp = new Bitmap(200, 70))
            {
                using (Graphics gr = Graphics.FromImage(errorBmp))
                {
                    gr.DrawLine(Pens.Red, 0, 0, 200, 70);
                    gr.DrawLine(Pens.Red, 0, 70, 200, 0);
                }
                using (var memoryStream = new MemoryStream())
                {
                    errorBmp.Save(memoryStream, ImageFormat.Gif);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}