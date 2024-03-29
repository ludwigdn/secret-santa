﻿using CommandLine;
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
                Config.Instance.Parse(opt.ConfigPath?.Trim(), opt.DryRun);
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
            if (santas.Count == 0)
            {
              return;
            }

            var mailservice = new MailService();
            var mailMessageFormatter = new MailMessageFormatter(localeService);

            // Todo: try to put back Task.WhenAll on Send() without having 'too much connections' issue
            foreach (var santa in santas)
            {
                var body = mailMessageFormatter.GetHtmlBody(santa);
                await mailservice.Send(santa, body);
            }
        }
    }
}
