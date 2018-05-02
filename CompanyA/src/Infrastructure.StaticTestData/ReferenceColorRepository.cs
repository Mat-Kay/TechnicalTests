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
                { new ReferenceColor("black", Color.FromArgb(36, 36, 36)) },
                { new ReferenceColor("grey", Color.FromArgb(68, 80, 96)) },
                { new ReferenceColor("navy", Color.FromArgb(0, 0, 116)) },
                { new ReferenceColor("teal", Color.FromArgb(14, 106, 117)) },
            };
    }
}
