using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using SecretSanta.Interfaces;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta
{
    public class MailSender : ImailSender
    {
        public async Task Send(List<Participant> participants)
        {
            string SMTP_USERNAME = Config.Instance.SmtpEmail;
            string SMTP_PASSWORD = Config.Instance.SmtpPassword;
            string HOST = Config.Instance.SmtpHost;
            int PORT = int.Parse(Config.Instance.SmtpPort);

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(HOST, PORT, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(SMTP_USERNAME, SMTP_PASSWORD);

                var tasks = new List<Task>();

                foreach (var santa in participants)
                    tasks.Add(Send(santa, emailClient).ContinueWith(o => { Log(santa); }));

                await Task.WhenAll(tasks);

                emailClient.Disconnect(true);
            }
        }

        private void Log(Participant santa)
        {
            Console.WriteLine("Sent mail to " + santa.Email);
        }

        public Task Send(Participant santa, SmtpClient client)
        {
            #region Values

            string FROM = Config.Instance.SmtpEmail;
            string TO = santa.Email;
            string SUBJECT = Config.Instance.MailSubject;
            string BODY = GetHtmlBody(santa);

            #endregion Values

            var msg = new MimeMessage();
            msg.To.AddRange(new List<MailboxAddress> { new MailboxAddress(TO) });
            msg.From.AddRange(new List<MailboxAddress> { new MailboxAddress(FROM) });
            msg.Subject = SUBJECT;
            msg.Body = new TextPart(TextFormat.Html)
            {
                Text = BODY
            };

            return client.SendAsync(msg);
        }

        private string GetHtmlBody(Participant santa)
        {
            var html = File.ReadAllText("MailContent.html"); ;

            return string.Format(html, 
                Config.Instance.MailBodyTitle, 
                string.Format(Config.Instance.MailBody, santa.ReceiverName));
        }
    }
}
