using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public int SmtpServerPort { get; set; }
        public bool EmableSsl { get; set; }
        public string EmailDisplayName { get; set; }
        public string SenderName { get; set; }

        public EmailSettings() { }
        public EmailSettings(string smtpServer, string smtpUsername, string smtpPassword, int smtpServerPort)
        {
            SmtpServer = smtpServer;
            SmtpUsername = smtpUsername;
            SmtpPassword = smtpPassword;
            SmtpServerPort = smtpServerPort;
        }
    }
}
