namespace Core.Entities
{
    public class ReferenceColorMatch
    {
        public ReferenceColorMatch(ReferenceColor referenceColor, double confidence)
        {
            ReferenceColor = referenceColor;
            Confidence = confidence;
        }

        public ReferenceColor ReferenceColor { get; }

        public double Confidence { get; }
    }
}