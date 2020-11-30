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

namespace SecretSanta.Services
{
    public class MailMessageFormatter
    {
        private LocaleService _localeService;

        public MailMessageFormatter(LocaleService localeService)
        {
            _localeService = localeService;
        }

        public string GetHtmlBody(Participant santa)
        {
            string bodyMessage;
            
            if (santa.HasGiftIdeas())
            {
                var body = _localeService.Get(LocaleService.AUTO_BODY_WITH_GIFTS);
                var giftsHtml = GetGiftIdeasFormatted(santa.ReceiversList);
                bodyMessage = string.Format(body, santa.Name, santa.ReceiversName, giftsHtml);
            }
            else
            {
                var body = _localeService.Get(LocaleService.AUTO_BODY);
                bodyMessage = string.Format(body, santa.Name, santa.ReceiversName);
            }

            return "<!DOCTYPE html>"
            + "<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">"
            + "<head>"
            + "<meta charset=\"utf-8\" />"
            + "<title></title>"
            + "</head>"
            + "<body>"
            + "<div style=\"height:100%; width:100%; background-color: #5a992f; position: absolute; top: 0; left: 0;\">"
            + "<div style=\"padding: 30px; font-family: Arial; color: white;\">"
            + "<div style=\"text-align: center;\">"
            + $"<h1>{_localeService.Get(LocaleService.AUTO_BODY_TITLE)}</h1>"
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
            + $"<p>{bodyMessage}</p>"
            + "</div>"
            + "</div>"
            + "</body>"
            + "</html>";
        }

        private string GetGiftIdeasFormatted(List<string> giftIdeas)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<ul>");

            foreach (var gift in giftIdeas.Where(o => !string.IsNullOrWhiteSpace(o)))
                stringBuilder.AppendLine($"<li>{gift}</li>");
            
            stringBuilder.AppendLine("</ul>");

            return stringBuilder.ToString();
        }
    }
}
