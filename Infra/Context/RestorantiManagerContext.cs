using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Context
{
    public class RestorantiManagerContext : DbContext
    {
        public RestorantiManagerContext(DbContextOptions<RestorantiManagerContext> options) : base(options)
        {
        }

        #region DbSet's

        public DbSet<Table>? Tables { get; set; }
        public DbSet<Request>? Requests { get; set; }
        public DbSet<UserInternal>? UserInternals { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(GetConnectionString(), new MySqlServerVersion(new Version()));
            }
        }

        private string GetConnectionString()
        {
            //return "Server=50.116.87.218;Port=3306;Database=restor96_restoranti;Uid=restor96_admin;Pwd=TCC@unip2022";
            return "Server=localhost;Port=3306;Database=meubancov3;Uid=root;Pwd=admin";
        }

    }
}
