using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace TwitterApiGraphics.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private const String oauthAccesToken = "2322834594-0VEeV0OSw3Bfml9EftKNAstS5fotGtbwicYDOgR";
        private const String oauthAccessTokenSecret = "2HV2KqLjd6lAviIC5zRXsOxLXYrklzrF3uHWlu4knGFV1";
        private const String consumerKey = "xkqVa14Tf6a7iJoD65LjHMSN6";
        private const String consumerKeySecret = "X9NyUNXjhS6VF2ciyKryMCZ5T8RgRe8l5hb8Ss4JgLf5LJRps4";
        private const String userId = "2322834594";
        private const String screenName = "twitter";
        private const int count = 5;


        public ActionResult GetTweets(String searchFor, int numberToGet)
        {
            return Content(ProcessRequest(System.Web.HttpContext.Current, searchFor, numberToGet));
            //return Content("Test Response");
        }

        public String ProcessRequest(HttpContext context, String searchFor, int numberToGet)
        {
            // get these from somewhere nice and secure...
            var key = consumerKey; //ConfigurationManager.AppSettings["twitterConsumerKey"];
            var secret = consumerKeySecret;//ConfigurationManager.AppSettings["twitterConsumerSecret"];
            var server = System.Web.HttpContext.Current.Server;
            var bearerToken = server.UrlEncode(key) + ":" + server.UrlEncode(secret);
            var b64Bearer = Convert.ToBase64String(Encoding.Default.GetBytes(bearerToken));
            using (var wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8");
                wc.Headers.Add("Authorization", "Basic " + b64Bearer);
                var tokenPayload = wc.UploadString("https://api.twitter.com/oauth2/token", "grant_type=client_credentials");
                var rgx = new Regex("\"access_token\"\\s*:\\s*\"([^\"]*)\"");
                // you can store this accessToken and just do the next bit if you want
                var accessToken = rgx.Match(tokenPayload).Groups[1].Value;
                wc.Headers.Clear();
                wc.Headers.Add("Authorization", "Bearer " + accessToken);

                string url = "https://api.twitter.com/1.1/search/tweets.json?q=" + Url.Encode(searchFor) + "&count=" + numberToGet;
                //string url = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=" + searchFor + "&count=" + count;
                // ...or you could pass through the query string and use this handler as if it was the old user_timeline.json
                // but only with YOUR Twitter account

                var tweets = wc.DownloadString(url);
                return tweets;

                ////do something sensible for caching here:
                //context.Response.AppendHeader("Cache-Control", "public, s-maxage=300, max-age=300");
                //context.Response.AppendHeader("Last-Modified", DateTime.Now.ToString("r", DateTimeFormatInfo.InvariantInfo));
                //context.Response.AppendHeader("Expires", DateTime.Now.AddMinutes(5).ToString("r", DateTimeFormatInfo.InvariantInfo));
                //context.Response.ContentType = "text/plain";
                //context.Response.Write("hello" + tweets);

                //var test = 1;
            }
        }
    }
}