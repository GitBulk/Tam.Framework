using Microsoft.Extensions.Configuration;
using System.IO;

namespace Tam.NetCore.Util
{
    public static class ConfigurationManager
    {
        static public IConfigurationRoot AppSettings { get; set; }

        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            AppSettings = builder.Build();

            //Console.WriteLine($"option1 = {Configuration["option1"]}");
            //Console.WriteLine($"option2 = {Configuration["option2"]}");
            //Console.WriteLine(
            //    $"option1 = {Configuration["subsection:suboption1"]}");
        }
    }
}
