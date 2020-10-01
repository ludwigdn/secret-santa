using CommandLine;
using System;
using System.IO;

namespace SecretSanta
{
    class Program
    {
        public class Options
        {
            [Option('c', "config", Required = true, HelpText = "Sets the config filepath.")]
            public string ConfigPath { get; set; }
        }

        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(opts => SetSingleton(opts));
            AssignSantasAndSendEmails();
        }

        private static void AssignSantasAndSendEmails()
        {
            var randomizer = new Randomizer();
            var santas = randomizer.Randomize(Config.Instance.Participants);
            new MailSender().Send(santas).Wait();
        }

        private static void SetSingleton(Options opt)
        {
            Config.Instance.Parse(opt.ConfigPath);
        }
    }
}
