using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SecretSanta;
using SecretSanta.Services;
using SecretSanta.Models;
using SecretSanta.Utils;

namespace SecretSanta.Tests
{
    [TestFixture, Parallelizable]
    public class Tests
    {
        private List<Participant> GetRandomizedList()
        {
            var list = new List<Participant>
            {
                new Participant { Name = "B.B. King", Email = "bbking@mail.com" },
                new Participant { Name = "Eric Clapton", Email = "ericclapton@mail.com" },
                new Participant { Name = "Jimmy Page", Email = "jimmypage@mail.com" },
                new Participant { Name = "David Gilmour", Email = "davidgilmour@mail.com" },
                new Participant { Name = "Slash", Email = "slash@mail.com" },
                new Participant { Name = "Brian May", Email = "brianmay@mail.com" },
                new Participant { Name = "Chuck Berry", Email = "chuckberry@mail.com" },
                new Participant { Name = "Buckethead", Email = "buckethead@mail.com" }
            };

            var randomizeService = new RandomizeService();
            return randomizeService.Randomize(list);
        }

        /// <summary>
        /// Ensure list is always set, and each participant is always set as well
        /// </summary>
        [Test, Parallelizable]
        public void IsSetTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                var result = GetRandomizedList();
                Assert.IsTrue(result.Any());
                Assert.IsTrue(result.All(o => o.IsSet));
            }
        }

        /// <summary>
        /// Ensure list is always well randomized whenn calling RandomizeService.Randomize()
        /// </summary>
        [Test, Parallelizable]
        public void IsActuallyRandomizedTest()
        {
            var prev = GetRandomizedList();
            for (int i = 0; i < 50; i++)
            {
                var current = GetRandomizedList();
                Assert.IsFalse(AreSame(prev, current));
                prev = current;
            }
        }


        private bool AreSame(List<Participant> left, List<Participant> right)
        {
            for (int i = 0 ; i < left.Count ; i++)
            {
                var first = left[i];
                var second = right[i];

                if (first.Name == second.Name && first.ReceiverName == second.ReceiverName)
                    continue;
                
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensure randomization is correct
        /// </summary>
        [Test, Parallelizable]
        public void RandomizationTest()
        {
            HashSet<string> firstParticipants = new HashSet<string>();
            bool isWellRandomized = false;

            for (int i = 0; i < 100000; i++)
            {
                firstParticipants.Add(GetRandomizedList()[0].Name);
                if (firstParticipants.Count > 1)
                {
                    isWellRandomized = true;
                    break;
                }
            }

            // After 100000 loops, it'd be pretty odd if the first participant of each loop is always the same...
            Assert.IsTrue(isWellRandomized);
        }

        [Test, Parallelizable]
        [TestCase(MailProvider.ALICE, "smtp.alice.fr", "587")]
        [TestCase(MailProvider.AOL, "smtp.aol.com", "465")]
        [TestCase(MailProvider.BOUYGUES, "smtp.bbox.fr", "587")]
        [TestCase(MailProvider.BBOX, "smtp.bbox.fr", "587")]
        [TestCase(MailProvider.FREE, "smtp.free.fr", "465")]
        [TestCase(MailProvider.GMAIL, "smtp.gmail.com", "465")]
        [TestCase(MailProvider.HOTMAIL, "smtp.live.com", "587")]
        [TestCase(MailProvider.LAPOSTE, "smtp.laposte.net", "465")]
        [TestCase(MailProvider.NUMERICABLE, "smtps.numericable.fr", "587")]
        [TestCase(MailProvider.ORANGE, "smtp.orange.fr", "465")]
        [TestCase(MailProvider.OUTLOOK, "SMTP.office365.com", "587")]
        [TestCase(MailProvider.SFR, "smtp.sfr.fr", "465")]
        [TestCase(MailProvider.NEUF, "smtp.sfr.fr", "465")]
        [TestCase(MailProvider.YAHOO, "smtp.mail.yahoo.com", "465")]
        [TestCase(MailProvider.ZOHO, "smtp.zoho.com", "465")]
        public void MailProviderSettingsTest(MailProvider provider, string host, string port)
        {
            var settings = MailProviderSettings.Get(provider);
            Assert.AreEqual(host, settings.SMTP_HOST);
            Assert.AreEqual(port, settings.SMTP_PORT);
        }
    }
}
