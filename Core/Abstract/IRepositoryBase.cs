using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstract
{
    public interface IRepositoryBase<TEntity> where TEntity: class
    {

        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);





    }
}
