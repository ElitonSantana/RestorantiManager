using Infra.Context;
using Infra.Repository.Generics.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Infra.Repository.Generics
{
    public class RestorantiManagerRepository<T> : IRestorantiRepositoryGeneric<T>, IDisposable where T : class
    {
        private readonly DbContextOptions<RestorantiManagerContext> _optionsBuilder;

        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public RestorantiManagerRepository()
        {
            this._optionsBuilder = new DbContextOptions<RestorantiManagerContext>();
        }

        public async Task AddAsync(T entity)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                await data.Set<T>().AddAsync(entity);
                await data.SaveChangesAsync();
            }
        }

        public async Task Delete(T entity)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                data.Set<T>().Remove(entity);
                await data.SaveChangesAsync();
            }
        }

        public async Task<T> GetById(int Id)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                var result = await data.Set<T>().FindAsync(Id);
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }
        

        public async Task<List<T>> GetList()
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                return await data.Set<T>().ToListAsync();
            }
        }

        public async Task Update(T entity)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                data.Set<T>().Update(entity);
                await data.SaveChangesAsync();
            }
        }

        #region Dispose Method

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        #endregion

    }
}
