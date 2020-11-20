using System;
using System.Linq;
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
            Console.WriteLine(); 
            WriteSlow("Langue du programme / Software language");
            Console.WriteLine(); 

            Console.WriteLine($"   1. Français");
            Console.WriteLine($"   2. English");

            Console.WriteLine(); 
            WriteSlow("Entrez le n° correspondant / Enter the corresponding number:");
            Console.Write(" > ");

            var lang = GetInteger(ReadLine(), max: 2, errorMessage: "Saisie invalide, veuillez réessayer. Invalid input, please try again.");
            return lang == 1 
                ? "fr" 
                : "en";
        }

        private List<Participant> GetParticipants()
        {
            Console.WriteLine(); 
            WriteSlow(_questions.Get(QuestionsServer.QUESTION_PARTICIPANTS)); 
            Console.WriteLine(); 
            int participantsCount = GetInteger(GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_PARTICIPANTS_COUNT)));

            var list = new List<Participant>();
            for (int i = 1 ; i <= participantsCount ; i++)
            {   
                var participant = new Participant();

                participant.Name = CheckDistinctString(
                    GetAnswerFrom(string.Format(_questions.Get(QuestionsServer.QUESTION_PARTICPANT_NAME), i)), 
                    list.Select(o => o.Name), 
                    _questions.Get(QuestionsServer.WARNING_NAME_EXISTS));

                participant.Email = CheckDistinctString(
                    GetAnswerFrom(string.Format(_questions.Get(QuestionsServer.QUESTION_PARTICPANT_EMAIL), i)), 
                    list.Select(o => o.Email), 
                    _questions.Get(QuestionsServer.WARNING_EMAIL_EXISTS));

                list.Add(participant);
            }
            return list;
        }

        private string GetMailSubject()
        {
            return GetAnswerFrom(_questions.Get(QuestionsServer.QUESTION_MAIL));
        }

        private MailProviderSettings GetMailProviderSettings()
        {
            var settings = MailProviderSettings.Get(AskMailProviderQuestion());
            if (settings == null)
                settings = AskCustomMailProviderSettings();
            
            return settings;
        }

        private MailProvider AskMailProviderQuestion()
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
            return dic[GetInteger(selectedProvider, max: idx)];
        }
        
        private MailProviderSettings AskCustomMailProviderSettings()
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
            Console.WriteLine();  
            return (email, password);
        }

        public void DisplayEndProcess()
        {
            Console.WriteLine();
            Console.WriteLine($"===================================================");
            Console.WriteLine();
            WriteSlow(_questions.Get(QuestionsServer.AUTO_THANKS));
            WriteSlow(_questions.Get(QuestionsServer.AUTO_THANKS_2), false);
            ReadLine();
        }
        
        #endregion Questions

        #region Utils

        private int GetInteger(string answer, int min = 1, int max = Int16.MaxValue, string errorMessage = null)
        {
            int count;            
            while (!int.TryParse(answer, System.Globalization.NumberStyles.Integer, null, out count) || count < min || count > max)
            {
                Console.WriteLine();  
                WriteSlow(errorMessage ?? _questions.Get(QuestionsServer.WARNING_INTEGER));  
                Console.Write(" > ");         
                answer = ReadLine();
            }
            return count;
        }
        
        private string CheckDistinctString(string answer, IEnumerable<string> existing, string errorMessage)
        {
            while (existing.Any(o => o.ToLowerInvariant() == answer.ToLowerInvariant()))
            {
                Console.WriteLine();  
                WriteSlow(errorMessage);  
                Console.Write(" > ");         
                answer = ReadLine();
            }
            return answer;
        }
       
        private string GetAnswerFrom(string question)
        {
            Console.WriteLine();  
            WriteSlow(question);
            Console.Write(" > ");
            return ReadLine();
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

        private string ReadLine()
        {
            return Console.ReadLine()?.Trim();
        }

        #endregion Utils

        private class QuestionsServer
        {
            public static string QUESTION_MAIL = "Q.MailSubject";
            public static string QUESTION_PARTICIPANTS = "Q.Participants";
            public static string QUESTION_PARTICIPANTS_COUNT = "Q.ParticipantsCount";
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
            public static string WARNING_NAME_EXISTS = "Warning.NameExists";
            public static string WARNING_EMAIL_EXISTS = "Warning.EmailExists";

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