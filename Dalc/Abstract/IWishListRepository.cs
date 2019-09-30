using Core.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dalc.Abstract
{
    public interface IWishListRepository:IRepositoryBase<WishList>
    {
        List<WishList> GetWishLists(Expression<Func<ApplicationUser, bool>> predicate);
        IQueryable<WishList> GetWishLists(Expression<Func<WishList, bool>> predicate);
    }
}
