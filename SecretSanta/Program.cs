using CommandLine;
using System.Linq;
using SecretSanta.Services;
using System.Threading.Tasks;

namespace SecretSanta
{
    class Program
    {
        public class Options
        {
            [Option('c', "config", HelpText = "Sets the config filepath.")]
            public string ConfigPath { get; set; }

            [Option('d', "dry-run", HelpText = "At the end of the process, won´t sent the emails.")]
            public bool DryRun { get; set; }
        }

        private static void Main(string[] args)
        {
            Init(args).GetAwaiter().GetResult();
        }

        private static async Task Init(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async opt =>
            {
                Config.Instance.Parse(opt.ConfigPath?.Trim(), opt.DryRun);
                await AssignSantasAndSendEmails();
            });
        }

        private static async Task AssignSantasAndSendEmails()
        {
            var santas = new RandomizeService().Randomize(Config.Instance.Participants);
            if (santas.Count == 0)
            {
                return;
            }

            var mailservice = new MailService();

            // Todo: try to put back Task.WhenAll on Send() without having 'too much connections' issue
            foreach (var santa in santas)
            {
                var localeService = new LocaleService(santa.Locale ?? Config.Instance.Locale);
                var mailMessageFormatter = new MailMessageFormatter(localeService);
                var mailBody = mailMessageFormatter.GetHtmlBody(santa);
                var mailSubject = localeService.Get(LocaleService.AUTO_SUBJECT);
                await mailservice.Send(santa, mailBody, mailSubject);
            }
        }
    }
}
