using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;
using Tam.Blog.Model;
using Tam.Blog.Model.EnumCollection;
using Tam.Blog.Repository.Interface;
using Tam.Blog.Resource;
using Tam.Blog.ViewModel;
using Tam.Database;
using Tam.Repository.EntityFramework;
using Tam.Util;

namespace Tam.Blog.Repository.Implementation
{
    public class UserRepository : EFBaseRepository<User>, IUserRepository
    {
        //ISqlServerHelper sqlHelper;

        public UserRepository(GreatBlogEntities context, ISqlServerHelper sqlHelper, IUnitOfWork unitOfWork)
            : base(context, false, sqlHelper, unitOfWork)
        {
            //this.sqlHelper = sqlHelper;
        }

        //public User GetUserByEmail(string email)
        //{
        //    return this.context.Users.SingleOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        //}

        //public User GetUserByUserName(string userName)
        //{
        //    return this.context.Users.SingleOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        //}

        //public void Register(ViewModels.RegisterViewModel model)
        //{
        //    // Is username existing ?
        //    var userByName = this.GetUserByUserName(model.UserName);
        //    if (userByName != null)
        //    {
        //        throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
        //    }

        //    // Is email existing ?
        //    var userByEmail = this.GetUserByEmail(model.Email);
        //    if (userByEmail != null)
        //    {
        //        throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);
        //    }

        //    // add user to database
        //    Guid guidSalt = Guid.NewGuid();
        //    string salt = guidSalt.ToString();
        //    string password = CrytorEngine.HashPassword(model.Password, salt);
        //    var userToAdd = new User()
        //    {
        //        Password = password,
        //        CreatedDate = DateTime.UtcNow,
        //        UpdatedDate = DateTime.UtcNow,
        //        PasswordSalt = guidSalt,
        //        UserName = model.UserName,
        //        Email = model.Email,
        //        Status = Convert.ToInt32(UserStatus.InActive),
        //    };
        //    this.Insert(userToAdd);
        //    this.context.SaveChanges();
        //}

        //public void ChangePassword(string userName, string newPassword)
        //{
        //    var user = GetUserByUserName(userName);
        //    if (user == null)
        //    {
        //        throw new Exception("Không tìm thấy thành viên " + userName);
        //    }

        //    user.Password = CrytorEngine.HashPassword(newPassword, user.PasswordSalt.ToString());
        //    user.UpdatedDate = DateTime.UtcNow;

        //    this.Update(user);
        //    this.context.SaveChanges();
        //}

        //public void ForgotPassword(string email)
        //{
        //    var user = GetUserByEmail(email);
        //    if (user == null)
        //    {
        //        throw new Exception("Không tìm thấy email.");
        //    }
        //    // we wiil use md5 for forgot password
        //    string userToken = CrytorEngine.Hash(Guid.NewGuid().ToString(), HashType.MD5);
        //    user.UpdatedDate = DateTime.UtcNow;
        //    user.Status = Convert.ToInt32(UserStatus.InActive);
        //    user.UserToken = userToken;
        //    this.Update(user);
        //    this.context.SaveChanges();

        //    //SendMail()
        //}

        //public void ActiveUser(string userName, string userToken)
        //{
        //    //var user = this.context.Users.SingleOrDefault(u => u.UserToken == userToken && u.UserName == userName && u.Status == Convert.ToInt32(UserStatus.InActive));
        //    //if (user == null)
        //    //{
        //    //    throw new Exception("Không tìm thấy tên thành viên.");
        //    //}
        //    var user = this.context.Users.SingleOrDefault(u => u.UserName == userName && u.Status == Convert.ToInt32(UserStatus.InActive));
        //    if (user == null)
        //    {
        //        throw new Exception("Không tìm thấy tên thành viên.");
        //    }
        //    if (string.Equals(user.UserToken, userToken, StringComparison.OrdinalIgnoreCase) == false)
        //    {
        //        throw new Exception("Chuỗi xác nhận không hợp lệ.");
        //    }
        //    user.Status = Convert.ToInt32(UserStatus.Active);
        //    user.UpdatedDate = DateTime.UtcNow;
        //    user.UserToken = "doi mat khau";
        //    this.Update(user);
        //    this.context.SaveChanges();
        //}

        //public User GetUserByEmailAndUsername(string email, string username)
        //{
        //    return this.context.Users.SingleOrDefault(u => u.UserName == username && u.Email == email);
        //}

        //public IList<User> GetBannedUsers()
        //{
        //    return this.context.Users.Where(u => u.Status == Convert.ToInt32(UserStatus.Banned)).ToList();
        //}

        //public void BannUser(string userName)
        //{
        //    var user = this.GetUserByUserName(userName);
        //    if (user != null)
        //    {
        //        user.Status = Convert.ToInt32(UserStatus.Banned);
        //        this.context.SaveChanges();
        //    }
        //}

