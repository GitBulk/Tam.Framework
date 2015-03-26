using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tam.Util;

namespace Tam.UnitTest
{
    [TestClass]
    public class UnitTestWebHelper
    {
        [TestMethod]
        public void CanGetPublicIP()
        {
            string ip = WebHelper.GetPublicIP();
            bool result = (ip != null && ip != "");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanMakeGoogleShortenUrl()
        {
            string longUrl = "http://www.bipinjoshi.net/articles/09bf16f8-ad91-4638-8fce-f2e286f6fe1d.aspx";
            //string expectedResult = "http://goo.gl/ObseyR";
            string shortUrl = WebHelper.ShortenUrl(longUrl, ShortenUrlType.Google);
            bool result = (shortUrl != "" && shortUrl.StartsWith("http://"));
            Assert.IsTrue(result);
        }
        // Ip: 1.54.149.254

    }
}
