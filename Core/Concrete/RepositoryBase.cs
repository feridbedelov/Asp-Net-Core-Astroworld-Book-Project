using Core.Abstract;
using SqlDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext applicationContext;

        public RepositoryBase(ApplicationContext ApplicationContext)
        {
            this.applicationContext = ApplicationContext;
        }


        public void Add(TEntity entity)
        {
            applicationContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            applicationContext.Set<TEntity>().AddRange(entities);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return applicationContext.Set<TEntity>().Where(predicate);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return applicationContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> GetAll()
        {
            return applicationContext.Set<TEntity>().AsQueryable();
        }

        public void Remove(TEntity entity)
        {
            applicationContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            applicationContext.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            applicationContext.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            applicationContext.Set<TEntity>().UpdateRange(entities);
        }
    }
}
