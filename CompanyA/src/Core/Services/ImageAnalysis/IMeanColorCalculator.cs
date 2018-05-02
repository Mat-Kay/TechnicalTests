namespace Core.Services.ImageAnalysis
{
    using System.Drawing;

    public interface IMeanColorCalculator
    {
        Color GetMeanColor(Bitmap bmp);
    }
}