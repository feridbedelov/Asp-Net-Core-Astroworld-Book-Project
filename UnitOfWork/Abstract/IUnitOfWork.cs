using Dalc.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitOfWork.Abstract
{
    public interface IUnitOfWork:IDisposable
    {
        IBookRepository BookRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        IWishListRepository WishListRepository { get; }

        int Complete();
    }
}
