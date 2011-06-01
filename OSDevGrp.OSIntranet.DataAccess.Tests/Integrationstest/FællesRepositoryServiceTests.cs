using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.DataAccess.Tests.Integrationstest
{
    /// <summary>
    /// Integrationstest af service for repository til fælles elementer.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class FællesRepositoryServiceTests
    {
        /// <summary>
        /// Tester, at brevhoveder hentes.
        /// </summary>
        [Test]
        public void TestAtBrevhovederHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFællesRepositoryService>("FællesRepositoryService");
            try
            {
                var query = new BrevhovedGetAllQuery();
                var brevhoveder = channel.BrevhovedGetAll(query);
                Assert.That(brevhoveder, Is.Not.Null);
                Assert.That(brevhoveder.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at et givent brevhoved kan hentes.
        /// </summary>
        [Test]
        public void TestAtBrevhovedHentes()
        {
            var random = new Random(DateTime.Now.Second);
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFællesRepositoryService>("FællesRepositoryService");
            try
            {
                var brevhovedGetAllQuery = new BrevhovedGetAllQuery();
                var brevhoveder = channel.BrevhovedGetAll(brevhovedGetAllQuery);
                Assert.That(brevhoveder, Is.Not.Null);
                Assert.That(brevhoveder.Count(), Is.GreaterThan(0));

                var no = random.Next(0, brevhoveder.Count() - 1);
                var query = new BrevhovedGetByNummerQuery
                                {
                                    Nummer = brevhoveder.ElementAt(no).Nummer
                                };
                var brevhoved = channel.BrevhovedGetByNummer(query);
                Assert.That(brevhoved, Is.Not.Null);
                Assert.That(brevhoved.Nummer, Is.EqualTo(brevhoveder.ElementAt(no).Nummer));
                Assert.That(brevhoved.Navn, Is.Not.Null);
                Assert.That(brevhoved.Navn, Is.EqualTo(brevhoveder.ElementAt(no).Navn));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }
    }
}
