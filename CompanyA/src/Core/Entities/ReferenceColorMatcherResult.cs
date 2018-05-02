namespace Core.Entities
{
    using System.Collections.Generic;

    public class ReferenceColorMatcherResult
    {
        public ReferenceColorMatchCharacterisation MatchCharacterisation { get; set; }

        public ReferenceColorMatch MatchedColor { get; set; }

        public List<ReferenceColorMatch> AllMatches { get; set; }
    }
}