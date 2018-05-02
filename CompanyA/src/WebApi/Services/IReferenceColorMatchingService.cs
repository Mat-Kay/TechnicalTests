namespace WebApi.Services
{
    using System;
    using System.Threading.Tasks;

    using Core.Entities;

    public interface IReferenceColorMatchingService
    {
        Task<ReferenceColorMatcherResult> MatchReferenceColorFromImageUri(Uri imageUri);
    }
}