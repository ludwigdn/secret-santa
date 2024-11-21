using Newtonsoft.Json;
using SecretSanta.Models;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace SecretSanta
{
    public sealed class Config
    {
        [DataMember]
        public bool DryRun { get; set; }

        [DataMember]
        public string Locale { get; set; }

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

        public void Parse(string filePath, bool dryRun)
        {
            DryRun = dryRun || false;
            var fileContent = File.ReadAllText(filePath);
            var config = JsonConvert.DeserializeObject<Config>(fileContent);
            Locale = config.Locale;
            SmtpEmail = config.SmtpEmail;
            SmtpPassword = config.SmtpPassword;
            SmtpHost = config.SmtpHost;
            SmtpPort = config.SmtpPort;
            Participants = config.Participants;
        }

        public void Parse(Config config)
        {
            Locale = config.Locale;
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
