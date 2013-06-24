
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Contrib;
using RestSharp.Authenticators;
using System.Diagnostics;

namespace RestSharpSamples
{
    [TestClass]
    public class RestSharpSamples
    {
        [TestMethod]
        public void Can_Authenticate_With_OAuth()
        {
            const string consumerKey = "HEGlGLuqfR1Aa8CwJNB0sw";
            const string consumerSecret = "D4UC98WMZbPaLztm47b7TwktQBiPhqpqJUd99hPhqw";

            var baseUrl = "http://api.twitter.com";
            var client = new RestClient(baseUrl);
            client.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
            // Token anfragen für Authorisierung
            var request = new RestRequest("oauth/request_token", Method.POST);
            var response = client.Execute(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var qs = HttpUtility.ParseQueryString(response.Content);
            var oauth_token = qs["oauth_token"];
            var oauth_token_secret = qs["oauth_token_secret"];
            Assert.IsNotNull(oauth_token);
            Assert.IsNotNull(oauth_token_secret);

            // Authorisierung starten
            request = new RestRequest("oauth/authorize");
            request.AddParameter("oauth_token", oauth_token);
            var url = client.BuildUri(request).ToString();
            Process.Start(url);

            // mit Authorisierungs-PIN ein Zugriffs-Token anfordern
            var verifier = "8083248"; // <-- Breakpoint here (set verifier in debugger)
            request = new RestRequest("oauth/access_token", Method.POST);
            client.Authenticator = OAuth1Authenticator.ForAccessToken(
                consumerKey, consumerSecret, oauth_token, oauth_token_secret, verifier
                );
            response = client.Execute(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            qs = HttpUtility.ParseQueryString(response.Content);
            oauth_token = qs["oauth_token"];
            oauth_token_secret = qs["oauth_token_secret"];
            Assert.IsNotNull(oauth_token);
            Assert.IsNotNull(oauth_token_secret);

            request = new RestRequest("1.1/statuses/user_timeline.json");
            request.AddParameter("count", 30);
            client.Authenticator = OAuth1Authenticator.ForProtectedResource(
                consumerKey, consumerSecret, oauth_token, oauth_token_secret
                );

            var typedResponse = client.Execute<List<Tweet>>(request);
            var tweets = typedResponse.Data;

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            Assert.IsNotNull(tweets);
            Assert.IsTrue(tweets.Count > 0);
        }
    }

    public class Tweet
    {
        public string Text { get; set; }
        public string CreatedAt { get; set; }

        public override string ToString()
        {
            return this.Text;
        }
    }
}