namespace Core.Infrastructure.Repositories
{
    using System.Collections.Generic;

    using Entities;

    public interface IReferenceColorRepository
    {
        List<ReferenceColor> GetAll();
    }
}
