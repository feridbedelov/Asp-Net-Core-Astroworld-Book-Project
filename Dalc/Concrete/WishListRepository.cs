using Core.Concrete;
using Dalc.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using SqlDatabase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dalc.Concrete
{
    public class WishListRepository : RepositoryBase<WishList>, IWishListRepository
    {
        public WishListRepository(ApplicationContext ApplicationContext) : base(ApplicationContext)
        {

        }
        public List<WishList> GetWishLists(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return this.applicationContext.Users.Include(x => x.WishLists).SingleOrDefault(predicate).WishLists;
        }
        public IQueryable<WishList> GetWishLists(Expression<Func<WishList, bool>> predicate)
        {
            return this.applicationContext.WishLists.Where(predicate);
        }
    }
}
