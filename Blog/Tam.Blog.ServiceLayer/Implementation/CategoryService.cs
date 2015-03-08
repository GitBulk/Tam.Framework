using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;
using Tam.Blog.ServiceLayer.Interface;
using Tam.Util;

namespace Tam.Blog.ServiceLayer.Implementation
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        IPostRepository postRepository;
        ICategoryRepository categoryRepository;
        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
            :base(unitOfWork, categoryRepository, null)
        {
            this.unitOfWork = unitOfWork;
            this.categoryRepository = categoryRepository;
        }
    }
}
