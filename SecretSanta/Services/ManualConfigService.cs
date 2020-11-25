using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using SecretSanta.Models;
using SecretSanta.Utils;

namespace SecretSanta.Services
{
    public class ManualConfigService
    {
        private int _step = 1;
        private LocaleService _localeService;

        public string Locale { get; private set; }

        public ManualConfigService()
        {
            DisplaySeparator(_step++);
            Locale = GetLanguage();
        }

        public Config GetConfig(LocaleService localeService)
        {
            _localeService = localeService;

            var config = new Config();
            config.Locale = Locale;

            config.MailBodyTitle = _localeService.Get(LocaleService.AUTO_BODY_TITLE);
            config.MailBody = _localeService.Get(LocaleService.AUTO_BODY);

            DisplaySeparator(_step++);
            config.MailSubject = GetMailSubject();

            DisplaySeparator(_step++);
            config.Participants.AddRange(GetParticipants());
            
            DisplaySeparator(_step++);
            var mailProviderSettings = GetMailProviderSettings();            
            config.SmtpHost = mailProviderSettings.SMTP_HOST;
            config.SmtpPort = mailProviderSettings.SMTP_PORT;

            DisplaySeparator(_step++);
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
            WriteSlow(_localeService.Get(LocaleService.QUESTION_PARTICIPANTS)); 
            Console.WriteLine(); 
            int participantsCount = GetInteger(GetAnswerFrom(_localeService.Get(LocaleService.QUESTION_PARTICIPANTS_COUNT)));

            var list = new List<Participant>();
            for (int i = 1 ; i <= participantsCount ; i++)
            {   
                var participant = new Participant();

                participant.Name = CheckDistinctString(
                    GetAnswerFrom(string.Format(_localeService.Get(LocaleService.QUESTION_PARTICPANT_NAME), i)), 
                    list.Select(o => o.Name), 
                    _localeService.Get(LocaleService.WARNING_NAME_EXISTS));

                participant.Email = CheckDistinctString(
                    GetAnswerFrom(string.Format(_localeService.Get(LocaleService.QUESTION_PARTICPANT_EMAIL), i)), 
                    list.Select(o => o.Email), 
                    _localeService.Get(LocaleService.WARNING_EMAIL_EXISTS));

                list.Add(participant);
            }
            return list;
        }

        private string GetMailSubject()
        {
            return GetAnswerFrom(_localeService.Get(LocaleService.QUESTION_MAIL));
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
            WriteSlow(_localeService.Get(LocaleService.QUESTION_USER_PROVIDER));
            Console.WriteLine(); 

            System.Threading.Thread.Sleep(500);

            int idx = 1;
            var dic = new Dictionary<int, MailProvider>();
            foreach (var value in Enum.GetValues(typeof(MailProvider)))
            {
                dic.Add(idx, (MailProvider)value);
                
                var displayedValue = (MailProvider)value == MailProvider.UNKOWN
                    ? _localeService.Get(LocaleService.QUESTION_USER_PROVIDER_UNKNOWN)
                    : $"{value}";

                Console.WriteLine($"   {idx ++}. {displayedValue}");
            }
            
            System.Threading.Thread.Sleep(500);

            var selectedProvider = GetAnswerFrom(_localeService.Get(LocaleService.QUESTION_USER_PROVIDER_INDEX));
            return dic[GetInteger(selectedProvider, max: idx)];
        }
        
        private MailProviderSettings AskCustomMailProviderSettings()
        {
            Console.WriteLine(); 
            WriteSlow(_localeService.Get(LocaleService.QUESTION_USER_PROVIDER_MANUAL));

            var host = GetAnswerFrom(_localeService.Get(LocaleService.QUESTION_USER_PROVIDER_HOST));
            var port = GetAnswerFrom(_localeService.Get(LocaleService.QUESTION_USER_PROVIDER_PORT));

            return new MailProviderSettings(host, port);
        }

        private (string email, string password) GetUserCredentials()
        {
            Console.WriteLine(); 
            WriteSlow(_localeService.Get(LocaleService.QUESTION_USER_EMAIL_SETUP));            
            var email = GetAnswerFrom(_localeService.Get(LocaleService.QUESTION_USER_EMAIL)); 
            var password = GetAnswerFrom(_localeService.Get(LocaleService.QUESTION_USER_PASSWORD));
            Console.WriteLine();  
            return (email, password);
        }

        public void DisplayEndProcess()
        {
            Console.WriteLine();
            Console.WriteLine($"===================================================");
            Console.WriteLine();
            WriteSlow(_localeService.Get(LocaleService.AUTO_THANKS));
            WriteSlow(_localeService.Get(LocaleService.AUTO_THANKS_2), false);
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
                WriteSlow(errorMessage ?? _localeService.Get(LocaleService.WARNING_INTEGER));  
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

        private void DisplaySeparator(int _step)
        {
            System.Threading.Thread.Sleep(500);
            Console.WriteLine();
            Console.WriteLine($"# {_step} ===============================================");
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

    }
}