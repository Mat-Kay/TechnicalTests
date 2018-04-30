namespace WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Core.Entities;

    using Microsoft.AspNetCore.Mvc;

    using Services;

    // TODO: Controller/action naming
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IReferenceColorMatchingService _referenceColorMatchingService;

        public ValuesController(IReferenceColorMatchingService referenceColorMatchingService)
        {
            _referenceColorMatchingService = referenceColorMatchingService;
        }

        [HttpGet]
        public async Task<List<ReferenceColorMatch>> Get(Uri imageUri)
        {
            return await _referenceColorMatchingService.ReferenceColorMatches(imageUri);
        }
    }
}
