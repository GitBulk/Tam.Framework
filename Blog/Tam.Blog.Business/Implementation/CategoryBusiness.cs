using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Business.Interface;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;
using Tam.Repository.Interface;
using Tam.Util;

namespace Tam.Blog.Business.Implementation
{
    public class CategoryBusiness : BaseBusiness<Category>, ICategoryBusiness
    {
        ICategoryRepository categoryRepository;
        public CategoryBusiness(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
            : base(unitOfWork, categoryRepository, null)
        {
            this.unitOfWork = unitOfWork;
            this.categoryRepository = categoryRepository;
        }
    }
}
