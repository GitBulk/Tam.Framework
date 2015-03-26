using System.Threading.Tasks;
using Tam.Blog.Business.Interface;
using Tam.Blog.Model;
using Tam.Blog.Model.EnumCollection;
using Tam.Blog.Repository.Interface;
using Tam.Blog.ViewModel;
using Tam.Repository.EntityFramework;

namespace Tam.Blog.Business.Implementation
{
    public class UserBusiness : BaseBusiness<User>, IUserBusiness
    {
        //IUnitOfWork unitOfWork;
        IUserRepository userRepository;
        public UserBusiness(IUnitOfWork unitOfWork, IUserRepository userRepository)
            : base(unitOfWork, userRepository)
        {
            //this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
        }

        //public User GetUserById(int id)
        //{
        //    User user = this.userRepository.GetByID(id);
        //    return user;
        //}

        public User GetUserByEmail(string email)
        {
            User user = this.userRepository.GetUserByEmail(email);
            return user;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            //var user = await this.userRepository.GetItemAsync(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            var user = await this.userRepository.GetUserByUserNameAsync(userName);
            return user;
        }

        public User GetUserByUserName(string userName)
        {
            var user = this.userRepository.GetUserByUserName(userName);
            return user;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            //// Is username existing ?
            //var userByName = await this.GetUserByUserNameAsync(model.UserName);
            //if (userByName != null)
            //{
            //    throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
            //}

            //// Is email existing ?
            //var userByEmail = this.GetUserByEmail(model.Email);
            //if (userByEmail != null)
            //{
            //    throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);
            //}

            //// add user to database
            //Guid guidSalt = Guid.NewGuid();
            //string salt = guidSalt.ToString();
            //string password = CryptorEngine.Hash(model.Password, salt); // md5
            //var userToAdd = new User()
            //{
            //    Password = password,
            //    CreatedDate = DateTime.UtcNow,
            //    UpdatedDate = DateTime.UtcNow,
            //    PasswordSalt = guidSalt,
            //    UserName = model.UserName,
            //    Email = model.Email,
            //    Status = Convert.ToInt32(UserStatus.InActive),
            //};
            //this.userRepository.Add(userToAdd);
            //int result = this.unitOfWork.SaveChanges();
            //return (result > 0);

            bool result = await this.userRepository.RegisterAsync(model);
            return result;
        }

        public async Task<bool> ChangePasswordAsync(string userName, string newPassword)
        {
            bool result = await this.userRepository.ChangePasswordAsync(userName, newPassword);
            return result;
        }


        public async Task<LoginResult> LoginAsync(LoginViewModel model)
        {
            var result = await this.userRepository.LoginAsync(model);
            return result;
        }
    }
}
