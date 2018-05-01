namespace Core.Services.ImageAnalysis
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class MeanColorCalculator : IMeanColorCalculator
    {
        public Color GetMeanColor(Bitmap bmp)
        {
            var rgbTotals = CalculateRgbTotals(bmp);

            long pixelCount = bmp.Width * bmp.Height;

            var averageRed = Convert.ToInt32(rgbTotals.Red / pixelCount);
            var averageGreen = Convert.ToInt32(rgbTotals.Green / pixelCount);
            var averageBlue = Convert.ToInt32(rgbTotals.Blue / pixelCount);

            return Color.FromArgb(averageRed, averageGreen, averageBlue);
        }

        private (long Red, long Green, long Blue) CalculateRgbTotals(Bitmap bmp)
        {
            var imageBits = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

            var bytesPerPixel = Image.GetPixelFormatSize(imageBits.PixelFormat) / 8;

            var rgbTotals = new long[] { 0, 0, 0 };

            var width = bmp.Width * bytesPerPixel;

            unsafe
            {
                var p = (byte*) (void*) imageBits.Scan0;

                for (var y = 0; y < bmp.Height; y++)
                {
                    for (var x = 0; x < width; x += bytesPerPixel)
                    {
                        rgbTotals[0] += p[x + 0];
                        rgbTotals[1] += p[x + 1];
                        rgbTotals[2] += p[x + 2];
                    }

                    p += imageBits.Stride;
                }
            }

            bmp.UnlockBits(imageBits);

            return (rgbTotals[2], rgbTotals[1], rgbTotals[0]);
        }
    }
}
