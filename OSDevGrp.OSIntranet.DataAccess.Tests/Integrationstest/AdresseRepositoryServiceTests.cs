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
        /// Tester, at adresseliste hentes.
        /// </summary>
        [Test]
        public void TestAtAdresselisteHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new AdresselisteGetAllQuery();
                var adresser = channel.AdresselisteGetAll(query);
                Assert.That(adresser, Is.Not.Null);
                Assert.That(adresser.Count, Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at postnumre hentes.
        /// </summary>
        [Test]
        public void TestAtPostnumreHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PostnummerGetAllQuery();
                var postnumre = channel.PostnummerGetAll(query);
                Assert.That(postnumre, Is.Not.Null);
                Assert.That(postnumre.Count, Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at postnumre for en landekode hentes.
        /// </summary>
        [Test]
        public void TestAtPostnumreForLandekodeHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PostnummerGetByLandekodeQuery
                                {
                                    Landekode = "DK"
                                };
                var postnumre = channel.PostnummerGetAllByLandekode(query);
                Assert.That(postnumre, Is.Not.Null);
                Assert.That(postnumre.Count, Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at et bynavn for et postnummer på en given landekode hentes.
        /// </summary>
        [Test]
        public void TestAtBynavnForPostnummerHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new BynavnGetByLandekodeAndPostnummerQuery
                                {
                                    Landekode = "DK",
                                    Postnummer = "5700"
                                };
                var postnummer = channel.BynavnGetByLandekodeAndPostnummre(query);
                Assert.That(postnummer, Is.Not.Null);
                Assert.That(postnummer.Landekode, Is.Not.Null);
                Assert.That(postnummer.Landekode.Length, Is.GreaterThan(0));
                Assert.That(postnummer.Postnummer, Is.Not.Null);
                Assert.That(postnummer.Postnummer.Length, Is.GreaterThan(0));
                Assert.That(postnummer.Bynavn, Is.Not.Null);
                Assert.That(postnummer.Bynavn.Length, Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException, hvis postnumemr ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisPostnummerIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new BynavnGetByLandekodeAndPostnummerQuery
                                {
                                    Landekode = "DK",
                                    Postnummer = "XYZ"
                                };
                Assert.Throws<FaultException>(() => channel.BynavnGetByLandekodeAndPostnummre(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

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
