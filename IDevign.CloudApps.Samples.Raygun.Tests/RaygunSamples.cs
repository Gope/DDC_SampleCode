using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mindscape.Raygun4Net;

namespace IDevign.CloudApps.Samples.Raygun.Tests
{
    [TestClass]
    public class RaygunSamples
    {
        [TestMethod]
        public void SimpleSample()
        {
            RaygunClient client = new RaygunClient("6Zq4x7UB+5mdlq8eigy0Ow==");
            RaygunSettings.Settings.ThrowOnError = true;
            client.SendInBackground(new StackOverflowException("Houston, we have a problem!"));

            MyRaygunMessageBuilder message = new MyRaygunMessageBuilder()
                                                    .SetException(new StackOverflowException("Custom Stuff"))
                                                    .SetUser(new User("Gerrit Puddig",
                                                             new MailAddress("gerrit.puddig@conjin.de")));
            client.Send(message.Build());
            //client.SendInBackground(message.Build());
        }
    }
}
