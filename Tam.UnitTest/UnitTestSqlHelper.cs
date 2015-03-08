using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tam.Util;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Tam.Database;

namespace Tam.UnitTest
{
    [TestClass]
    public class UnitTestSqlHelper
    {
        const string ConnectionString = @"data source=TOAN-PC\SQLEXPRESS;initial catalog=Test;user id=sa;password=123456;MultipleActiveResultSets=True;";

        [TestMethod]
        public void DapperOnlyReturnOneItem()
        {
            var sqlHelper = new SqlServerHelper(ConnectionString);
            string storedName = "usp_Blog_GetUserByName";
            User user = sqlHelper.GetItem<User>(storedName, new { UserName = "toan" });
            Assert.AreEqual("toan", user.UserName);
        }

        [TestMethod]
        public async Task DapperAsyncOnlyReturnOneItem()
        {
            var sqlHelper = new SqlServerHelper(ConnectionString);
            string storedName = "usp_Blog_GetUserByName";
            var parameter = new DynamicParameters();
            parameter.Add("@UserName", "toan");
            //User user = sqlHelper.Get<User>(storedName, new { UserName = "toan" });
            User user = await sqlHelper.GetItemAsync<User>(storedName, parameter);
            Assert.AreEqual("toan@gmail.com", user.Email);
        }

        [TestMethod]
        public async Task DapperAsyncReturnManyItems()
        {
            var sqlHelper = new SqlServerHelper(ConnectionString);
            string storedName = "usp_Blog_GetActiveUsers";
            DateTime? onday = null;
            var query = await sqlHelper.GetManyItemsAsync<User>(storedName, new { OnDay = onday });
            List<User> users = query.ToList();
            Assert.IsTrue(users != null && users[0].UserName == "toan");
        }


        [TestMethod]
        public void Do()
        {
            int a, b;
            a = 2;
            b = 5;
            Assert.AreEqual(7, a + b);

        }
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public System.Guid PasswordSalt { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public byte[] DataRowVersion { get; set; }
        public int Status { get; set; }
        public string UserToken { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
    }
}
