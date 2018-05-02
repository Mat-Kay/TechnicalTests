namespace Core.Services.ImageAnalysis
{
    using System.Collections.Generic;
    using System.Drawing;

    using Entities;

    public interface IReferenceColorMatcher
    {
        ReferenceColorMatcherResult Match(Bitmap image, IEnumerable<ReferenceColor> referenceColors);
    }
}