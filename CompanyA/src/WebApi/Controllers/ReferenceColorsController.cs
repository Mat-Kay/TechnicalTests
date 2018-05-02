namespace WebApi.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Core.Entities;

    using Microsoft.AspNetCore.Mvc;

    using Services;

    [Route("api/reference-colors")]
    public class ReferenceColorsController : Controller
    {
        private readonly IReferenceColorMatchingService _referenceColorMatchingService;

        public ReferenceColorsController(IReferenceColorMatchingService referenceColorMatchingService)
        {
            _referenceColorMatchingService = referenceColorMatchingService;
        }

        /// <summary>
        /// Gets the reference color most closely matching the provided image.
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<ReferenceColorMatcherResult> MatchReferenceColorFromImageUri(Uri imageUri)
        {
            return await _referenceColorMatchingService.MatchReferenceColorFromImageUri(imageUri);
        }
    }
}
