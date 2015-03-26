using Tam.Blog.Business.Interface;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;
using Tam.Repository.EntityFramework;

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
