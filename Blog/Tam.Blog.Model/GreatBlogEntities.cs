using System.Data.Entity;

namespace Tam.Blog.Model
{
    public partial class GreatBlogEntities : DbContext
    {
        /// <summary>
        /// Constructor GreatBlogEntities
        /// </summary>
        /// <param name="connectionString">Entity Framework connection string</param>
        public GreatBlogEntities(string connectionString)
            : base(connectionString)
        {
        }
    }

}
