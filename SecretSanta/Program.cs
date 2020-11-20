using CommandLine;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Bson;
using System;
using System.IO;
using System.Linq;

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
            var doManualConfig = args == null || !args.Any();

            if (args == null || !args.Any())
            {
                var configurator = new ManualConfigurator();
                SetConfig(configurator.Get());
                configurator.DisplayEndProcess();
                return;
            }  
            
            Parser.Default.ParseArguments<Options>(args).WithParsed(SetConfig);
        }

        private static void SetConfig(Options opt)
        {
            Config.Instance.Parse(opt.ConfigPath?.Trim());
            AssignSantasAndSendEmails();
        }

        private static void SetConfig(Config config)
        {
            Config.Instance.Parse(config);
            AssignSantasAndSendEmails();
        }

        private static void AssignSantasAndSendEmails()
        {
            var randomizer = new Randomizer();
            var santas = randomizer.Randomize(Config.Instance.Participants);
            new MailSender().Send(santas).Wait();
        }        
    }
}
