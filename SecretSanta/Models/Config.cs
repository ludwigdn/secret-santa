using Newtonsoft.Json;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SecretSanta
{
    public sealed class Config
    {
        [DataMember]
        public string Locale { get; set; }
        
        [DataMember]
        public string MailSubject { get; set; }

        [DataMember]
        public string MailBodyTitle { get; set; }

        [DataMember]
        public string MailBody { get; set; }

        [DataMember]
        public string SmtpEmail { get; set; }

        [DataMember]
        public string SmtpPassword { get; set; }

        [DataMember]
        public string SmtpHost { get; set; }

        [DataMember]
        public string SmtpPort { get; set; }

        [DataMember]
        public List<Participant> Participants { get; set; } = new List<Participant>();
        public Config() { }     

        public void Parse(string filePath)
        {
            var fileContent = File.ReadAllText(filePath);
            var config = JsonConvert.DeserializeObject<Config>(fileContent);
            Locale = config.Locale;
            MailSubject = config.MailSubject;
            MailBodyTitle = config.MailBodyTitle;
            MailBody = config.MailBody;
            SmtpEmail = config.SmtpEmail;
            SmtpPassword = config.SmtpPassword;
            SmtpHost = config.SmtpHost;
            SmtpPort = config.SmtpPort;
            Participants = config.Participants;
        }

        public void Parse(Config config)
        {
            Locale = config.Locale;
            MailSubject = config.MailSubject;
            MailBodyTitle = config.MailBodyTitle;
            MailBody = config.MailBody;
            SmtpEmail = config.SmtpEmail;
            SmtpPassword = config.SmtpPassword;
            SmtpHost = config.SmtpHost;
            SmtpPort = config.SmtpPort;
            Participants = config.Participants;
        }  

        #region Singleton stuff

        private static Config instance = null;
        private static readonly object instanceLock = new object();


        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                            instance = new Config();                        
                    }
                }
                
                return instance;
            }
        }

        #endregion Singleton stuff
    }
}
