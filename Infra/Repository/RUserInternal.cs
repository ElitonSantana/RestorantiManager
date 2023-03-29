using Entities.Entities;
using Infra.Context;
using Infra.Repository.Generics;
using Infra.Repository.Generics.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repository
{
    public class RUserInternal : RestorantiManagerRepository<UserInternal>, IRUserInternal
    {
        private readonly DbContextOptions<RestorantiManagerContext> _optionsBuilder;
        public RUserInternal()
        {
            this._optionsBuilder = new DbContextOptions<RestorantiManagerContext>();
        }

        public async Task<bool> CreateAsync(UserInternal user)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {
                //await data.Set<User>().AddAsync(user);
                AddModelBase(user);

                var result = data.AddAsync(user);
                await data.SaveChangesAsync();

                if (result.IsCompleted && result.IsCompletedSuccessfully)
                    return true;
                else
                    return false;
            }
        }

        public async Task<UserInternal> GetByUsername(UserInternal user)
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {

                var userExistent = data.Set<UserInternal>().Where(x => x.Username == user.Username).FirstOrDefault();

                if (userExistent != null)
                    return userExistent;
                else
                    return null;
            }
        }

        public async Task<UserInternal> GetUserAdm()
        {
            using (var data = new RestorantiManagerContext(_optionsBuilder))
            {

                var userExistent = data.Set<UserInternal>().Where(x => x.Username == "admin").FirstOrDefault();

                if (userExistent != null)
                    return userExistent;
                else
                    return null;
            }
        }
        public void AddModelBase(UserInternal user)
        {
            user.CreationDate = DateTime.Now;
        }
    }
}
