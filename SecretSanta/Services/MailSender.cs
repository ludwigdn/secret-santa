using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using SecretSanta.Interfaces;
using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta
{
    public class MailSender : ImailSender
    {
        private readonly string SMTP_USERNAME;
        private readonly string SMTP_PASSWORD;
        private readonly string HOST;
        private readonly int PORT;

        public MailSender()
        {            
            SMTP_USERNAME = Config.Instance.SmtpEmail;
            SMTP_PASSWORD = Config.Instance.SmtpPassword;
            HOST = Config.Instance.SmtpHost;
            PORT = int.Parse(Config.Instance.SmtpPort);
        }

        public async Task Send(List<Participant> participants)
        {
            var tasks = participants.Select(Send);
            await Task.WhenAll(tasks);
        }

        private async Task Send(Participant santa)
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
                msg.Body = new TextPart(TextFormat.Html) { Text = GetHtmlBody(santa) };

                try
                {
                    await emailClient.SendAsync(msg);
                    Debug.WriteLine($"{santa.Email} - Sent mail ");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{santa.Email} - Error when sending mail");
                    Debug.WriteLine($"{santa.Email} - Exception: " + ex.InnerException?.Message ?? ex.Message);
                }
                finally
                {
                    await emailClient?.DisconnectAsync(true);
                }
            }
        }

        private string GetHtmlBody(Participant santa)
        {
            string _htmlBody = "<!DOCTYPE html>"
            + "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">"
            + "<head>"
            + "<meta charset=\"utf-8\" />"
            + "<title></title>"
            + "</head>"
            + "<body>"
            + "<div style=\"height:100%; width:100%; background-color: #5a992f; position: absolute; top: 0; left: 0;\">"
            + "<div style=\"padding: 30px; font-family: Arial; color: white;\">"
            + "<div style=\"text-align: center;\">"
            + "<h1>{0}</h1>"
            + "<div style=\"display: inline-flex; margin-bottom: 15px;\">"
            + "<span style=\"opacity: 60%; background-color: red; height: 30px; width: 30px; border-radius: 50%; margin: 0 20px; border: 0px black solid;\"></span>"
            + "<span style=\"opacity: 60%; background-color: green; height: 30px; width: 30px; border-radius: 50%; margin: 0 20px; border: 0px black solid;\"></span>"
            + "<span style=\"opacity: 60%; background-color: blue; height: 30px; width: 30px; border-radius: 50%; margin: 0 20px; border: 0px black solid;\"></span>"
            + "<span style=\"opacity: 60%; background-color: yellow; height: 30px; width: 30px; border-radius: 50%; margin: 0 20px; border: 0px black solid;\"></span>"
            + "<span style=\"opacity: 60%; background-color: blue; height: 30px; width: 30px; border-radius: 50%; margin: 0 20px; border: 0px black solid;\"></span>"
            + "<span style=\"opacity: 60%; background-color: green; height: 30px; width: 30px; border-radius: 50%; margin: 0 20px; border: 0px black solid;\"></span>"
            + "<span style=\"opacity: 60%; background-color: red; height: 30px; width: 30px; border-radius: 50%; margin: 0 20px; border: 0px black solid;\"></span>"               
            + "</div>"
            + "</div>"
            + "<p>{1}</p>"
            + "</div>"
            + "</div>"
            + "</body>"
            + "</html>";

            return string.Format(_htmlBody, 
                Config.Instance.MailBodyTitle, 
                string.Format(Config.Instance.MailBody, santa.ReceiverName));
        }
    }
}
