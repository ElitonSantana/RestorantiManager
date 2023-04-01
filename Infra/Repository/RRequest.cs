using Entities.Entities;
using Infra.Context;
using Infra.Repository.Generics.Interface;
using Infra.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class RRequest : RestorantiManagerRepository<Request>, IRRequest
    {
        private readonly DbContextOptions<RestorantiManagerContext> _optionsBuilder;
        public RRequest()
        {
            this._optionsBuilder = new DbContextOptions<RestorantiManagerContext>();
        }
    }
}
