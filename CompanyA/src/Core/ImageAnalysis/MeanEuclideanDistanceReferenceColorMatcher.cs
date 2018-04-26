namespace Core.ImageAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Entities;

    public class MeanEuclideanDistanceReferenceColorMatcher : IReferenceColorMatchingStrategy
    {
        private readonly IMeanColorCalculator _meanColorCalculator;

        public MeanEuclideanDistanceReferenceColorMatcher(IMeanColorCalculator meanColorCalculator)
        {
            _meanColorCalculator = meanColorCalculator ?? throw new ArgumentNullException(nameof(meanColorCalculator));
        }

        public List<ReferenceColorMatch> Match(Bitmap image, IEnumerable<ReferenceColor> referenceColors)
        {
            var meanColor = _meanColorCalculator.GetMeanColor(image);

            // TODO: Some form of confidence calculation
            // TODO: Better return model
            return referenceColors
                .Select(r => new ReferenceColorMatch(r, GetEuclideanDistanceBetweenColors(r.Color, meanColor)))
                .OrderBy(r => r.Confidence)
                .ToList();
        }

        private int GetEuclideanDistanceBetweenColors(Color a, Color b)
        {
            var redComponent = a.R - b.R;
            var greenComponent = a.G - b.G;
            var blueComponent = a.B - b.B;

            return (int)Math.Sqrt(Square(redComponent) + Square(greenComponent) + Square(blueComponent));
        }

        private int Square(int i)
            => i * i;
    }
}