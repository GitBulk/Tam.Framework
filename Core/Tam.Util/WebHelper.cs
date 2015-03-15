using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Tam.JsonManager;

namespace Tam.Util
{
    public static class WebHelper
    {
        public static string GetUserIp()
        {
            var request = HttpContext.Current.Request;
            string ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            return ip;
        }

        /// <summary>
        /// Get gravatar image link
        /// </summary>
        /// <param name="email">Email address that you registered at gavatar.com</param>
        /// <param name="size">Size of image in pixel (from 1px to 2048px)</param>
        /// <returns>Image tag</returns>
        public static string GetGravatarImageLink(string email, int size = 80)
        {
            // size from 1px up to 2048p
            email = (email == null ? "" : email.Trim());
            if (size >= 1 && size <= 2048)
            {
                string hashEmail = CryptorEngine.Hash(email.ToLower(), "", HashType.Md5);
                return string.Format("<img src=\"http://www.gravatar.com/avatar/{0}?s={1}\" />", hashEmail, size);
            }
            else
            {
                throw new Exception("Image size from 1px up to 2048px");
            }
        }

        /// <summary>
        /// Get Alexa rank
        /// </summary>
        /// <param name="url">Url to website. Ex: http://dotnetslackers.com</param>
        /// <returns>An Alexa rank number.</returns>
        public static long GetAlexaRank(string url)
        {
            try
            {
                string tempUrl = "http://data.alexa.com/data?cli=10&dat=snbamz&url=" + url.Trim();
                var webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                string myData = webClient.DownloadString(tempUrl);
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(myData);
                var node = xmlDocument.SelectNodes("/ALEXA/SD")[1];
                if (node["POPULARITY"].Attributes["TEXT"] != null)
                {
                    return Convert.ToInt64(node["POPULARITY"].Attributes["TEXT"].Value);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsMobileTablet(string userAgent)
        {
            userAgent = userAgent.ToLower();
            var devices = new List<string>();
            devices.Add("android");
            devices.Add("(iphone|ipod|ipad)");
            devices.Add("blackberry");
            devices.Add("opera mini");
            devices.Add("palm");
            devices.Add("portable");
            devices.Add("(windows phone|windows ce)");
            devices.Add("mobile");
            foreach (var item in devices)
            {
                var match = Regex.Match(userAgent, item);
                if (match.Success)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetPublicIP()
        {
            String result = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                result = stream.ReadToEnd();
            }

            //Search for the ip in the html
            int first = result.IndexOf("Address: ") + 9;
            int last = result.LastIndexOf("</body>");
            result = result.Substring(first, last - first);

            return result;
        }


        public static string GetUserAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        /// <summary>
        /// Replace a Youtube link into videos
        /// </summary>
        /// <param name="url">The url to the video</param>
        /// <param name="width">The width of the player in pixels</param>
        /// <param name="height">The height of the player in pixels</param>
        /// <param name="autoPlay">Auto play video</param>
        /// <param name="loop">Play video again.</param>
        /// <param name="youtubeTheme">Youtube theme: Dark or Light.</param>
        /// <param name="removeYoutubeBranding">Remove Youtube branding.</param>
        /// <returns>An iframe Youtube video.</returns>
        public static string EmbedYoutube(string url, int width = 600, int height = 400, bool autoPlay = false, bool loop = false,
            YoutubeTheme youtubeTheme = YoutubeTheme.Dark, bool removeYoutubeBranding = false)
        {
            try
            {
                string tempUrl = url.Trim();
                if (string.IsNullOrEmpty(tempUrl))
                {
                    throw new Exception("Url is null or empty");
                }

                // get video id  -----------------------------------------------------
                // (?: ...) is everything inside the bracket won't be captured.
                string patternYouTube = @"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)";
                Match match = Regex.Match(tempUrl, patternYouTube);
                if (match.Success == false)
                {
                    return url;
                }
                string videoId = match.Groups[1].Value.ToString();
                // -------------------------------------------------------------------

                // create an iframe Youtube video.
                tempUrl = "http://www.youtube.com/embed/" + videoId;
                YoutubeTheme yThem = youtubeTheme;
                var builder = new StringBuilder();
                builder.Append(string.Format("<iframe width=\"{0}\" height=\"{1}\" frameborder=\"0\" allowfullscreen src=\"{2}?theme={3}",
                    width, height, tempUrl, yThem.ToString().ToLower()));
                if (autoPlay)
                {
                    builder.Append("&autoplay=1");
                }
                if (removeYoutubeBranding)
                {
                    builder.Append("&modestbranding=1");
                }
                if (loop)
                {
                    builder.Append("&loop=1");
                }
                builder.Append("\"></iframe>");
                return builder.ToString();
            }
            catch (Exception)
            {
                return url;
            }
        }

        //public static string EmbedSlideShare(string url, int width = 600, int height = 40)
        //{
        //    if (string.IsNullOrEmpty(url.Trim()))
        //    {
        //        throw new Exception("Url is null or empty.");
        //    }

        //    var builder = new StringBuilder();
        //    builder.Append(string.Format("<iframe src=\"{0}\" width=\"{1}\" height=\"{2}\" frameborder=\"0\" marginwidth=\"0\" marginheight=\"0\" scrolling=\"no\" allowfullscreen></iframe>",
        //        url, width, height));
        //    return builder.ToString();
        //}


        /// <summary>
        /// Replace a Youtube, Vimeo link into videos
        /// </summary>
        /// <param name="input">The text containing the url to Youtube or Vimeo</param>
        /// <param name="width">The width of the player in pixels</param>
        /// <param name="height">The height of the player in pixels</param>
        /// <returns></returns>
        public static string EmbedVideo(string input, int width = 600, int height = 400)
        {
            string temp = input.Trim();
            if (string.IsNullOrEmpty(temp))
            {
                throw new Exception("Input is null or empty.");
            }

            // References http://stackoverflow.com/questions/10576686/c-sharp-regex-pattern-to-extract-urls-from-given-string-not-full-html-urls-but
            // \b: matches a word boundary
            // (?: ...) is everything inside the bracket won't be captured.
            // https?://|www: http or https or www are ok
            // \S: match a series of non-whitespace characters            
            // \b: match the closing word boundary.
            string urlPattern = @"\b(?:https?://|www\.)\S+\b";

            var regex = new Regex(urlPattern);
            var matchCollection = regex.Matches(input);
            foreach (Match m in matchCollection)
            {
                // vimeo link is found.
                if (m.Value.IndexOf("vimeo", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    string vimeo = EmbedVimeo(m.Value, width, height);
                    if (vimeo != m.Value)
                    {
                        //temp = temp.Replace(m.Value, "<br />" + vimeo);
                        temp = temp.Replace(m.Value, vimeo);
                    }
                } // Youtube link is found
                else if (m.Value.IndexOf("youtube", StringComparison.OrdinalIgnoreCase) > -1 ||
                    m.Value.IndexOf("youtu.be", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    string youtube = EmbedYoutube(m.Value, width, height);
                    if (youtube != m.Value)
                    {
                        //temp = temp.Replace(m.Value, "<br />" + youtube);
                        temp = temp.Replace(m.Value, youtube);
                    }
                }
            }
            return temp;
        }

        /// <summary>
        /// Replace a Vimeo link into videos
        /// </summary>
        /// <param name="url">The url to the video</param>
        /// <param name="width">The width of the player in pixels</param>
        /// <param name="height">The height of the player in pixels</param>
        /// <param name="autoPlay">Auto play video</param>
        /// <param name="loop">Play video again.</param>
        /// <returns>An iframe Viemo video</returns>
        public static string EmbedVimeo(string url, int width = 600, int height = 400, bool autoPlay = false, bool loop = false)
        {
            string tempUrl = url.Trim();
            if (string.IsNullOrEmpty(tempUrl))
            {
                throw new Exception("Url is null or empty.");
            }
            var builder = new StringBuilder();

            // get video id  -----------------------------------------------------
            // (?: ...) is everything inside the bracket won't be captured.
            // . is any character
            // * is zero or mor matches.
            // # is #
            // | is or
            // ? is zero or one match
            // http://vimeo.com/channels/staffpicks/93498336
            string vimeoPattern = @"vimeo\.com/(?:.*#|.*/)?([0-9]+)(?:.*)";
            Match match = Regex.Match(tempUrl, vimeoPattern);
            if (match.Success == false)
            {
                return url;
            }
            string videoId = match.Groups[1].Value.ToString();
            // -------------------------------------------------------------------

            // create an iframe Vimeo video.
            tempUrl = "http://player.vimeo.com/video/" + videoId;
            builder.Append(string.Format("<iframe width=\"{0}\" height=\"{1}\" frameborder=\"0\" allowfullscreen mozallowfullscreen webkitallowfullscreen src=\"{2}?title=1",
                    width, height, tempUrl));
            if (autoPlay)
            {
                builder.Append("&autoplay=1");
            }
            if (loop)
            {
                builder.Append("&loop=1");
            }
            builder.Append("\"></iframe>");

            return builder.ToString();
        }

        /// <summary>
        /// Check a string is ip address
        /// </summary>
        /// <param name="input">A string to check</param>
        /// <returns>True: valid ip. False: invalid ip.</returns>
        public static bool ValidateIPAddress(string input)
        {
            // \b: matches a word boundary
            // (?: ...) is everything inside the bracket won't be captured.
            // {3} is exactly 3 matches
            // \b: match the closing word boundary.
            string ipPattern = @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
            Match match = Regex.Match(input, ipPattern);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check person'age is old than N
        /// </summary>
        /// <param name="birthDay">BirthDay</param>
        /// <param name="age">A number to check. Ex: 18</param>
        /// <returns></returns>
        public static bool OldThan(DateTime birthDay, int age = 18)
        {
            if (DateTime.Now.Year - birthDay.Year > age)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Connvert BBCode into HTML
        /// </summary>
        /// <param name="input">BBCode string that you want to convert</param>
        /// <returns>A html string</returns>
        public static string ConvertBBCodeToHtml(string input)
        {
            string temp = input.Trim();
            if (string.IsNullOrEmpty(temp) == false)
            {
                string pattern = "";
                var listBBCode = BBCode.Create();
                foreach (var item in listBBCode)
                {
                    // Set regular expression
                    pattern = item.Key;
                    Match match = Regex.Match(temp, pattern, RegexOptions.IgnoreCase);
                    // There is BBCode in input string
                    if (match.Success)
                    {
                        if (match.Value.IndexOf("video", StringComparison.OrdinalIgnoreCase) > -1 ||
                            match.Value.IndexOf("vimeo", StringComparison.OrdinalIgnoreCase) > -1 ||
                            match.Value.IndexOf("youtube", StringComparison.OrdinalIgnoreCase) > -1 ||
                            match.Value.IndexOf("youtu.be", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            // get BBCodeVideo. Ex: BBCodeVideo is [video]http://vimeo.com/channels/staffpicks/93498336[/video]
                            string bbCodeVideo = match.Groups[0].Value;

                            // Use regex to replace BBCode video. (See list DicBBCode at Static Constructor)
                            string resultReplace = Regex.Replace(bbCodeVideo, pattern, item.Value, RegexOptions.IgnoreCase);

                            // Format of resultRepalce is string.Format("$3{0}$1{0}$2", Separator) or $1
                            // So we will split resultReplace into array.
                            string[] array = resultReplace.Split(new string[] { BBCode.SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                            string videoUrl = array[0];
                            int width = 600;
                            int height = 400;
                            if (array.Length == 3)
                            {
                                width = Convert.ToInt32(array[1]);
                                height = Convert.ToInt32(array[2]);
                            }
                            // Create a iframe video
                            string iframeVideo = EmbedVideo(videoUrl, width, height);
                            temp = temp.Replace(bbCodeVideo, iframeVideo);
                        }
                        else
                        {
                            temp = Regex.Replace(temp, pattern, item.Value, RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            return temp;
        }

        #region shorten url
        /// <summary>
        /// Fenerate a shorten url
        /// </summary>
        /// <param name="url">Url that you want to shorten</param>
        /// <returns>a shorten url </returns>
        public static string ShortenUrl(string url, ShortenUrlType shortenUrlType = ShortenUrlType.Google)
        {
            string result = "";
            switch (shortenUrlType)
            {
                case ShortenUrlType.TinyUrl:
                    result = MakeTinyUrl(url);
                    break;
                case ShortenUrlType.Isgd:
                    result = MakeIsgdUrl(url);
                    break;
                default:
                    result = MakeGoogleShortenUrl(url);
                    break;
            }
            return result;
        }

        private static string MakeGoogleShortenUrl(string url)
        {
            // http://gempixel.com/demo/simplephp
            // http://www.bipinjoshi.net/articles/09bf16f8-ad91-4638-8fce-f2e286f6fe1d.aspx
            // http://www.jarloo.com/google-url-shortening/
            // http://www.aspdotnet-suresh.com/2013/02/create-google-short-urls-in-javascript.html
            // http://www.fluxbytes.com/csharp/shortening-a-url-using-bit-bitly-api-in-c/
            try
            {
                // check url is existing ?
                if (UrlIsExists(url))
                {
                    // Make request
                    //string encondingUrl = HttpUtility.UrlEncode(url);
                    var request = WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url");
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    string postData = "{\"longUrl\":\"" + url + "\"}";
                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(postData);
                    }
                    // Get respone
                    var respone = request.GetResponse();
                    GoogleUrlResponse result;
                    using (var reader = new StreamReader(respone.GetResponseStream()))
                    {
                        // read data is returned from server and deserialize it.
                        string responeData = reader.ReadToEnd();
                        ISerializer serializer = new NewtonSoftJsonSerializer();
                        result = serializer.Deserialize<GoogleUrlResponse>(responeData);
                    }
                    if (result != null)
                    {
                        return result.Id;
                    }
                }
                return url;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static string MakeIsgdUrl(string url)
        {
            string provider = "http://is.gd/api.php?longurl=";
            return ShortenUrl(url, provider);
        }

        private static string ShortenUrl(string url, string provider)
        {
            try
            {
                // http://tinyurl.com/api-create.php?url=
                // http://stackoverflow.com/questions/366115/using-tinyurl-com-in-a-net-application-possible
                // http://www.codeproject.com/Articles/111010/Consuming-URL-Shortening-Services-is-gd
                //if (Url.Length <= 12)
                //{
                //    return Url;
                //}
                //if (!Url.ToLower().StartsWith("http") && !Url.ToLower().StartsWith("ftp"))
                //{
                //    Url = "http://" + Url;
                //}
                if (UrlIsExists(url))
                {
                    //string encondingUrl = HttpUtility.UrlEncode(provider + url);
                    var request = WebRequest.Create(provider + url);
                    var res = request.GetResponse();
                    string result;
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                    return result;
                }
                return url;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static string MakeTinyUrl(string url)
        {
            try
            {
                string provider = "http://tinyurl.com/api-create.php?url=";
                return ShortenUrl(url, provider);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Expand short Google url to original url (This method use google api)
        /// </summary>
        /// <param name="shortUrl">Short Google url</param>
        /// <returns>An orginal url</returns>
        public static string ExpandShortGoogleUrl(string shortUrl)
        {
            try
            {
                // make request
                string provider = "https://www.googleapis.com/urlshortener/v1/url?shortUrl=" + shortUrl;
                var request = WebRequest.Create(provider);
                var res = request.GetResponse();
                string responeData;
                GoogleUrlResponse result;
                // read respone data and deserialize
                using (var reader = new StreamReader(res.GetResponseStream()))
                {
                    responeData = reader.ReadToEnd();
                    //var deserializer = new JavaScriptSerializer();
                    //result = deserializer.Deserialize<GoogleUrlResponse>(responeData);
                    ISerializer serializer = new NewtonSoftJsonSerializer();
                    result = serializer.Deserialize<GoogleUrlResponse>(responeData);
                }
                if (result != null)
                {
                    return result.LongUrl;
                }
                return shortUrl;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Expand short url to original url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ExpandShortUrl(string url)
        {
            try
            {
                // http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.allowautoredirect.aspx
                // make request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                // handle redirect myself.
                request.AllowAutoRedirect = false;

                // get respone data
                HttpWebResponse res = (HttpWebResponse)request.GetResponse();

                // get Location value of Http respone header.
                if (res.Headers["Location"] != null)
                {
                    return res.Headers["Location"].ToString();
                }
                return url;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        /// <summary>
        /// Check URL is available or not.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlIsExists(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("Url is null or empty");
            }
            try
            {
                // make a request to url
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";

                // check respone
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == OK
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get video information of Vimeo
        /// </summary>
        /// <param name="url">The url to the video</param>
        /// <returns>An object VimeoInfo</returns>
        public static VimeoInfo GetVimeoInfo(string url)
        {
            try
            {
                if (UrlIsExists(url) == false)
                {
                    throw new Exception("Url Vimeo is not exist");
                }

                // get video id of Vimeo ---
                //string vimeoPattern = @"vimeo\.com/(?:.*#|.*/)?([0-9]+)(?:.*)";
                //Match match = Regex.Match(url, vimeoPattern);
                //if (match.Success == false)
                //{
                //    return null;
                //}
                //string videoId = match.Groups[1].Value.ToString();
                string videoId = GetVimeoVideoId(url);
                if (videoId == "")
                {
                    return null;
                }
                // ------------------------

                // Get video information
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                string myData = webClient.DownloadString("http://vimeo.com/api/v2/video/" + videoId + ".xml");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(myData);
                // format of xmlDocument is:
                //<?xml version="1.0" encoding="UTF-8"?>
                //<videos>
                //    <video>
                //          <id>TEXT</id>
                //          <title>TEXT</title>
                //          ....
                //    </video>
                //<videos>
                var node = xmlDocument.SelectNodes("/videos/video")[0];
                var vimeoInfo = new VimeoInfo()
                {
                    Id = node["id"].InnerText,
                    Title = node["title"].InnerText,
                    Description = node["description"].InnerText,
                    Duration = Convert.ToInt32(node["duration"].InnerText),
                    ThumbnailUrl = node["thumbnail_medium"].InnerText,
                    Url = node["url"].InnerText,
                    UploadDate = Convert.ToDateTime(node["upload_date"].InnerText),
                    Tags = node["tags"].InnerText
                };
                return vimeoInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Embed facebook comment
        /// </summary>
        /// <param name="url">You url. If you set empty or null for this parameter, this method will get current url at address bar</param>
        /// <param name="width">Width of facebook comment int pixels</param>
        /// <param name="numPost">Number of posting</param>
        /// <returns>An embeded facebook comment string</returns>
        public static string EmbedFacebokComment(string url = "", int width = 0, int numPost = 10)
        {
            try
            {
                if (width < 0)
                {
                    throw new Exception("Size must be >= 0.");
                }
                string tempUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                if (string.IsNullOrWhiteSpace(url) == false)
                {
                    tempUrl = url;
                }

                string facebookComment = string.Format("<div id=\"fb-root\"></div><script src=\"http://connect.facebook.net/en_US/all.js#xfbml=1\"></script><fb:comments href=\"{0}\" num_posts=\"{1}\" width=\"{2}\"></fb:comments>",
                    tempUrl, numPost, width);
                return facebookComment;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Get Vimeo video id
        /// </summary>
        /// <param name="url">Url to Vimeo video</param>
        /// <returns></returns>
        private static string GetVimeoVideoId(string url)
        {
            string vimeoPattern = @"vimeo\.com/(?:.*#|.*/)?([0-9]+)(?:.*)";
            Match match = Regex.Match(url, vimeoPattern);
            if (match.Success == false)
            {
                return "";
            }
            return match.Groups[1].Value.ToString();
        }


        /// <summary>
        /// Get Vimeo thumnail
        /// </summary>
        /// <param name="url">Url to Vimeo video</param>
        /// <param name="thumnailType">Vimeo thumbnail type</param>
        /// <returns>A url to thumbnail image</returns>
        public static string GetVimeoThumbnail(string url, VimeoThumnbnailType thumnailType = VimeoThumnbnailType.Medium)
        {
            try
            {
                if (UrlIsExists(url) == false)
                {
                    throw new Exception("Url Vimeo is not exist");
                }
                // get video id
                string videoId = GetVimeoVideoId(url);
                if (videoId == "")
                {
                    return url;
                }

                // Get video information
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                string myData = webClient.DownloadString("http://vimeo.com/api/v2/video/" + videoId + ".xml");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(myData);
                // format of xmlDocument is:
                //<?xml version="1.0" encoding="UTF-8"?>
                //<videos>
                //    <video>
                //          <id>TEXT</id>
                //          <title>TEXT</title>
                //          ....
                //    </video>
                //<videos>
                var node = xmlDocument.SelectNodes("/videos/video")[0];
                if (thumnailType == VimeoThumnbnailType.Small)
                {
                    return node["thumbnail_small"].InnerText;
                }
                else if (thumnailType == VimeoThumnbnailType.Large)
                {
                    return node["thumbnail_medium"].InnerText;
                }
                else
                {
                    return node["thumbnail_large"].InnerText;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get all thumbnail image of Vimeo
        /// </summary>
        /// <param name="url">Url to Vimeo video</param>
        /// <returns>An object VimeoThumbnail or null (If this method can not get Vimeo video id --> It return null)</returns>
        public static VimeoThumbnail GetAllVimeoThumbnail(string url)
        {
            try
            {
                if (UrlIsExists(url) == false)
                {
                    throw new Exception("Url Vimeo is not exist");
                }
                // get video id
                string videoId = GetVimeoVideoId(url);
                if (videoId == "")
                {
                    //throw new Exception("Can not get Vimeo video id. Please check your Viemo url");
                    return null;
                }

                // Get video information
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                string myData = webClient.DownloadString("http://vimeo.com/api/v2/video/" + videoId + ".xml");
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(myData);
                // format of xmlDocument is:
                //<?xml version="1.0" encoding="UTF-8"?>
                //<videos>
                //    <video>
                //          <id>TEXT</id>
                //          <title>TEXT</title>
                //          ....
                //    </video>
                //<videos>
                var node = xmlDocument.SelectNodes("/videos/video")[0];
                var thumbail = new VimeoThumbnail()
                {
                    Small = node["thumbnail_small"].InnerText,
                    Medium = node["thumbnail_medium"].InnerText,
                    Large = node["thumbnail_medium"].InnerText
                };
                return thumbail;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IpLocation GetLocation(string ipAddress)
        {
            try
            {
                string requestUrl = "http://www.freegeoip.net/xml/" + ipAddress;
                var request = WebRequest.Create(requestUrl);
                IpLocation result = null;
                using (WebResponse response = request.GetResponse())
                using (var stream = new StreamReader(response.GetResponseStream()))
                {
                    string responeData = stream.ReadToEnd();
                    //var deserializer = new JavaScriptSerializer();
                    //result = deserializer.Deserialize<IpLocation>(responeData);
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(responeData);
                    var node = xmlDocument.SelectNodes("/Response")[0];
                    if (node != null)
                    {
                        result = new IpLocation()
                        {
                            Ip = ipAddress,
                            CountryCode = node["CountryCode"].InnerText,
                            CountryName = node["CountryName"].InnerText,
                        };
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string DisqusComment(string userName)
        {
            var builder = new StringBuilder();
            builder.Append("<div id=\"disqus_thread\"></div>");
            builder.Append("<script type=\"text/javascript\">");
            builder.Append("var disqus_shortname = '" + "danchithancong" + "';");

            builder.Append("(function() {");
            builder.Append("var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;");
            builder.Append("dsq.src = '//' + disqus_shortname + '.disqus.com/embed.js';");
            builder.Append("(document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);");

            builder.Append("})(); </script>");
            builder.Append("<noscript>Please enable JavaScript to view the <a href=\"http://disqus.com/?ref_noscript\">comments powered by Disqus.</a></noscript>");
            builder.Append("<a href=\"http://disqus.com\" class=\"dsq-brlink\">comments powered by <span class=\"logo-disqus\">Disqus</span></a>");
            return builder.ToString();
        }
    } // end class WebHelper

    class GoogleUrlResponse
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string LongUrl { get; set; }
        public string Status { get; set; }
    }

    public class IpLocation
    {
        //private string Status { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        //public string City { get; set; }
        //public int Region_Name { get; set; }

        /// <summary>
        /// This is Ip address
        /// </summary>
        public string Ip { get; set; }
    }

    public class VimeoInfo
    {
        public string Id { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Seconds
        /// </summary>
        public int Duration { get; set; }
        public string ThumbnailUrl { get; set; }

        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime UploadDate { get; set; }
        public string Tags { get; set; }
    }

    public class VimeoThumbnail
    {
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Large { get; set; }
    }

    public class BBCode
    {
        /// <summary>
        /// This is list BBCode. Key is regular expression of BBCode. Value is html replacement.
        /// </summary>
        //static Dictionary<string, string> DicBBCode = new Dictionary<string, string>();

        static ConcurrentDictionary<string, string> DicBBCode = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Separator string.
        /// </summary>
        public const string SEPARATOR = "(*_*)";

        public static ConcurrentDictionary<string, string> Create()
        {
            if (DicBBCode.Count == 0)
            {
                // references http://www.ostree.org/code-snippet/53/bbcode-parser-with-c
                // http://www.phpclasses.org/browse/file/24732.html
                // ALLCAPS BBCode is ok.
                // [b]TEXT[/b]. Ex: [b]I am Iron Man[/b]
                DicBBCode.TryAdd(@"\[b(?:\s*)](.*?)\[/b(?:\s*)]", "<b>$1</b>");

                // [i]TEXT[/i]
                DicBBCode.TryAdd(@"\[i(?:\s*)](.*?)\[/i(?:\s*)]", "<i>$1</i>");

                // [u]TEXT[/u]
                DicBBCode.TryAdd(@"\[u(?:\s*)](.*?)\[/u(?:\s*)]", "<u>$1</u>");

                // [s]TEXT[/s]
                DicBBCode.TryAdd(@"\[s(?:\s*)](.*?)\[/s(?:\s*)]", "<s>$1</s>");

                // [center]TEXT[/center]
                DicBBCode.TryAdd(@"\[center(?:\s*)](.*?)\[/center(?:\s*)]", "<div style=\"text-align: center;\">$1</div>");

                // [left]TEXT[/left]
                DicBBCode.TryAdd(@"\[left(?:\s*)](.*?)\[/left(?:\s*)]", "<div style=\"text-align: left;\">$1</div>");

                // [right]TEXT[/right]
                DicBBCode.TryAdd(@"\[right(?:\s*)](.*?)\[/right(?:\s*)]", "<div style=\"text-align: right;\">$1</div>");

                // [size=@@@]TEXT[/size]. @@@ is font size. Ex: [size=18]Some text here[/size]
                DicBBCode.TryAdd(@"\[size(?:\s*)=(?:\s*)(.*?)(?:\s*)](.*?)\[/size(?:\s*)]", "<span style=\"font-size: $1;\">$2</span>");

                // [font=@@@]TEXT[/font]. @@@ is font name Ex: [font=Arial]Some text here[/font]
                DicBBCode.TryAdd(@"\[font(?:\s*)=(?:\s*)(.*?)(?:\s*)](.*?)\[/font(?:\s*)]", "<span style=\"font-family: $1;\">$2</span>");

                // [align=@@@]TEXT[/align]. @@@ is center or left or right. Ex: [align=center]Some text here.[/align]
                DicBBCode.TryAdd(@"\[align(?:\s*)=(?:\s*)(.*?)(?:\s*)](.*?)\[/align(?:\s*)]", "<span style=\"text-align: $1;\">$2</span>");

                // [color=@@@]TEXT[/color]. @@@ is color. Ex [color=red]Some text here[/color]
                DicBBCode.TryAdd(@"\[color(?:\s*)=(?:\s*)(.*?)(?:\s*)](.*?)\[/color(?:\s*)]", "<span style=\"color:$1;\">$2</span>");

                // [email]TEXT[/email]. [email]someone@gmail.com[/email]
                DicBBCode.TryAdd(@"\[email(?:\s*)\](.*?)\[/email(?:\s*)]", "<a href=\"mailto:$1\">$1</a>");

                // [hr]
                DicBBCode.TryAdd(@"\[hr(?:\s*)]", "<hr />");

                //[img]LINK[/img]
                //[video]LINK[/video] (either youtube or vimeo)

                // [url=LINK]TEXT[/url]. Ex: [url=http://google.com]GOOGLE[/url]
                DicBBCode.TryAdd(@"\[url(?:\s*)=(?:\s*)(.*?)(?:\s*)](.*?)\[/url(?:\s*)]", "<a class=\"bbcode-url\" href=\"$1\" target=\"_blank\" title=\"$1\">$2</a>");

                // [link=LINK]TEXT[/url]. Ex: [link=http://google.com]GOOGLE[/link]
                DicBBCode.TryAdd(@"\[link(?:\s*)=(?:\s*)(.*?)(?:\s*)](.*?)\[/link(?:\s*)]", "<a class=\"bbcode-url\" href=\"$1\" target=\"_blank\" title=\"$1\">$2</a>");

                // [img]LINK[/img]. Ex: [img]http://www.bbcode.org/images/lubeck_small.jpg[img]
                DicBBCode.TryAdd(@"\[img(?:\s*)](.*?)\[/img(?:\s*)]", "<img src=\"$1\" alt=\"\" />");

                // [img=WIDTHxHEIGHT]LINK[/img]. Ex: [img=400x300]http://www.bbcode.org/images/lubeck_small.jpg[/img]
                DicBBCode.TryAdd(@"\[img(?:\s*)=(?:\s*)(\d+)[xX](\d+)(?:\s*)](.*?)\[/img(?:\s*)]", "<img width=\"$1\" height=\"$2\" src=\"$3\" alt=\"\" />");

                // [img=width="number" height="number" alt="text" ]LINK TO IMAGE[/img].
                // Ex: [img width="100" height="50" alt="Lubeck city gate"]http://www.bbcode.org/images/lubeck_small.jpg[/img]
                //DicBBCode.TryAdd(@"\[img(?:\s*)width(?:\s*)=(?:\s*)""(.*?)""(?:\s*)height(?:\s*)=(?:\s*)""(.*?)""(?:\s*)alt(?:\s*)=(?:\s*)""(.*?)""(?:\s*)](.*?)\[/img(?:\s*)\]", "<img width=\"$1\" height=\"$2\" alt=\"$3\" src=\"$4\" />");
                DicBBCode.TryAdd(@"\[img(?:\s*)width(?:\s*)=(?:\s*)""(\d+)""(?:\s*)height(?:\s*)=(?:\s*)""(\d+)""(?:\s*)alt(?:\s*)=(?:\s*)""(.*?)""(?:\s*)](.*?)\[/img(?:\s*)]", "<img width=\"$1\" height=\"$2\" alt=\"$3\" src=\"$4\" />");

                // [img=width="number" height="number" alt="text" title="text" ]LINK TO IMAGE[/img].
                // Ex: [img width="100" height="50" alt="Lubeck city gate" title="This is one of the medieval city gates of Lubeck"]http://www.bbcode.org/images/lubeck_small.jpg[/img]
                DicBBCode.TryAdd(@"\[img(?:\s*)width(?:\s*)=(?:\s*)""(.*?)""(?:\s*)height(?:\s*)=(?:\s*)""(.*?)""(?:\s*)title(?:\s*)=(?:\s*)""(.*?)""(?:\s*)alt(?:\s*)=(?:\s*)""(.*?)""(?:\s*)](.*?)\[/img(?:\s*)]", "<img width=\"$1\" height=\"$2\" title=\"$3\" alt=\"$4\" src=\"$5\" />");

                // [youtube]LINK YOUTUBE[/youtube]. Ex: [youtube]https://www.youtube.com/watch?v=ZdP0KM49IVk[/youtube]
                DicBBCode.TryAdd(@"\[youtube(?:\s*)](.*?)\[/youtube(?:\s*)]", "$1");

                // [youtube=400x300]LINK YOUTUBE[/youtube]. Ex: [youtube=400x300]https://www.youtube.com/watch?v=ZdP0KM49IVk[/youtube]
                // $1 is width
                // $2 is height
                // $3 is LINK VIEMO
                DicBBCode.TryAdd(@"\[youtube(?:\s*)=(?:\s*)(\d+)[xX](\d+)(?:\s*)](.*?)\[/youtube(?:\s*)]", string.Format("$3{0}$1{0}$2", SEPARATOR));

                // [vimeo=400x300]LINK VIMEO[/vimeo]. Ex: [vimeo]http://vimeo.com/channels/staffpicks/93498336[/vimeo]
                DicBBCode.TryAdd(@"\[vimeo(?:\s*)](.*?)\[/vimeo(?:\s*)]", "$1");

                // [vimeo=400x300]LINK VIMEO[/vimeo]. Ex: [vimeo=400x300]http://vimeo.com/channels/staffpicks/93498336[/vimeo]            /
                DicBBCode.TryAdd(@"\[vimeo(?:\s*)=(?:\s*)(.*?)[xX](.*?)(?:\s*)](.*?)\[/vimeo(?:\s*)]", string.Format("$3{0}$1{0}$2", SEPARATOR));

                // [video]LINK VIMEO OR YOUTUBE[/video]. Ex: [video]http://vimeo.com/channels/staffpicks/93498336[/video]
                DicBBCode.TryAdd(@"\[video(?:\s*)](.*?)\[/video(?:\s*)]", "$1");

                // [video=400x300]LINK VIMEO OR YOUTUBE[/video]. Ex: [video=400x300]http://vimeo.com/channels/staffpicks/93498336[/video]
                DicBBCode.TryAdd(@"\[video(?:\s*)=(?:\s*)(.*?)[xX](.*?)(?:\s*)](.*?)\[/video(?:\s*)]", string.Format("$3{0}$1{0}$2", SEPARATOR));
            }

            return DicBBCode;
        }
    }

    public enum YoutubeTheme
    {
        Dark = 0,
        Light = 1
    }

    public enum ShortenUrlType
    {
        Google,
        TinyUrl,
        Isgd
    }

    public enum VimeoThumnbnailType
    {
        Small,
        Medium,
        Large
    }
}
