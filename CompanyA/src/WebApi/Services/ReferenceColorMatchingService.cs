namespace WebApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Core.Entities;
    using Core.Infrastructure.Repositories;
    using Core.Services.ImageAnalysis;

    public class ReferenceColorMatchingService : IReferenceColorMatchingService
    {
        private readonly IReferenceColorMatchingStrategy _referenceColorMatchingStrategy;
        private readonly IReferenceColorRepository _referenceColorRepository;

        public ReferenceColorMatchingService(
            IReferenceColorMatchingStrategy referenceColorMatchingStrategy,
            IReferenceColorRepository referenceColorRepository)
        {
            _referenceColorMatchingStrategy = referenceColorMatchingStrategy;
            _referenceColorRepository = referenceColorRepository;
        }

        public async Task<List<ReferenceColorMatch>> ReferenceColorMatches(Uri imageUri)
        {
            var image = await DownloadImageAsync(imageUri);

            var referenceColors = _referenceColorRepository.GetAll();

            return _referenceColorMatchingStrategy.Match(image, referenceColors);
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