using SecretSanta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            if (santa.ReceiversList.Any())
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

            return string.Join(Environment.NewLine, new string[]{
              $"<h1>{_localeService.Get(LocaleService.AUTO_BODY_TITLE)}</h1>",
              bodyMessage,
            });
        }

        private string GetGiftIdeasFormatted(List<string> giftIdeas)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<br/>");
            stringBuilder.Append("<br/>");
            stringBuilder.Append("<ul>");
            foreach (var gift in giftIdeas.Where(o => !string.IsNullOrWhiteSpace(o)))
                stringBuilder.Append($"<li>{gift}</li>");
            stringBuilder.Append("</ul>");
            stringBuilder.Append("<br/>");
            stringBuilder.Append("<br/>");
            return stringBuilder.ToString();
        }
    }
}
