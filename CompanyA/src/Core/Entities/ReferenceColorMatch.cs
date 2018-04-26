namespace Core.Entities
{
    public class ReferenceColorMatch
    {
        public ReferenceColorMatch(ReferenceColor referenceColor, int confidence)
        {
            ReferenceColor = referenceColor;
            Confidence = confidence;
        }

        public ReferenceColor ReferenceColor { get; }

        public int Confidence { get; }
    }
}