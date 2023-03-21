using Entities.Entities;
using Infra.Context;
using Infra.Repository.Generics;
using Infra.Repository.Generics.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository
{
    public class RTable : RestorantiManagerRepository<Table>, IRTable
    {
        private readonly DbContextOptions<RestorantiManagerContext> _optionsBuilder;
        public RTable()
        {
            this._optionsBuilder = new DbContextOptions<RestorantiManagerContext>();
        }
    }
}
