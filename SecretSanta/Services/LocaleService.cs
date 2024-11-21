using System.IO;
using Newtonsoft.Json.Linq;

namespace SecretSanta.Services
{
    public class LocaleService
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
        public static string AUTO_SUBJECT = "Auto.Subject";
        public static string AUTO_BODY = "Auto.Body";
        public static string AUTO_BODY_WITH_GIFTS = "Auto.BodyWithGifts";
        public static string AUTO_THANKS = "Auto.Thanks";
        public static string AUTO_THANKS_2 = "Auto.Thanks2";
        public static string AUTO_BODY_TITLE = "Auto.BodyTitle";
        public static string WARNING_INTEGER = "Warning.BadInteger";
        public static string WARNING_NAME_EXISTS = "Warning.NameExists";
        public static string WARNING_EMAIL_EXISTS = "Warning.EmailExists";

        private JObject _jsonObject;

        public string Locale { get; private set; }

        public LocaleService(string locale)
        {
            Locale = locale;
            var file = File.ReadAllText($"./locale/{locale}.json");
            _jsonObject = JObject.Parse(file);
        }

        public string Get(string path)
        {
            return (string)_jsonObject.SelectToken(path);
        }
    }
}
