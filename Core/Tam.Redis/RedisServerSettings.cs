using System;
using System.Configuration;

namespace Tam.Redis
{
    //public class RedisServerSettings : ConfigurationSection, IRedisServerSettings
    //{
    //    public static Lazy<IRedisServerSettings> Settings = new Lazy<IRedisServerSettings>();

    //    [ConfigurationProperty("RedisPreferSlaveForRead", IsRequired = false, DefaultValue = false)]
    //    public bool PreferSlaveForRead
    //    {
    //        get { return Convert.ToBoolean(this["RedisPreferSlaveForRead"]); }
    //    }

    //    [ConfigurationProperty("RedisConnectionStringName", IsRequired = true)]
    //    public string ConnectionStringOrName
    //    {
    //        get
    //        {
    //            string result = this["RedisConnectionStringName"].ToString();
    //            return result;
    //        }
    //    }

    //    [ConfigurationProperty("RedisDefaultDb", IsRequired = false, DefaultValue = 0)]
    //    public int DefaultDb
    //    {
    //        get { return Convert.ToInt32(this["RedisDefaultDb"]); }
    //    }
    //}

    public class RedisServerSettings : IRedisServerSettings
    {
        //public static Lazy<IRedisServerSettings> Settings = new Lazy<IRedisServerSettings>();

        private static string GetConfig(string key)
        {
            var temp = ConfigurationManager.AppSettings[key];
            if (temp == null)
            {
                return "";
            }
            return temp.ToString();
        }

        public bool PreferSlaveForRead
        {
            get
            {
                string s = GetConfig("RedisPreferSlaveForRead");
                if (string.IsNullOrEmpty(s))
                {
                    return false;
                }
                return Convert.ToBoolean(s);
            }
        }

        public string ConnectionStringOrName
        {
            get
            {
                string result = GetConfig("RedisConnectionStringName");
                return result;
            }
        }

        public int DefaultDb
        {
            get
            {
                string s = GetConfig("RedisDefaultDb");
                if (string.IsNullOrEmpty(s))
                {
                    return 0;
                }
                return Convert.ToInt32(s);
            }
        }
    }
}