using System.Configuration;
using Tam.Repository.Contraction;

namespace Tam.Blog.Repository.Implementation
{
    public class ConnectionStringRepository : IConnectionString
    {
        public string GetEFConnectionString()
        {
            var temp = ConfigurationManager.ConnectionStrings["GreatBlogEntities"];
            if (temp != null)
            {
                return temp.ToString();
            }
            return "";
        }

        public string GetSqlConnectionString()
        {
            //return ConnectionUtil.GetSqlConnectionString();
            //return ConfigurationManager.ConnectionStrings["TestEntities"].ToString();
            //return @"Data Source=TOAN-PC\SQLEXPRESS;initial catalog=Test1.1;persist security info=True;user id=sa;password=123456;multipleactiveresultsets=True;";

            var temp = ConfigurationManager.ConnectionStrings["GreatBlogSql"];
            if (temp != null)
            {
                return temp.ToString();
            }
            return "";
        }

    }
}
