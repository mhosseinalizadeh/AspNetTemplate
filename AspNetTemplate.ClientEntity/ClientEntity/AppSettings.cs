using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.ClientEntity
{
        public class AppSettings
        {
            public MailSettings MailSettings { get; set; }
        }

        public class MailSettings
        {
            public string Host { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
            public string Port { get; set; }
            public string From { get; set; }
        }
}
