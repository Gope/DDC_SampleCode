using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace IDevign.CloudApps.Samples.Mailgun.Tests
{
    [TestClass]
    public class MailgunSamples
    {
        [TestMethod]
        public void SendSimpleMail()
        {
           RestClient client = new RestClient();
           client.BaseUrl = "https://api.mailgun.net/v2";
           client.Authenticator =
                   new HttpBasicAuthenticator("api",
                                              "key-733f4bye56mmctvs5s09evpqae4rkge6");
           RestRequest request = new RestRequest();
           request.AddParameter("domain",
                                "idevign.mailgun.org", ParameterType.UrlSegment);
           request.Resource = "{domain}/messages";
           request.AddParameter("from", "Gope <gope@idevign.mailgun.org>");
           request.AddParameter("to", "gerrit.puddig@conjin.de");
           request.AddParameter("subject", "Hello");
           request.AddParameter("text", "Testing some Mailgun awesomness!");
           request.Method = Method.POST;
           IRestResponse response = client.Execute(request);

           Assert.IsNotNull(response);
           Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        [TestMethod]
        public void CreateMailingList()
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               "key-733f4bye56mmctvs5s09evpqae4rkge6");
            RestRequest request = new RestRequest();
            request.Resource = "lists";
            request.AddParameter("address", "dev@idevign.mailgun.org");
            request.AddParameter("description", "Mailgun developers list");
            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            // Add Member
            this.AddListMember("gerrit.puddig@conjin.de", "Gerrit Puddig", 35);

            this.SendMail("dev@idevign.mailgun.org");
        }

        private IRestResponse AddListMember(string memberEmail, string memberName, int age)
        {
            RestClient client = new RestClient();
            client.BaseUrl = "https://api.mailgun.net/v2";
            client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                               "key-733f4bye56mmctvs5s09evpqae4rkge6");
            RestRequest request = new RestRequest();
            request.Resource = "lists/{list}/members";
            request.AddParameter("list", "dev@idevign.mailgun.org", ParameterType.UrlSegment);
            request.AddParameter("address", memberEmail);
            request.AddParameter("subscribed", true);
            request.AddParameter("name", memberName);
            request.AddParameter("description", "Developer");
            request.AddParameter("vars", "{\"age\": + " + age + "}");
            request.Method = Method.POST;
            return client.Execute(request);
        }

        private void SendMail(string email)
        {
            RestClient client = new RestClient();
           client.BaseUrl = "https://api.mailgun.net/v2";
           client.Authenticator =
                   new HttpBasicAuthenticator("api",
                                              "key-733f4bye56mmctvs5s09evpqae4rkge6");
           RestRequest request = new RestRequest();
           request.AddParameter("domain",
                                "idevign.mailgun.org", ParameterType.UrlSegment);
           request.Resource = "{domain}/messages";
           request.AddParameter("from", "Gope <dev@idevign.mailgun.org>");
           request.AddParameter("to", "gerrit.puddig@conjin.de");
           request.AddParameter("subject", "Mailing List Message");
           request.AddParameter("text", "Testing some Mailgun awesomness!");
           request.Method = Method.POST;
           client.Execute(request);
        }
    }
}
