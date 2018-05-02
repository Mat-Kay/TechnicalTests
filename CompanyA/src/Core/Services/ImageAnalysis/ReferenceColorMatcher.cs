namespace Core.Services.ImageAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Entities;

    using Microsoft.Extensions.Options;

    using ReferenceColorMatchingStrategies;

    public class ReferenceColorMatcher : IReferenceColorMatcher
    {
        private readonly IReferenceColorMatchingStrategy _matchingStrategy;
        private readonly ReferenceColorMatchingSettings _referenceColorMatchingSettings;

        public ReferenceColorMatcher(
            IReferenceColorMatchingStrategy matchingStrategy,
            IOptions<ReferenceColorMatchingSettings> referenceColorMatchingSettings)
        {
            _matchingStrategy = matchingStrategy ?? throw new ArgumentNullException(nameof(matchingStrategy));
            _referenceColorMatchingSettings = referenceColorMatchingSettings.Value ?? throw new ArgumentNullException(nameof(referenceColorMatchingSettings));
        }

        public ReferenceColorMatcherResult Match(Bitmap image, IEnumerable<ReferenceColor> referenceColors)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (referenceColors == null)
            {
                throw new ArgumentNullException(nameof(referenceColors));
            }

            var matches = _matchingStrategy.Match(image, referenceColors)
                .OrderByDescending(r => r.Confidence)
                .ToList();

            var matchCharacterisation = CalculateMatchCharacterisation(matches[0].Confidence, matches.Count > 1 ? matches[1].Confidence : default(double?));

            return new ReferenceColorMatcherResult()
            {
                MatchCharacterisation = matchCharacterisation,
                MatchedColor = matchCharacterisation == ReferenceColorMatchCharacterisation.MatchFound ? matches[0] : null,
                AllMatches = matches,
            };
        }

        public ReferenceColorMatchCharacterisation CalculateMatchCharacterisation(double firstConfidenceValue, double? maybeSecondConfidenceValue)
        {
            if (firstConfidenceValue < _referenceColorMatchingSettings.MinimumConfidenceThreshold)
            {
                return ReferenceColorMatchCharacterisation.NoMatchMeetsConfidenceThreshold;
            }

            if (maybeSecondConfidenceValue.HasValue && firstConfidenceValue - maybeSecondConfidenceValue.Value < _referenceColorMatchingSettings.MinimumConfidenceMargin)
            {
                return ReferenceColorMatchCharacterisation.NoDefinitiveMatch;
            }

            return ReferenceColorMatchCharacterisation.MatchFound;
        }
    }
}