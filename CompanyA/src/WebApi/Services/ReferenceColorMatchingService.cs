namespace WebApi.Services
{
    using System;
    using System.Drawing;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Core.Entities;
    using Core.Infrastructure.Repositories;
    using Core.Services.ImageAnalysis;

    public class ReferenceColorMatchingService : IReferenceColorMatchingService
    {
        private readonly IReferenceColorMatcher _referenceColorMatcher;
        private readonly IReferenceColorRepository _referenceColorRepository;

        public ReferenceColorMatchingService(
            IReferenceColorMatcher referenceColorMatcher,
            IReferenceColorRepository referenceColorRepository)
        {
            _referenceColorMatcher = referenceColorMatcher;
            _referenceColorRepository = referenceColorRepository;
        }

        public async Task<ReferenceColorMatcherResult> MatchReferenceColorFromImageUri(Uri imageUri)
        {
            using (var image = await DownloadImageAsync(imageUri))
            {
                var referenceColors = _referenceColorRepository.GetAll();

                return _referenceColorMatcher.Match(image, referenceColors);
            }
        }

        private async Task<Bitmap> DownloadImageAsync(Uri requestUri)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (var contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync())
            {
                return new Bitmap(contentStream);
            }
        }
    }
}