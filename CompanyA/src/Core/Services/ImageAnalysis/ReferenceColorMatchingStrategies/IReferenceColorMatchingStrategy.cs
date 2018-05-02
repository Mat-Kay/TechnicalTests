namespace Core.Services.ImageAnalysis.ReferenceColorMatchingStrategies
{
    using System.Collections.Generic;
    using System.Drawing;

    using Entities;

    public interface IReferenceColorMatchingStrategy
    {
        List<ReferenceColorMatch> Match(Bitmap image, IEnumerable<ReferenceColor> referenceColors);
    }
}