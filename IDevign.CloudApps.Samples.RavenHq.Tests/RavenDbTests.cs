using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Document;

namespace IDevign.CloudApps.Samples.RavenHq.Tests
{
    [TestClass]
    public class RavenDbTests
    {
        [TestMethod]
        public void ConnectSaveAndLoad()
        {
            // GET NUR WENN DER SERVER LÄUFT 
            // -------------------------------

            var documentStore = new DocumentStore()
                {
                    Url = "http://localhost:8080"
                };
            documentStore.Initialize();

            using (var session = documentStore.OpenSession())
            {
                var mail1 = new Email() { From = "sabine@conjin.de", To = "paul.paulsen@conjin.de" };
                var mail2 = new Email() { From = "paul.paulsen@conjin.de", To = "sabine@conjin.de" };
                session.Store(mail1);
                session.Store(mail2);
                session.SaveChanges();
            }

            List<Email> emails = null;
            using (var session = documentStore.OpenSession())
            {
                emails = session.Query<Email>().ToList();
            }

            Assert.IsNotNull(emails);
        }
    }

    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
    }
}
