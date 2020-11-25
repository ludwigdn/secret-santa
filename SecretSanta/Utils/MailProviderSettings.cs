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

namespace SecretSanta.Utils
{
    public class MailProviderSettings
    {
        public string SMTP_PORT { get; private set; }
        public string SMTP_HOST { get; private set; }

        public MailProviderSettings(string host, string port)
        {
            SMTP_HOST = host;
            SMTP_PORT = port;            
        }
        
        private MailProviderSettings() { }

        public static MailProviderSettings Get(MailProvider provider)
        {       
            string host;
            string port;

            switch(provider)
            {
                case MailProvider.ALICE:
                    host = "smtp.alice.fr";
                    port = "587";
                    break;

                case MailProvider.AOL:
                    host = "smtp.aol.com";
                    port = "465";
                    break;
                    
                case MailProvider.BOUYGUES:
                case MailProvider.BBOX:
                    host = "smtp.bbox.fr";
                    port = "587";
                    break;
                    
                case MailProvider.FREE:
                    host = "smtp.free.fr";
                    port = "465";
                    break;
                    
                case MailProvider.GMAIL:
                    host = "smtp.gmail.com";
                    port = "465";
                    break;                    
                    
                case MailProvider.HOTMAIL:
                    host = "smtp.live.com";
                    port = "587";
                    break;

                case MailProvider.LAPOSTE:
                    host = "smtp.laposte.net";
                    port = "465";
                    break;

                case MailProvider.NUMERICABLE:
                    host = "smtps.numericable.fr";
                    port = "587";
                    break;
                    
                case MailProvider.ORANGE:
                    host = "smtp.orange.fr";
                    port = "465";
                    break;
                    
                case MailProvider.OUTLOOK:
                    host = "SMTP.office365.com";
                    port = "587";
                    break;
                    
                case MailProvider.SFR:
                case MailProvider.NEUF:
                    host = "smtp.sfr.fr";
                    port = "465";
                    break;
                    
                case MailProvider.YAHOO:
                    host = "smtp.mail.yahoo.com";
                    port = "465";
                    break;
                    
                case MailProvider.ZOHO:
                    host = "smtp.zoho.com";
                    port = "465";
                    break;

                case MailProvider.UNKOWN:
                default:
                    return null;
            }

            return new MailProviderSettings
            {
                SMTP_HOST = host,
                SMTP_PORT = port
            };
        }
    }

    public enum MailProvider
    {
        ALICE,
        AOL,
        BOUYGUES,
        BBOX,
        FREE,
        GMAIL,
        HOTMAIL,
        LAPOSTE,
        NUMERICABLE,
        ORANGE,
        OUTLOOK,
        SFR,
        NEUF,
        YAHOO,
        ZOHO,
        UNKOWN
    }
}