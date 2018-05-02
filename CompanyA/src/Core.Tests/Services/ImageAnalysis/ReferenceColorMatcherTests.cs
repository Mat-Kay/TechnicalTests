namespace Core.Tests.Services.ImageAnalysis
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using Core.Services.ImageAnalysis;
    using Core.Services.ImageAnalysis.ReferenceColorMatchingStrategies;

    using Entities;

    using Microsoft.Extensions.Options;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ReferenceColorMatcherTests
    {
        [TestFixture]
        public class MatchCharacterisation
        {
            private ReferenceColorMatcher _service;

            private Mock<IReferenceColorMatchingStrategy> _matchingStrategy;
            private ReferenceColorMatchingSettings _defaultSettings;

            [SetUp]
            public void Setup()
            {
                _defaultSettings = new ReferenceColorMatchingSettings() { MinimumConfidenceThreshold = 0.8, MinimumConfidenceMargin = 0.2 };
                var options = Options.Create(_defaultSettings);

                _matchingStrategy = new Mock<IReferenceColorMatchingStrategy>();

                _service = new ReferenceColorMatcher(_matchingStrategy.Object, options);
            }

            [Test]
            public void GivenASingleMatchIsFoundWithConfidenceEqualToTheThreshold_ThenShouldBeMatchFound()
            {
                // Arrange
                _matchingStrategy
                    .Setup(x => x.Match(It.IsAny<Bitmap>(), It.IsAny<IEnumerable<ReferenceColor>>()))
                    .Returns(new List<ReferenceColorMatch>()
                    {
                        new ReferenceColorMatch(new ReferenceColor("green", Color.Green), _defaultSettings.MinimumConfidenceThreshold)
                    });

                // Act
                var match = _service.Match(new Bitmap(1, 1), Enumerable.Empty<ReferenceColor>());

                // Assert
                Assert.That(match.MatchCharacterisation, Is.EqualTo(ReferenceColorMatchCharacterisation.MatchFound));
            }

            [Test]
            public void GivenASingleMatchIsFoundWithConfidenceGreaterThanTheThreshold_ThenShouldBeMatchFound()
            {
                // Arrange
                _matchingStrategy
                    .Setup(x => x.Match(It.IsAny<Bitmap>(), It.IsAny<IEnumerable<ReferenceColor>>()))
                    .Returns(new List<ReferenceColorMatch>()
                    {
                        new ReferenceColorMatch(new ReferenceColor("green", Color.Green), _defaultSettings.MinimumConfidenceThreshold + 0.1)
                    });

                // Act
                var match = _service.Match(new Bitmap(1, 1), Enumerable.Empty<ReferenceColor>());

                // Assert
                Assert.That(match.MatchCharacterisation, Is.EqualTo(ReferenceColorMatchCharacterisation.MatchFound));
            }

            [Test]
            public void GivenAllMatchedColorsHaveConfidenceBelowTheThreshold_ThenShouldBeNoMatch()
            {
                // Arrange
                _matchingStrategy
                    .Setup(x => x.Match(It.IsAny<Bitmap>(), It.IsAny<IEnumerable<ReferenceColor>>()))
                    .Returns(new List<ReferenceColorMatch>()
                    {
                        new ReferenceColorMatch(new ReferenceColor("green", Color.Green), _defaultSettings.MinimumConfidenceThreshold - 0.1),
                        new ReferenceColorMatch(new ReferenceColor("blue", Color.Blue), _defaultSettings.MinimumConfidenceThreshold - 0.1),
                        new ReferenceColorMatch(new ReferenceColor("red", Color.Red), _defaultSettings.MinimumConfidenceThreshold - 0.1),
                    });

                // Act
                var match = _service.Match(new Bitmap(1, 1), Enumerable.Empty<ReferenceColor>());

                // Assert
                Assert.That(match.MatchCharacterisation, Is.EqualTo(ReferenceColorMatchCharacterisation.NoMatchMeetsConfidenceThreshold));
            }
        }
    }
}
