using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Blog.Model.EnumCollection;
using Tam.Blog.ViewModel;

namespace Tam.Blog.Business.Interface
{
    public interface IUserBusiness : IBaseBusiness<User>
    {
        //User GetUserById(int id);

        User GetUserByEmail(string email);

        Task<User> GetUserByUserNameAsync(string userName);

        User GetUserByUserName(string userName);

        Task<bool> RegisterAsync(RegisterViewModel model);

        Task<bool> ChangePasswordAsync(string userName, string newPassword);

        Task<LoginResult> LoginAsync(LoginViewModel model);
    }
}
