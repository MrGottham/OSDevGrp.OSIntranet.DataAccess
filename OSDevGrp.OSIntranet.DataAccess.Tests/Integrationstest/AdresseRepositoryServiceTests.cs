using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using NUnit.Framework;

namespace OSDevGrp.OSIntranet.DataAccess.Tests.Integrationstest
{
    /// <summary>
    /// Integrationstest af service for repository til adressekartoteket.
    /// </summary>
    [TestFixture]
    [Category("Integrationstest")]
    public class AdresseRepositoryServiceTests
    {
        /// <summary>
        /// Tester, at adressegrupper hentes.
        /// </summary>
        [Test]
        public void TestAtAdressegrupperHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new AdressegruppeGetAllQuery();
                var adressegrupper = channel.AdressegruppeGetAll(query);
                Assert.That(adressegrupper, Is.Not.Null);
                Assert.That(adressegrupper.Count, Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en given adressegruppe hentes.
        /// </summary>
        [Test]
        public void TestAtAdressegruppeHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new AdressegruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var adressegruppe = channel.AdressegruppeGetByNummer(query);
                Assert.That(adressegruppe, Is.Not.Null);
                Assert.That(adressegruppe.Nummer, Is.GreaterThan(0));
                Assert.That(adressegruppe.Navn, Is.Not.Null);
                Assert.That(adressegruppe.Navn.Length, Is.GreaterThan(0));
                Assert.That(adressegruppe.AdressegruppeOswebdb, Is.GreaterThanOrEqualTo(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisAdressegruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new AdressegruppeGetByNummerQuery
                                {
                                    Nummer = -1
                                };
                Assert.Throws<FaultException>(() => channel.AdressegruppeGetByNummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at betalingsbetingelser hentes.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelserHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new BetalingsbetingelseGetAllQuery();
                var betalingsbetingelser = channel.BetalingsbetingelseGetAll(query);
                Assert.That(betalingsbetingelser, Is.Not.Null);
                Assert.That(betalingsbetingelser.Count, Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en given betalingsbetingelse hentes.
        /// </summary>
        [Test]
        public void TestAtBetalingsbetingelseHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new BetalingsbetingelseGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var betalingsbetingelse = channel.BetalingsbetingelseGetByNummer(query);
                Assert.That(betalingsbetingelse, Is.Not.Null);
                Assert.That(betalingsbetingelse.Nummer, Is.GreaterThan(0));
                Assert.That(betalingsbetingelse.Navn, Is.Not.Null);
                Assert.That(betalingsbetingelse.Navn.Length, Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisBetalingsbetingelseIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new BetalingsbetingelseGetByNummerQuery
                                {
                                    Nummer = -1
                                };
                Assert.Throws<FaultException>(() => channel.BetalingsbetingelseGetByNummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }
    }
}
