using Maverick.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maverick.DataAccess.Context
{
    public class IdentityBaseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IEntitiesContext
    {
        private DbTransaction _transaction;

        public IdentityBaseDbContext(DbContextOptions options) : base(options)
        {
            // this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public void BeginTransaction()
        {
            if (this.Database.GetDbConnection().State == ConnectionState.Open)
            {
                return;
            }
            this.Database.GetDbConnection().Open();
            _transaction = this.Database.GetDbConnection().BeginTransaction();
        }

        public int Commit()
        {
            var saveChanges = SaveChanges();
            _transaction.Commit();
            return saveChanges;
        }

        public Task<int> CommitAsync()
        {
            var saveChangesAsync = SaveChangesAsync();
            _transaction.Commit();
            return saveChangesAsync;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseModel, new()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
            => base.Set<TEntity>();

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : class
            => base.Set<TElement>().FromSql(sql, parameters).AsEnumerable();
    }
}
