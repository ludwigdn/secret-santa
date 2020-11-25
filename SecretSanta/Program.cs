using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Bson;
using System;
using System.IO;
using System.Linq;
using SecretSanta.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta
{
    class Program
    {
        public class Options
        {
            [Option('c', "config", HelpText = "Sets the config filepath.")]
            public string ConfigPath { get; set; }
        }

        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {            
            if (!args?.Any() ?? true)
            {
                await SetManualConfig();
            }   
            else
            {
                await SetAutoConfig(args);
            }
        }

        private static async Task SetAutoConfig(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async opt => 
            {
                Config.Instance.Parse(opt.ConfigPath?.Trim());
                var localeService = new LocaleService(Config.Instance.Locale);
                await AssignSantasAndSendEmails(localeService);
            });
        }

        private static async Task SetManualConfig()
        {
            var configurator = new ManualConfigService();
            var localeService = new LocaleService(configurator.Locale);

            var config = configurator.GetConfig(localeService);
            Config.Instance.Parse(config);

            await AssignSantasAndSendEmails(localeService);

            configurator.DisplayEndProcess();
        }

        private static async Task AssignSantasAndSendEmails(LocaleService localeService)
        {
            var santas = new RandomizeService().Randomize(Config.Instance.Participants);

            var mailservice = new MailService();
            var mailMessageFormatter = new MailMessageFormatter(localeService);
            
            var tasks = new List<Task>();
            foreach (var santa in santas)
            {
                var body = mailMessageFormatter.GetHtmlBody(santa);
                tasks.Add(mailservice.Send(santa, body));
            }
            
            await Task.WhenAll(tasks);
        }        
    }
}
