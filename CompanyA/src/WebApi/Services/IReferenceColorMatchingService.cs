namespace WebApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Core.Entities;

    public interface IReferenceColorMatchingService
    {
        Task<List<ReferenceColorMatch>> ReferenceColorMatches(Uri imageUri);
    }
}