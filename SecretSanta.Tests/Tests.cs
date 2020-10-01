using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SecretSanta;
using SecretSanta.Models;

namespace SecretSanta.Tests
{
    [TestFixture, Parallelizable]
    public class Tests
    {
        private List<Participant> GetRandomizedList()
        {
            var list = new List<Participant>
                {
                    new Participant { Name = "B.B. King" },
                    new Participant { Name = "Eric Clapton" },
                    new Participant { Name = "Jimmy Page" },
                    new Participant { Name = "David Gilmour" },
                    new Participant { Name = "Slash" },
                    new Participant { Name = "Brian May" },
                    new Participant { Name = "Chuck Berry" },
                    new Participant { Name = "Buckethead" }
                };

            var randomizer = new Randomizer();
            return randomizer.Randomize(list);
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
    }
}
