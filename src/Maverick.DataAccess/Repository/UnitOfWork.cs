using Maverick.DataAccess.Context;
using Maverick.DataAccess.Repository.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maverick.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IEntitiesContext _context;
        private bool _disposed;
        private Hashtable _repositories;

        public UnitOfWork(IEntitiesContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _context.BeginTransaction();
        }

        public int Commit()
        {
            return _context.Commit();
        }

        public Task<int> CommitAsync()
        {
            return _context.CommitAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();

                if (_repositories != null && _repositories.Values != null && _repositories.Values.OfType<IDisposable>().Any())
                {
                    foreach (IDisposable repository in _repositories.Values)
                    {
                        repository.Dispose();// dispose all repositries
                    }
                }
            }
            _disposed = true;
            GC.SuppressFinalize(this);

        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(EntityRepository<>);
            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));
            return (IRepository<TEntity>)_repositories[type];
        }

        public void Rollback()
        {
            _context.Rollback();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
