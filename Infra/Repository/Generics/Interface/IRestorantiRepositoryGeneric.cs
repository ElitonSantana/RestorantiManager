using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repository.Generics.Interface
{
    public interface IRestorantiRepositoryGeneric<T> where T : class
    {
        Task AddAsync(T Entity);
        Task Update(T Entity);
        Task Delete(T Entity);
        Task<T> GetById(int Id);
        Task<List<T>> GetList();
    }
}