        //public LoginResult Login(string userName, string password)
        //{
        //    //if (userName == "admin" && password == "admin")
        //    //{
        //    //    return LoginResult.Success;
        //    //}
        //    User user = this.GetUserByUserName(userName);
        //    if (user == null)
        //    {
        //        return LoginResult.NotFound;
        //    }
        //    if (user.Status == Convert.ToInt32(UserStatus.Banned))
        //    {
        //        return LoginResult.Banned;
        //    }
        //    string hashPassWord = CrytorEngine.HashPassword(password, user.PasswordSalt.ToString());
        //    // compare input password and database password.
        //    if (String.Equals(hashPassWord, user.Password, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return LoginResult.Success;
        //    }
        //    return LoginResult.Failed;
        //}

        ///// <summary>
        ///// Get user by CreatedDate (View list registered user on day)
        ///// </summary>
        ///// <param name="onDay">Id onDay is null --> onDay = DateTime.UtcNow</param>
        ///// <returns></returns>
        //public async Task<IList<User>> GetUsersOnDay(DateTime? onDay = null)
        //{
        //    List<User> users = null;
        //    if (onDay == null)
        //    {
        //        onDay = DateTime.UtcNow;
        //    }
        //    users = await this.context.Users.Where(u => SqlFunctions.DateDiff("d", onDay, u.CreatedDate) == 0).ToListAsync();
        //    return users;
        //}

        //private IQueryable<User> GetUsers()
        //{
        //    return this.context.Users;
        //}

        ///// <summary>
        ///// Get list active user
        ///// </summary>
        ///// <param name="onDay">If onDay is null --> get all active user. Else filter by created date</param>
        ///// <returns></returns>
        //public async Task<IList<User>> GetActiveUsers(Nullable<DateTime> onDay = null)
        //{
        //    var users = GetUsers().Where(u => u.Status == Convert.ToInt32(UserStatus.Active));
        //    if (onDay != null)
        //    {
        //        users = users.Where(u => SqlFunctions.DateDiff("d", onDay, u.CreatedDate) == 0);
        //    }
        //    return await users.ToListAsync();
        //}

        ///// <summary>
        ///// Get list in-active user
        ///// </summary>
        ///// <param name="onDay">If onDay is null --> get all in-active user. Else filter by created date</param>
        ///// <returns></returns>
        //public async Task<IList<User>> GetInActiveUsers(Nullable<DateTime> onDay = null)
        //{
        //    var users = GetUsers().Where(u => u.Status == Convert.ToInt32(UserStatus.InActive));
        //    if (onDay != null)
        //    {
        //        users = users.Where(u => SqlFunctions.DateDiff("d", onDay, u.CreatedDate) == 0);
        //    }
        //    return await users.ToListAsync();
        //}

        //public async Task<IList<User>> GetUsersByStatus(UserStatus status = UserStatus.Active, DateTime? onDay = null)
        //{
        //    switch (status)
        //    {
        //        case UserStatus.InActive:
        //            return await GetInActiveUsers(onDay);
        //        case UserStatus.Active:
        //            return await GetActiveUsers(onDay);
        //        default: // banned
        //            return await GetBannedUsers(onDay);
        //    }
        //}

        //public async Task<IList<User>> GetBannedUsers(DateTime? onDay = null)
        //{
        //    var users = GetUsers().Where(u => u.Status == Convert.ToInt32(UserStatus.Banned));
        //    if (onDay != null)
        //    {
        //        users = users.Where(u => SqlFunctions.DateDiff("d", onDay, u.CreatedDate) == 0);
        //    }
        //    return await users.ToListAsync();
        //}

