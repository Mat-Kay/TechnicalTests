namespace TechnicalTests.CompanyA.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Core.Entities;
    using Core.ImageAnalysis;
    using Core.Infrastructure.Repositories;

    using Microsoft.AspNetCore.Mvc;

    // TODO: Name
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IReferenceColorMatchingStrategy _referenceColorMatchingStrategy;
        private readonly IReferenceColorRepository _referenceColorRepository;

        public ValuesController(
            IReferenceColorMatchingStrategy referenceColorMatchingStrategy, 
            IReferenceColorRepository referenceColorRepository)
        {
            _referenceColorMatchingStrategy = referenceColorMatchingStrategy ?? throw new ArgumentNullException(nameof(referenceColorMatchingStrategy));
            _referenceColorRepository = referenceColorRepository ?? throw new ArgumentNullException(nameof(referenceColorRepository));
        }

        [HttpGet]
        public async Task<List<ReferenceColorMatch>> Get(Uri imageUri)
        {
            var image = await DownloadImageAsync(imageUri);

            var referenceColors = _referenceColorRepository.GetAll();

            return _referenceColorMatchingStrategy.Match(image, referenceColors);
        }

        // TODO: Move
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
