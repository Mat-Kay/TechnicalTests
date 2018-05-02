namespace Core.Services.ImageAnalysis.ReferenceColorMatchingStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Entities;

    public class MeanEuclideanDistanceReferenceColorMatchingStrategy : IReferenceColorMatchingStrategy
    {
        private readonly IMeanColorCalculator _meanColorCalculator;

        public MeanEuclideanDistanceReferenceColorMatchingStrategy(IMeanColorCalculator meanColorCalculator)
        {
            _meanColorCalculator = meanColorCalculator ?? throw new ArgumentNullException(nameof(meanColorCalculator));
        }

        public List<ReferenceColorMatch> Match(Bitmap image, IEnumerable<ReferenceColor> referenceColors)
        {
            var meanColor = _meanColorCalculator.GetMeanColor(image);

            return referenceColors
                .Select(r => MatchColor(r, meanColor))
                .ToList();
        }

        private ReferenceColorMatch MatchColor(ReferenceColor r, Color meanColor)
        {
            var distance = GetEuclideanDistanceBetweenColors(r.Color, meanColor);

            var confidence = CalculateConfidence(distance, 255);

            return new ReferenceColorMatch(r, confidence);
        }

        private int GetEuclideanDistanceBetweenColors(Color a, Color b)
        {
            int Square(int i) => i * i;

            var redComponent = a.R - b.R;
            var greenComponent = a.G - b.G;
            var blueComponent = a.B - b.B;

            return (int)Math.Sqrt(Square(redComponent) + Square(greenComponent) + Square(blueComponent));
        }

        private double CalculateConfidence(double value, double upperLimit = 1)
        {
            var normalizedValue = value / upperLimit;

            return Math.Pow((Math.Cos(normalizedValue * Math.PI) + 1) / 2, 2);
        }
    }
}