        public async Task<bool> ActiveUserAsync(string userName, string userToken)
        {
            var user = await this.GetItemAsync(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                && u.UserToken.Equals(userToken, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                //string message = "ActiveUser: " + TextResource.UserNotFound + " " + userName;
                string message = string.Format(TextResource.MethodError, "ActiveUser", TextResource.UserNotFound, userName);
                Log.Error(message);
                throw new Exception(message);
            }
            bool result = await SetUsetStatus(user, UserStatus.Active);
            return result;
        }

        public async Task<bool> BannUserAsync(string userName)
        {
            var user = await GetUserByUserNameAsync(userName);
            if (user == null)
            {
                string message = string.Format(TextResource.MethodError, "BannUser", TextResource.UserNotFound, userName);
                Log.Error(message);
                throw new Exception(message);
            }
            bool result = await SetUsetStatus(user, UserStatus.Banned);
            return result;
        }

        public async Task<bool> ChangePasswordAsync(string userName, string newPassword)
        {
            var user = await GetUserByUserNameAsync(userName);
            if (user == null)
            {
                string message = string.Format(TextResource.MethodError, "ChangePassword", TextResource.UserNotFound, userName);
                Log.Error(message);
                throw new Exception(message);
            }
            user.Password = CryptorEngine.Hash(newPassword, user.PasswordSalt.ToString());
            user.UpdatedDate = DateTime.UtcNow;
            this.Update(user);
            int result = await this.unitOfWork.SaveChangesAsync();
            return (result > 0);
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            //GetUserByEmail
            var user = await GetUserByEmailAsync(email);
            if (user == null)
            {
                string message = string.Format(TextResource.MethodError, "ForgotPassword", TextResource.EmailNotFound, email);
                Log.Error(message);
                throw new Exception(email);
            }
            string userToken = Guid.NewGuid().ToString().Replace("-", "");
            user.UpdatedDate = DateTime.UtcNow;
            user.Status = Convert.ToInt32(UserStatus.InActive);
            user.UserToken = userToken;
            this.Update(user);
            int result = await this.unitOfWork.SaveChangesAsync();
            return (result > 0);
        }

        public async Task<IList<User>> GetActiveUsersAsync(DateTime? onDay = null)
        {
            string storedName = "usp_Blog_GetActiveUsers";
            var users = await sqlHelper.GetManyItemsAsync<User>(storedName, new { OnDay = onDay });
            return users.ToList();
        }

        public IList<User> GetBannedUsers(DateTime? onDay = null)
        {
            string storedName = "usp_Blog_GetBannedUsers";
            var users = sqlHelper.GetManyItems<User>(storedName, new { OnDay = onDay });
            return users.ToList();
        }

        public async Task<IList<User>> GetBannedUsersAsync(DateTime? onDay = null)
        {
            string storedName = "usp_Blog_GetBannedUsers";
            var users = await sqlHelper.GetManyItemsAsync<User>(storedName, new { OnDay = onDay });
            return users.ToList();
        }

        public async Task<IList<User>> GetInActiveUsersAsync(DateTime? onDay = null)
        {
            string storedName = "usp_Blog_GetInActiveUsers";
            var users = await sqlHelper.GetManyItemsAsync<User>(storedName, new { OnDay = onDay });
            return users.ToList();
        }

        public User GetUserByEmail(string email)
        {
            string storedName = "usp_Blog_GetUserByEmail";
            User user = sqlHelper.GetItem<User>(storedName, new { Email = email });
            return user;
        }

        public User GetUserByEmailAndUsername(string email, string userName)
        {
            string storedName = "usp_Blog_GetUserByEmailAndUsername";
            User user = sqlHelper.GetItem<User>(storedName, new { UserName = userName, Email = email });
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            string storedName = "usp_Blog_GetUserByEmail";
            User user = await sqlHelper.GetItemAsync<User>(storedName, new { Email = email });
            return user;
        }

        public User GetUserByUserName(string userName)
        {
            //ISqlServerHelper sqlHelper = new SqlServerHelper(this.GetSqlConnectionString());
            string storedName = "usp_Blog_GetUserByName";
            User user = sqlHelper.GetItem<User>(storedName, new { UserName = userName });
            return user;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            string storedName = "usp_Blog_GetUserByName";
            //User user = sqlHelper.Get<User>(storedName, new { UserName = userName });
            //var parameter = new DynamicParameters();
            //parameter.Add("@UserName", "toan");
            User user = await sqlHelper.GetItemAsync<User>(storedName, new { UserName = userName });
            return user;
        }

        public Task<IList<User>> GetUsersOnDayAsync(DateTime? onDay = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsync(ViewModel.RegisterViewModel model)
        {
            // Is username existing ?
            var userByName = await this.GetUserByUserNameAsync(model.UserName);
            if (userByName != null)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
            }

            // Is email existing ?
            var userByEmail = this.GetUserByEmail(model.Email);
            if (userByEmail != null)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);
            }

            // add user to database
            Guid guidSalt = Guid.NewGuid();
            string salt = guidSalt.ToString();
            string password = CryptorEngine.Hash(model.Password, salt); // md5
            var userToAdd = new User()
            {
                Password = password,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                PasswordSalt = guidSalt,
                UserName = model.UserName,
                Email = model.Email,
                Status = Convert.ToInt32(UserStatus.InActive),
            };
            this.Add(userToAdd);
            int result = this.unitOfWork.SaveChanges();
            return (result > 0);
        }

        private async Task<bool> SetUsetStatus(User user, UserStatus status)
        {
            user.Status = (int)status;
            user.UpdatedDate = DateTime.UtcNow;
            this.Update(user);
            int result = await this.unitOfWork.SaveChangesAsync();
            return (result > 0);
        }

        public async Task<LoginResult> LoginAsync(LoginViewModel model)
        {
#if DEBUG
            if (model.UserName == "admin" && model.Password == "admin")
            {
                return LoginResult.Success;
            }
#endif
            User user = await this.GetUserByUserNameAsync(model.UserName);
            if (user == null)
            {
                return LoginResult.NotFound;
            }
            else if (user.Status == Convert.ToInt32(UserStatus.Banned))
            {
                return LoginResult.Banned;
            }
            string hashPassWord = CryptorEngine.Hash(model.Password, user.PasswordSalt.ToString());
            // compare input password and database password.
            if (String.Equals(hashPassWord, user.Password, StringComparison.OrdinalIgnoreCase))
            {
                return LoginResult.Success;
            }
            return LoginResult.Failed;
        }
    }
}