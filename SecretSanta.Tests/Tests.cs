using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SecretSanta.Services;
using SecretSanta.Models;
using SecretSanta.Utils;

namespace SecretSanta.Tests
{
    [TestFixture, Parallelizable]
    public class Tests
    {
        private List<Participant> GetRandomizedList(Random rand)
        {
            var list = new List<Participant>
            {
                new Participant { Name = "B.B. King", Email = "bbking@mail.com", Partner  = "Slash" },
                new Participant { Name = "Eric Clapton", Email = "ericclapton@mail.com", Partner = "Jimmy Page" },
                new Participant { Name = "Jimmy Page", Email = "jimmypage@mail.com" , Partner = "Eric Clapton" },
                new Participant { Name = "David Gilmour", Email = "davidgilmour@mail.com" , Partner = "Brian May" },
                new Participant { Name = "Slash", Email = "slash@mail.com" , Partner = "B.B. King" },
                new Participant { Name = "Brian May", Email = "brianmay@mail.com" , Partner = "David Gilmour" },
                new Participant { Name = "Chuck Berry", Email = "chuckberry@mail.com" , Partner = "Buckethead" },
                new Participant { Name = "Buckethead", Email = "buckethead@mail.com" , Partner = "Chuck Berry" }
            };

            var randomizeService = new RandomizeService(rand);
            return randomizeService.Randomize(list);
        }

        /// <summary>
        /// Ensure list is always set, and each participant is always set as well
        /// </summary>
        [Test, Parallelizable]
        public void IsSetTest()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 10000; i++)
            {
                var result = GetRandomizedList(rand);
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
            bool AreDifferentAndNotPartners(List<Participant> left, List<Participant> right)
            {
                for (int i = 0 ; i < left.Count ; i++)
                {
                    if (left[i].Name == right[i].Name && left[i].ReceiversName == right[i].ReceiversName)
                    {
                        return false;
                    }
                    if (left[i].Name == right[i].Partner || right[i].Name == left[i].Partner)
                    {
                        return false;
                    }


                }
                return true;
            }

            List<bool> randomizedStates = new List<bool>();
            List<bool> everyonePickedOnlyOnceStates = new List<bool>();
            var rand = new Random(DateTime.Now.Millisecond);

            var prev = GetRandomizedList(rand);
            for (int i = 0; i < 10000; i++)
            {
                var current = GetRandomizedList(rand);
                randomizedStates.Add(AreDifferentAndNotPartners(prev, current));
                everyonePickedOnlyOnceStates.Add(RandomizeService.EnsureEveryonePickedOnlyOnce(current));
                prev = current;
            }

            Assert.IsFalse(randomizedStates.All(o => !o));
            Assert.IsTrue(everyonePickedOnlyOnceStates.All(o => o));
        }

        /// <summary>
        /// Ensure randomization is correct
        /// </summary>
        [Test, Parallelizable]
        public void RandomizationTest()
        {
            var rand = new Random(DateTime.Now.Millisecond);
            HashSet<string> firstParticipants = new HashSet<string>();
            bool isWellRandomized = false;

            for (int i = 0; i < 100000; i++)
            {
                firstParticipants.Add(GetRandomizedList(rand)[0].Name);

                if (firstParticipants.Count > 1)
                {
                    // As soon as we reach at least two different participants, we can say the randomize process works
                    isWellRandomized = true;
                    break;
                }
            }

            // With a maximum of 100000 loops, it'd be pretty odd if the first participant of each loop is always the same...
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
