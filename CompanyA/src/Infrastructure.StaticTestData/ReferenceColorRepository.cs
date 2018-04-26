namespace Infrastructure.StaticTestData
{
    using System.Collections.Generic;
    using System.Drawing;

    using Core.Entities;
    using Core.Infrastructure.Repositories;

    public class ReferenceColorRepository : IReferenceColorRepository
    {
        public List<ReferenceColor> GetAll()
            => new List<ReferenceColor>()
            {
                new ReferenceColor("black", Color.Black),
                new ReferenceColor("grey", Color.Gray),
                new ReferenceColor("navy", Color.Navy),
                new ReferenceColor("teal", Color.Teal),
            };
    }
}
