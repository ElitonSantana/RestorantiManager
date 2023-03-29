using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repository.Generics.Interface
{
    public interface IRUserInternal : IRestorantiRepositoryGeneric<UserInternal>
    {
        Task<bool> CreateAsync(UserInternal user);
        Task<UserInternal> GetByUsername(UserInternal user);
        Task<UserInternal> GetUserAdm();
    }
}
