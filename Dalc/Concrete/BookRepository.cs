using Core.Concrete;
using Dalc.Abstract;
using Entities;
using SqlDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dalc.Concrete
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        
        public BookRepository(ApplicationContext applicationContext) :base(applicationContext)
        {
            
        }

        public IQueryable<Book> GetByCategoryId(int categoryId)
        {
            return this.Find(x => x.Category.Id == categoryId);
        }

        public IQueryable<Book> GetByUserId(string userId)
        {
           
            return this.Find(x => x.User.Id == userId);
        }

        public IQueryable<Book> GetLatestBooks(int count)
        {
            return this.Find(x => x.Draft == false).OrderByDescending(y => y.CreatedAt).Take(count);
        }

        public IQueryable<Book> GetRandomBook()
        {
            return this.applicationContext.Books.Where(x => x.Draft == false)
                .OrderBy(y => Guid.NewGuid());

                
        }

    }
}
