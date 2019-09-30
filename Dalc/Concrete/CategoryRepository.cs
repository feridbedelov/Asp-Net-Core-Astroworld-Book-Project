using Core.Concrete;
using Dalc.Abstract;
using Entities;
using SqlDatabase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalc.Concrete
{
    public class CategoryRepository:RepositoryBase<Category> ,ICategoryRepository
    {
        public CategoryRepository(ApplicationContext ApplicationContext) : base(ApplicationContext)
        {

        }
    }
}
