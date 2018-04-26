namespace Core.ImageAnalysis
{
    using System.Drawing;

    public interface IMeanColorCalculator
    {
        Color GetMeanColor(Bitmap bmp);
    }
}