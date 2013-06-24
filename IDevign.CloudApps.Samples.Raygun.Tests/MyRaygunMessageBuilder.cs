using System;
using System.Collections.Generic;
using System.Net.Mail;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Messages;

namespace IDevign.CloudApps.Samples.Raygun.Tests
{
    public class MyRaygunMessageBuilder
    {
        readonly IRaygunMessageBuilder _builder;
        readonly Dictionary<string, string> _customData = new Dictionary<string, string>();

        public MyRaygunMessageBuilder()
        {
            _builder = RaygunMessageBuilder.New
                                           .SetEnvironmentDetails()
                                           .SetMachineName(Environment.MachineName)
                                           .SetClientDetails()
                                           .SetVersion();
        }

        public MyRaygunMessageBuilder SetException(Exception exception)
        {
            _builder.SetExceptionDetails(exception);
            return this;
        }

        public MyRaygunMessageBuilder SetCustomData(string key, string value)
        {
            _customData[key] = value;
            return this;
        }

        public MyRaygunMessageBuilder SetUser(User user)
        {
            if (user == null) return this;
            return SetCustomData("User", user.DisplayName).SetCustomData("Email", user.Email.ToString());
        }

        public RaygunMessage Build()
        {
            if (_customData.Count > 0) _builder.SetUserCustomData(_customData);
            return _builder.Build();
        }
    }

    public class User
    {
        public User(string displayName, MailAddress email)
        {
            DisplayName = displayName;
            Email = email;
        }

        public string DisplayName { get; set; }
        public MailAddress Email { get; set; }
    }
}