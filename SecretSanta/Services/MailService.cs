using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using SecretSanta.Interfaces;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SecretSanta.Services
{
    public class MailService : ImailSender
    {
        private readonly string SMTP_USERNAME;
        private readonly string SMTP_PASSWORD;
        private readonly string HOST;
        private readonly int PORT;

        public MailService()
        {
            SMTP_USERNAME = Config.Instance.SmtpEmail;
            SMTP_PASSWORD = Config.Instance.SmtpPassword;
            HOST = Config.Instance.SmtpHost;
            PORT = int.Parse(Config.Instance.SmtpPort);
        }

        public async Task Send(Participant santa, string body)
        {
            using (var emailClient = new SmtpClient())
            {
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                await emailClient.ConnectAsync(HOST, PORT, true);
                await emailClient.AuthenticateAsync(SMTP_USERNAME, SMTP_PASSWORD);

                var msg = new MimeMessage();
                msg.To.AddRange(new List<MailboxAddress> { MailboxAddress.Parse(santa.Email) });
                msg.From.AddRange(new List<MailboxAddress> { MailboxAddress.Parse(Config.Instance.SmtpEmail) });
                msg.Subject = Config.Instance.MailSubject;
                msg.Body = new TextPart(TextFormat.Html) { Text = body };

                try
                {
                    if (!Config.Instance.DryRun)
                    {
                      await emailClient.SendAsync(msg);
                      Console.WriteLine($" > {santa.Email}: ok! (Dryrun: {Config.Instance.DryRun})");
                      return;
                    }

                    Console.WriteLine($" > Dryrun: {Config.Instance.DryRun} - {santa}");
                }
                catch (Exception ex)
                {
                    // todo put this in a log file
                    Debug.WriteLine($"{santa.Email} - Error when sending mail");
                    Debug.WriteLine($"{santa.Email} - Exception: " + ex.InnerException?.Message ?? ex.Message);
                }
                finally
                {
                    await emailClient?.DisconnectAsync(true);
                }
            }
        }
    }
}
