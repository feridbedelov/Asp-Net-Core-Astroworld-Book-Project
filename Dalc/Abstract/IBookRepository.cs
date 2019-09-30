using Core.Abstract;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dalc.Abstract
{
    public interface IBookRepository:IRepositoryBase<Book>
    {
        IQueryable<Book> GetByCategoryId(int categoryId);
        IQueryable<Book> GetByUserId(string userId);
        IQueryable<Book> GetLatestBooks(int count);
        IQueryable<Book> GetRandomBook();
    }
}
