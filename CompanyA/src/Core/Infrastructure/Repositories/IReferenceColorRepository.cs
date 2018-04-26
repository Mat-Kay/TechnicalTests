using System.Collections.Generic;

namespace Core.Infrastructure.Repositories
{
    using Entities;

    public interface IReferenceColorRepository
    {
        List<ReferenceColor> GetAll();
    }
}
