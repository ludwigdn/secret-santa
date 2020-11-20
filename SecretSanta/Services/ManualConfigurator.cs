using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using SecretSanta.Models;

namespace SecretSanta
{
    public class ManualConfigurator
    {
        private QuestionsServer _questions;

        public Config Get()
        {            
            int index = 1;

            DisplaySeparator(index++);
            var lang = GetLanguage();

            _questions = new QuestionsServer(lang);

            var config = new Config();
            config.MailBodyTitle = _questions.Get(QuestionsServer.AUTO_BODY_TITLE);
            config.MailBody = _questions.Get(QuestionsServer.AUTO_BODY);

            DisplaySeparator(index++);
            config.MailSubject = GetMailSubject();

            DisplaySeparator(index++);
            config.Participants.AddRange(GetParticipants());
            
            DisplaySeparator(index++);
            var mailProviderSettings = GetMailProviderSettings();            
            config.SmtpHost = mailProviderSettings.SMTP_HOST;
            config.SmtpPort = mailProviderSettings.SMTP_PORT;

            DisplaySeparator(index++);
            var credentials = GetUserCredentials();
            config.SmtpEmail = credentials.email;
            config.SmtpPassword = credentials.password;

            return config;
        }

        #region Questions

        private string GetLanguage()
        {
            var answer = GetAnswerFrom("Programme en Français ? Software in English? (fr/en)");
            return  answer == "fr" 
                ? "fr" 
                : "en";
        }

        private IEnumerable<Participant> GetParticipants()
        {
            int participantsCount = GetInteger(_questions, GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_PARTICIPANTS)));
            for (int i = 1 ; i <= participantsCount ; i++)
            {                
                yield return new Participant
                {
                    Name = GetAnswerFrom(string.Format(_questions.Get(QuestionsServer.QUESTION_PARTICPANT_NAME), i)),
                    Email = GetAnswerFrom(string.Format(_questions.Get(QuestionsServer.QUESTION_PARTICPANT_EMAIL), i))
                };
            }
        }

        private string GetMailSubject()
        {
            return GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_MAIL));
        }

        private MailProviderSettings GetMailProviderSettings()
        {
            var settings = MailProviderSettings.Get(AskMailProviderQuestion(_questions));
            if (settings == null)
                settings = AskCustomMailProviderSettings(_questions);
            
            return settings;
        }

        private MailProvider AskMailProviderQuestion(QuestionsServer questions)
        {
            Console.WriteLine(); 
            WriteSlow(_questions.Get(QuestionsServer.QUESTION_USER_PROVIDER));
            Console.WriteLine(); 

            System.Threading.Thread.Sleep(500);

            int idx = 1;
            var dic = new Dictionary<int, MailProvider>();
            foreach (var value in Enum.GetValues(typeof(MailProvider)))
            {
                dic.Add(idx, (MailProvider)value);
                
                var displayedValue = (MailProvider)value == MailProvider.UNKOWN
                    ? _questions.Get(QuestionsServer.QUESTION_USER_PROVIDER_UNKNOWN)
                    : $"{value}";

                Console.WriteLine($"   {idx ++}. {displayedValue}");
            }
            
            System.Threading.Thread.Sleep(500);

            var selectedProvider = GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_USER_PROVIDER_INDEX));
            return dic[GetInteger(questions, selectedProvider, idx)];
        }
        
        private MailProviderSettings AskCustomMailProviderSettings(QuestionsServer questions)
        {
            Console.WriteLine(); 
            WriteSlow(_questions.Get(QuestionsServer.QUESTION_USER_PROVIDER_MANUAL));

            var host = GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_USER_PROVIDER_HOST));
            var port = GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_USER_PROVIDER_PORT));

            return new MailProviderSettings(host, port);
        }

        private (string email, string password) GetUserCredentials()
        {
            Console.WriteLine(); 
            WriteSlow(_questions.Get(QuestionsServer.QUESTION_USER_EMAIL_SETUP));            
            var email = GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_USER_EMAIL)); 
            var password = GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_USER_PASSWORD));
            return (email, password);
        }

        public void DisplayEndProcess()
        {
            Console.WriteLine();
            Console.WriteLine($"===================================================");
            Console.WriteLine();
            WriteSlow(_questions.Get(QuestionsServer.AUTO_THANKS));
            WriteSlow(_questions.Get(QuestionsServer.AUTO_THANKS_2), false);
            Console.ReadLine();
        }
        
        #endregion Questions

        #region Utils

        private int GetInteger(QuestionsServer questions, string answer, int max = Int16.MaxValue)
        {
            int count;            
            while (!int.TryParse(answer, System.Globalization.NumberStyles.Integer, null, out count) || count > max)
            {
                Console.WriteLine();  
                WriteSlow(_questions.Get(QuestionsServer.WARNING_INTEGER));            
                answer = Console.ReadLine();
            }
            return count;
        }
       
        private string GetAnswerFrom(string question)
        {
            Console.WriteLine();  
            WriteSlow(question);
            Console.Write(" > ");
            return Console.ReadLine();
        }

        private void DisplaySeparator(int index)
        {
            System.Threading.Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine($"# {index} ===============================================");
        }

        private void WriteSlow(string text, bool addLine = true)
        {
            foreach (var c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(5);
            }

            if (addLine)
                Console.WriteLine();
        }

        #endregion Utils

        private class QuestionsServer
        {
            public static string QUESTION_MAIL = "Q.MailSubject";
            public static string QUESTION_PARTICIPANTS = "Q.Participants";
            public static string QUESTION_PARTICPANT_NAME = "Q.ParticipantName";
            public static string QUESTION_PARTICPANT_EMAIL = "Q.ParticipantEmail";
            public static string QUESTION_USER_PROVIDER = "Q.UserProvider";
            public static string QUESTION_USER_PROVIDER_INDEX = "Q.UserProviderIndex";
            public static string QUESTION_USER_PROVIDER_UNKNOWN = "Q.UserProviderUnown";
            public static string QUESTION_USER_PROVIDER_MANUAL = "Q.UserProviderManual";   
            public static string QUESTION_USER_PROVIDER_HOST = "Q.UserProviderManualSmtpHost";   
            public static string QUESTION_USER_PROVIDER_PORT = "Q.UserProviderManualSmtpPort";            
            public static string QUESTION_USER_EMAIL_SETUP = "Q.UserEmailAccountSetup";       
            public static string QUESTION_USER_EMAIL = "Q.UserEmail";
            public static string QUESTION_USER_PASSWORD = "Q.UserPassword";
            public static string AUTO_BODY = "Auto.Body";
            public static string AUTO_THANKS = "Auto.Thanks";
            public static string AUTO_THANKS_2 = "Auto.Thanks2";
            public static string AUTO_BODY_TITLE = "Auto.BodyTitle";
            public static string WARNING_INTEGER = "Warning.BadInteger";

            private JObject _locale;

            public QuestionsServer(string lang)
            {
                var file = File.ReadAllText($"./locale/config.{lang}.json");
                _locale = JObject.Parse(file);
            }

            public string Get(string path)
            {
                return (string)_locale.SelectToken(path);
            }
        }
    }
}