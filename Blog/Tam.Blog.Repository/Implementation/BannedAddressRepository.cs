using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tam.Blog.Model;
using Tam.Blog.Repository.Interface;


namespace Tam.Blog.Repository.Implementation
{
    //public class BannedAddressRepository : GenericRepository<BannedIpAddress>, IBannedIpAddress
    //{
    //    public BannedAddressRepository(GreatBlogEntities context)
    //        : base(context)
    //    {

    //    }

    //    public void BannIP(User user)
    //    {
            //// 1. Get list ip of user in table UserLoginHistories
            //var items = this.context.UserLoginHistories.Where(u => u.UserName == user.UserName);

            //// 2. Insert into table BannedIpAddresses
            //foreach (UserLoginHistory item in items)
            //{
            //    var bannedIpObject = new BannedIpAddress()
            //    {
            //        IpAddress = item.IpAdress,
            //        CreatedDate = DateTime.UtcNow
            //    };
            //    this.Add(bannedIpObject);
            //}

            //// 3. Save
            //this.context.SaveChanges();
    //    }
    //}
}
