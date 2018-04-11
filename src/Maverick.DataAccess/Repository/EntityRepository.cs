using Maverick.DataAccess.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Maverick.DataAccess.Repository
{
    class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        private DbContext context;
        internal DbSet<TEntity> dbSet;
        private bool _disposed;

        public EntityRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> All => dbSet.AsQueryable();

        public void Delete(TEntity entity) => context.Remove(entity);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                context.Dispose();
            }

            _disposed = true;
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            foreach (var include in includes)
                query = query.Include(include);

            return query;
        }

        public TEntity Get(object id) => dbSet.Find(id);

        public Task<TEntity> GetAsync(object id) => dbSet.FindAsync(id);

        public void Insert(TEntity entity) => dbSet.Add(entity);

        public Task InsertAsync(TEntity entity) => dbSet.AddAsync(entity);

        public IQueryable<TEntity> SqlQuery(string sql, params object[] parameters) => dbSet.FromSql(sql, parameters);

        public void Update(TEntity entity) => dbSet.Update(entity);
    }
}
