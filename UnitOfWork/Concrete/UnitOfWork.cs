using System;
using System.Collections.Generic;
using System.Text;
using Dalc.Abstract;
using Dalc.Concrete;
using SqlDatabase;
using UnitOfWork.Abstract;

namespace UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        public IBookRepository BookRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public IWishListRepository WishListRepository { get; }

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;

            BookRepository = new BookRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
            WishListRepository = new WishListRepository(_context);
        }

        

        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
