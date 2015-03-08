using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Blog.Model.EnumCollection;
using Tam.Blog.ViewModel;


namespace Tam.Blog.Repository.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User GetUserByEmail(string email);

        User GetUserByUserName(string userName);

        Task<User> GetUserByUserNameAsync(string userName);

        Task<User> GetUserByEmailAsync(string email);

        Task<bool> RegisterAsync(RegisterViewModel model);

        Task<bool> ChangePasswordAsync(string userName, string newPassword);

        Task<bool> ForgotPasswordAsync(string email);

        Task<bool> ActiveUserAsync(string userName, string userToken);

        User GetUserByEmailAndUsername(string email, string userName);

        IList<User> GetBannedUsers(DateTime? onDay = null);

        Task<bool> BannUserAsync(string userName);

        Task<LoginResult> LoginAsync(LoginViewModel model);

        Task<IList<User>> GetUsersOnDayAsync(Nullable<DateTime> onDay = null);

        Task<IList<User>> GetActiveUsersAsync(Nullable<DateTime> onDay = null);

        Task<IList<User>> GetInActiveUsersAsync(Nullable<DateTime> onDay = null);

        Task<IList<User>> GetBannedUsersAsync(Nullable<DateTime> onDay = null);

        //Task<IList<User>> GetUsersByStatus(UserStatus status = UserStatus.Active, Nullable<DateTime> onDay = null);
    }
}
