using System;
using System.Linq;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
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
        /// Tester, at personer hentes.
        /// </summary>
        [Test]
        public void TestAtPersonerHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PersonGetAllQuery();
                var personer = channel.PersonGetAll(query);
                Assert.That(personer, Is.Not.Null);
                Assert.That(personer.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en given person hentes.
        /// </summary>
        [Test]
        public void TestAtPersonHentes()
        {
            var random = new Random(DateTime.Now.Second);
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var queryGetAll = new PersonGetAllQuery();
                var personer = channel.PersonGetAll(queryGetAll);
                Assert.That(personer, Is.Not.Null);
                Assert.That(personer.Count(), Is.GreaterThan(0));

                var query = new PersonGetByNummerQuery
                                {
                                    Nummer = personer.ElementAt(random.Next(0, personer.Count() - 1)).Nummer
                                };
                var person = channel.PersonGetByNummer(query);
                Assert.That(person, Is.Not.Null);
                Assert.That(person.Nummer == query.Nummer);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException, hvis personen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisPersonIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PersonGetByNummerQuery
                                {
                                    Nummer = -1
                                };
                Assert.Throws<FaultException>(() => channel.PersonGetByNummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at person oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af person er testet.")]
        public void TestAtPersonOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PersonAddCommand
                                  {
                                      Navn = "_Navn",
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon = "_TELEFON",
                                      Mobil = "_MÒBIL",
                                      Fødselsdato = DateTime.Now.AddYears(-7).AddDays(-7),
                                      Adressegruppe = 1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = 1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true,
                                      Firma = 48
                                  };
                var result = channel.PersonAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.GreaterThan(0));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.Adresse1, Is.EqualTo(command.Adresse1));
                Assert.That(result.Adresse2, Is.EqualTo(command.Adresse2));
                Assert.That(result.PostnummerBy, Is.EqualTo(command.PostnummerBy));
                Assert.That(result.Telefon, Is.EqualTo(command.Telefon));
                Assert.That(result.Mobil, Is.EqualTo(command.Mobil));
                Assert.That(result.Fødselsdato, Is.EqualTo(command.Fødselsdato).Within(1).Days);
                Assert.That(result.Adressegruppe, Is.Not.Null);
                Assert.That(result.Adressegruppe.Nummer, Is.EqualTo(command.Adressegruppe));
                Assert.That(result.Adressegruppe.Nummer, Is.EqualTo(command.Adressegruppe));
                Assert.That(result.Bekendtskab, Is.EqualTo(command.Bekendtskab));
                Assert.That(result.Mailadresse, Is.EqualTo(command.Mailadresse));
                Assert.That(result.Webadresse, Is.EqualTo(command.Webadresse));
                Assert.That(result.Betalingsbetingelse, Is.Not.Null);
                Assert.That(result.Betalingsbetingelse.Nummer, Is.EqualTo(command.Betalingsbetingelse));
                Assert.That(result.Udlånsfrist, Is.EqualTo(command.Udlånsfrist));
                Assert.That(result.FilofaxAdresselabel, Is.EqualTo(command.FilofaxAdresselabel));
                Assert.That(result.Firma, Is.Not.Null);
                Assert.That(result.Firma.Nummer, Is.EqualTo(command.Firma));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfPersonHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PersonAddCommand
                                  {
                                      Navn = string.Empty,
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon = "_Telefon",
                                      Mobil = "_Mobil",
                                      Fødselsdato = DateTime.Now.AddYears(-7).AddDays(-7),
                                      Adressegruppe = 1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = 1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true,
                                      Firma = 48
                                  };
                Assert.Throws<FaultException>(() => channel.PersonAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfPersonHvisAdressegruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PersonAddCommand
                                  {
                                      Navn = "_Navn",
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon = "_Telefon",
                                      Mobil = "_Mobil",
                                      Fødselsdato = DateTime.Now.AddYears(-7).AddDays(-7),
                                      Adressegruppe = -1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = 1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true,
                                      Firma = 48
                                  };
                Assert.Throws<FaultException>(() => channel.PersonAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfPersonHvisBetalingsbetingelseIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PersonAddCommand
                                  {
                                      Navn = "_Navn",
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon = "_Telefon",
                                      Mobil = "_Mobil",
                                      Fødselsdato = DateTime.Now.AddYears(-7).AddDays(-7),
                                      Adressegruppe = 1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = -1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true,
                                      Firma = 48
                                  };
                Assert.Throws<FaultException>(() => channel.PersonAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis firmaet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfPersonHvisFirmaIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PersonAddCommand
                                  {
                                      Navn = "_Navn",
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon = "_Telefon",
                                      Mobil = "_Mobil",
                                      Fødselsdato = DateTime.Now.AddYears(-7).AddDays(-7),
                                      Adressegruppe = 1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = 1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true,
                                      Firma = -1
                                  };
                Assert.Throws<FaultException>(() => channel.PersonAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at person opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af personer er testet.")]
        public void TestAtPersonOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PersonGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var person = channel.PersonGetByNummer(query);
                Assert.That(person, Is.Not.Null);
                Assert.That(person.Nummer, Is.EqualTo(query.Nummer));

                var command = new PersonModifyCommand
                                  {
                                      Nummer = person.Nummer,
                                      Navn = string.Format("_{0}", person.Navn),
                                      Adresse1 = person.Adresse1,
                                      Adresse2 = person.Adresse2,
                                      PostnummerBy = person.PostnummerBy,
                                      Telefon = person.Telefon,
                                      Mobil = person.Mobil,
                                      Fødselsdato = person.Fødselsdato,
                                      Adressegruppe = person.Adressegruppe.Nummer,
                                      Bekendtskab = person.Bekendtskab,
                                      Mailadresse = person.Mailadresse,
                                      Webadresse = person.Webadresse,
                                      Betalingsbetingelse = person.Betalingsbetingelse == null
                                                                ? 0
                                                                : person.Betalingsbetingelse.Nummer,
                                      Udlånsfrist = person.Udlånsfrist,
                                      FilofaxAdresselabel = person.FilofaxAdresselabel,
                                      Firma = person.Firma == null ? 0 : person.Firma.Nummer
                                  };
                var result = channel.PersonModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.Adresse1, Is.EqualTo(command.Adresse1));
                Assert.That(result.Adresse2, Is.EqualTo(command.Adresse2));
                Assert.That(result.PostnummerBy, Is.EqualTo(command.PostnummerBy));
                Assert.That(result.Telefon, Is.EqualTo(command.Telefon));
                Assert.That(result.Mobil, Is.EqualTo(command.Mobil));
                Assert.That(result.Fødselsdato, Is.EqualTo(command.Fødselsdato).Within(1).Days);
                Assert.That(result.Adressegruppe, Is.Not.Null);
                Assert.That(result.Adressegruppe.Nummer, Is.EqualTo(command.Adressegruppe));
                Assert.That(result.Adressegruppe.Nummer, Is.EqualTo(command.Adressegruppe));
                Assert.That(result.Bekendtskab, Is.EqualTo(command.Bekendtskab));
                Assert.That(result.Mailadresse, Is.EqualTo(command.Mailadresse));
                Assert.That(result.Webadresse, Is.EqualTo(command.Webadresse));
                Assert.That(result.Betalingsbetingelse, Is.Not.Null);
                Assert.That(result.Betalingsbetingelse.Nummer, Is.EqualTo(command.Betalingsbetingelse));
                Assert.That(result.Udlånsfrist, Is.EqualTo(command.Udlånsfrist));
                Assert.That(result.FilofaxAdresselabel, Is.EqualTo(command.FilofaxAdresselabel));
                Assert.That(result.Firma, Is.Not.Null);
                Assert.That(result.Firma.Nummer, Is.EqualTo(command.Firma));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis navnet er tomt
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfPersonHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PersonGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var person = channel.PersonGetByNummer(query);
                Assert.That(person, Is.Not.Null);
                Assert.That(person.Nummer, Is.EqualTo(query.Nummer));

                var command = new PersonModifyCommand
                                  {
                                      Nummer = person.Nummer,
                                      Navn = string.Empty,
                                      Adresse1 = person.Adresse1,
                                      Adresse2 = person.Adresse2,
                                      PostnummerBy = person.PostnummerBy,
                                      Telefon = person.Telefon,
                                      Mobil = person.Mobil,
                                      Fødselsdato = person.Fødselsdato,
                                      Adressegruppe = person.Adressegruppe.Nummer,
                                      Bekendtskab = person.Bekendtskab,
                                      Mailadresse = person.Mailadresse,
                                      Webadresse = person.Webadresse,
                                      Betalingsbetingelse = person.Betalingsbetingelse == null
                                                                ? 0
                                                                : person.Betalingsbetingelse.Nummer,
                                      Udlånsfrist = person.Udlånsfrist,
                                      FilofaxAdresselabel = person.FilofaxAdresselabel,
                                      Firma = person.Firma == null ? 0 : person.Firma.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.PersonModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfPersonHvisAdressegruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PersonGetByNummerQuery
                {
                    Nummer = 1
                };
                var person = channel.PersonGetByNummer(query);
                Assert.That(person, Is.Not.Null);
                Assert.That(person.Nummer, Is.EqualTo(query.Nummer));

                var command = new PersonModifyCommand
                                  {
                                      Nummer = person.Nummer,
                                      Navn = person.Navn,
                                      Adresse1 = person.Adresse1,
                                      Adresse2 = person.Adresse2,
                                      PostnummerBy = person.PostnummerBy,
                                      Telefon = person.Telefon,
                                      Mobil = person.Mobil,
                                      Fødselsdato = person.Fødselsdato,
                                      Adressegruppe = -1,
                                      Bekendtskab = person.Bekendtskab,
                                      Mailadresse = person.Mailadresse,
                                      Webadresse = person.Webadresse,
                                      Betalingsbetingelse = person.Betalingsbetingelse == null
                                                                ? 0
                                                                : person.Betalingsbetingelse.Nummer,
                                      Udlånsfrist = person.Udlånsfrist,
                                      FilofaxAdresselabel = person.FilofaxAdresselabel,
                                      Firma = person.Firma == null ? 0 : person.Firma.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.PersonModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfPersonHvisBetalingsbetingelseIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PersonGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var person = channel.PersonGetByNummer(query);
                Assert.That(person, Is.Not.Null);
                Assert.That(person.Nummer, Is.EqualTo(query.Nummer));

                var command = new PersonModifyCommand
                                  {
                                      Nummer = person.Nummer,
                                      Navn = person.Navn,
                                      Adresse1 = person.Adresse1,
                                      Adresse2 = person.Adresse2,
                                      PostnummerBy = person.PostnummerBy,
                                      Telefon = person.Telefon,
                                      Mobil = person.Mobil,
                                      Fødselsdato = person.Fødselsdato,
                                      Adressegruppe = person.Adressegruppe.Nummer,
                                      Bekendtskab = person.Bekendtskab,
                                      Mailadresse = person.Mailadresse,
                                      Webadresse = person.Webadresse,
                                      Betalingsbetingelse = -1,
                                      Udlånsfrist = person.Udlånsfrist,
                                      FilofaxAdresselabel = person.FilofaxAdresselabel,
                                      Firma = person.Firma == null ? 0 : person.Firma.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.PersonModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis firmaet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfPersonHvisFirmaIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PersonGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var person = channel.PersonGetByNummer(query);
                Assert.That(person, Is.Not.Null);
                Assert.That(person.Nummer, Is.EqualTo(query.Nummer));

                var command = new PersonModifyCommand
                                  {
                                      Nummer = person.Nummer,
                                      Navn = person.Navn,
                                      Adresse1 = person.Adresse1,
                                      Adresse2 = person.Adresse2,
                                      PostnummerBy = person.PostnummerBy,
                                      Telefon = person.Telefon,
                                      Mobil = person.Mobil,
                                      Fødselsdato = person.Fødselsdato,
                                      Adressegruppe = person.Adressegruppe.Nummer,
                                      Bekendtskab = person.Bekendtskab,
                                      Mailadresse = person.Mailadresse,
                                      Webadresse = person.Webadresse,
                                      Betalingsbetingelse = person.Betalingsbetingelse == null
                                                                ? 0
                                                                : person.Betalingsbetingelse.Nummer,
                                      Udlånsfrist = person.Udlånsfrist,
                                      FilofaxAdresselabel = person.FilofaxAdresselabel,
                                      Firma = -1
                                  };
                Assert.Throws<FaultException>(() => channel.PersonModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at firmaer hentes.
        /// </summary>
        [Test]
        public void TestAtFirmaerHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new FirmaGetAllQuery();
                var firmaer = channel.FirmaGetAll(query);
                Assert.That(firmaer, Is.Not.Null);
                Assert.That(firmaer.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at et givent firma hentes.
        /// </summary>
        [Test]
        public void TestAtFirmaHentes()
        {
            var random = new Random(DateTime.Now.Second);
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var queryGetAll = new FirmaGetAllQuery();
                var firmaer = channel.FirmaGetAll(queryGetAll);
                Assert.That(firmaer, Is.Not.Null);
                Assert.That(firmaer.Count(), Is.GreaterThan(0));

                var query = new FirmaGetByNummerQuery
                                {
                                    Nummer = firmaer.ElementAt(random.Next(0, firmaer.Count() - 1)).Nummer
                                };
                var firma = channel.FirmaGetByNummer(query);
                Assert.That(firma, Is.Not.Null);
                Assert.That(firma.Nummer == query.Nummer);
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException, hvis firmaet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisFirmaIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new FirmaGetByNummerQuery
                                {
                                    Nummer = -1
                                };
                Assert.Throws<FaultException>(() => channel.FirmaGetByNummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis navnet er tomt.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af firma er testet.")]
        public void TestAtFirmaOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new FirmaAddCommand
                                  {
                                      Navn = "_Navn",
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon1 = "_TELEFON 1",
                                      Telefon2 = "_TELEFON 2",
                                      Telefax = "_TELEFAX",
                                      Adressegruppe = 1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = 1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true
                                  };
                var result = channel.FirmaAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.GreaterThan(0));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.Adresse1, Is.EqualTo(command.Adresse1));
                Assert.That(result.Adresse2, Is.EqualTo(command.Adresse2));
                Assert.That(result.PostnummerBy, Is.EqualTo(command.PostnummerBy));
                Assert.That(result.Telefon1, Is.EqualTo(command.Telefon1));
                Assert.That(result.Telefon2, Is.EqualTo(command.Telefon2));
                Assert.That(result.Telefax, Is.EqualTo(command.Telefax));
                Assert.That(result.Adressegruppe, Is.Not.Null);
                Assert.That(result.Adressegruppe.Nummer, Is.EqualTo(command.Adressegruppe));
                Assert.That(result.Adressegruppe.Nummer, Is.EqualTo(command.Adressegruppe));
                Assert.That(result.Bekendtskab, Is.EqualTo(command.Bekendtskab));
                Assert.That(result.Mailadresse, Is.EqualTo(command.Mailadresse));
                Assert.That(result.Webadresse, Is.EqualTo(command.Webadresse));
                Assert.That(result.Betalingsbetingelse, Is.Not.Null);
                Assert.That(result.Betalingsbetingelse.Nummer, Is.EqualTo(command.Betalingsbetingelse));
                Assert.That(result.Udlånsfrist, Is.EqualTo(command.Udlånsfrist));
                Assert.That(result.FilofaxAdresselabel, Is.EqualTo(command.FilofaxAdresselabel));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfFirmaHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new FirmaAddCommand
                                  {
                                      Navn = string.Empty,
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon1 = "_Telefon 1",
                                      Telefon2 = "_Telefon 2",
                                      Telefax = "_Telefax",
                                      Adressegruppe = 1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = 1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true
                                  };
                Assert.Throws<FaultException>(() => channel.FirmaAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfFirmaHvisAdressegruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new FirmaAddCommand
                                  {
                                      Navn = "_Navn",
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon1 = "_Telefon 1",
                                      Telefon2 = "_Telefon 2",
                                      Telefax = "_Telefax",
                                      Adressegruppe = -1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = 1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true
                                  };
                Assert.Throws<FaultException>(() => channel.FirmaAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfFirmaHvisBetalingsbetingelseIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new FirmaAddCommand
                                  {
                                      Navn = "_Navn",
                                      Adresse1 = "_Adresse 1",
                                      Adresse2 = "_Adresse 2",
                                      PostnummerBy = "_Postnummer og bynavn",
                                      Telefon1 = "_Telefon 1",
                                      Telefon2 = "_Telefon 2",
                                      Telefax = "_Telefax",
                                      Adressegruppe = 1,
                                      Bekendtskab = "_Bekendtskab",
                                      Mailadresse = "_Mailadresse",
                                      Webadresse = "_Webadresse",
                                      Betalingsbetingelse = -1,
                                      Udlånsfrist = 7,
                                      FilofaxAdresselabel = true
                                  };
                Assert.Throws<FaultException>(() => channel.FirmaAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at firma opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af firmaer er testet.")]
        public void TestAtFirmaOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new FirmaGetByNummerQuery
                                {
                                    Nummer = 48
                                };
                var firma = channel.FirmaGetByNummer(query);
                Assert.That(firma, Is.Not.Null);
                Assert.That(firma.Nummer, Is.EqualTo(query.Nummer));

                var command = new FirmaModifyCommand
                                  {
                                      Nummer = firma.Nummer,
                                      Navn = string.Format("_{0}", firma.Navn),
                                      Adresse1 = firma.Adresse1,
                                      Adresse2 = firma.Adresse2,
                                      PostnummerBy = firma.PostnummerBy,
                                      Telefon1 = firma.Telefon1,
                                      Telefon2 = firma.Telefon2,
                                      Telefax = firma.Telefax,
                                      Adressegruppe = firma.Adressegruppe.Nummer,
                                      Bekendtskab = firma.Bekendtskab,
                                      Mailadresse = firma.Mailadresse,
                                      Webadresse = firma.Webadresse,
                                      Betalingsbetingelse = firma.Betalingsbetingelse == null
                                                                ? 0
                                                                : firma.Betalingsbetingelse.Nummer,
                                      Udlånsfrist = firma.Udlånsfrist,
                                      FilofaxAdresselabel = firma.FilofaxAdresselabel
                                  };
                var result = channel.FirmaModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.GreaterThan(0));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.Adresse1, Is.EqualTo(command.Adresse1));
                Assert.That(result.Adresse2, Is.EqualTo(command.Adresse2));
                Assert.That(result.PostnummerBy, Is.EqualTo(command.PostnummerBy));
                Assert.That(result.Telefon1, Is.EqualTo(command.Telefon1));
                Assert.That(result.Telefon2, Is.EqualTo(command.Telefon2));
                Assert.That(result.Telefax, Is.EqualTo(command.Telefax));
                Assert.That(result.Adressegruppe, Is.Not.Null);
                Assert.That(result.Adressegruppe.Nummer, Is.EqualTo(command.Adressegruppe));
                Assert.That(result.Adressegruppe.Nummer, Is.EqualTo(command.Adressegruppe));
                Assert.That(result.Bekendtskab, Is.EqualTo(command.Bekendtskab));
                Assert.That(result.Mailadresse, Is.EqualTo(command.Mailadresse));
                Assert.That(result.Webadresse, Is.EqualTo(command.Webadresse));
                Assert.That(result.Betalingsbetingelse, Is.Not.Null);
                Assert.That(result.Betalingsbetingelse.Nummer, Is.EqualTo(command.Betalingsbetingelse));
                Assert.That(result.Udlånsfrist, Is.EqualTo(command.Udlånsfrist));
                Assert.That(result.FilofaxAdresselabel, Is.EqualTo(command.FilofaxAdresselabel));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }
        
        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfFirmaHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new FirmaGetByNummerQuery
                                {
                                    Nummer = 48
                                };
                var firma = channel.FirmaGetByNummer(query);
                Assert.That(firma, Is.Not.Null);
                Assert.That(firma.Nummer, Is.EqualTo(query.Nummer));

                var command = new FirmaModifyCommand
                                  {
                                      Nummer = firma.Nummer,
                                      Navn = string.Empty,
                                      Adresse1 = firma.Adresse1,
                                      Adresse2 = firma.Adresse2,
                                      PostnummerBy = firma.PostnummerBy,
                                      Telefon1 = firma.Telefon1,
                                      Telefon2 = firma.Telefon2,
                                      Telefax = firma.Telefax,
                                      Adressegruppe = firma.Adressegruppe.Nummer,
                                      Bekendtskab = firma.Bekendtskab,
                                      Mailadresse = firma.Mailadresse,
                                      Webadresse = firma.Webadresse,
                                      Betalingsbetingelse = firma.Betalingsbetingelse == null
                                                                ? 0
                                                                : firma.Betalingsbetingelse.Nummer,
                                      Udlånsfrist = firma.Udlånsfrist,
                                      FilofaxAdresselabel = firma.FilofaxAdresselabel
                                  };
                Assert.Throws<FaultException>(() => channel.FirmaModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis adressegruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfFirmaHvisAdressegruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new FirmaGetByNummerQuery
                                {
                                    Nummer = 48
                                };
                var firma = channel.FirmaGetByNummer(query);
                Assert.That(firma, Is.Not.Null);
                Assert.That(firma.Nummer, Is.EqualTo(query.Nummer));

                var command = new FirmaModifyCommand
                                  {
                                      Nummer = firma.Nummer,
                                      Navn = firma.Navn,
                                      Adresse1 = firma.Adresse1,
                                      Adresse2 = firma.Adresse2,
                                      PostnummerBy = firma.PostnummerBy,
                                      Telefon1 = firma.Telefon1,
                                      Telefon2 = firma.Telefon2,
                                      Telefax = firma.Telefax,
                                      Adressegruppe = -1,
                                      Bekendtskab = firma.Bekendtskab,
                                      Mailadresse = firma.Mailadresse,
                                      Webadresse = firma.Webadresse,
                                      Betalingsbetingelse = firma.Betalingsbetingelse == null
                                                                ? 0
                                                                : firma.Betalingsbetingelse.Nummer,
                                      Udlånsfrist = firma.Udlånsfrist,
                                      FilofaxAdresselabel = firma.FilofaxAdresselabel
                                  };
                Assert.Throws<FaultException>(() => channel.FirmaModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis betalingsbetingelsen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfFirmaHvisBetalingsbetingelseIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new FirmaGetByNummerQuery
                                {
                                    Nummer = 48
                                };
                var firma = channel.FirmaGetByNummer(query);
                Assert.That(firma, Is.Not.Null);
                Assert.That(firma.Nummer, Is.EqualTo(query.Nummer));

                var command = new FirmaModifyCommand
                                  {
                                      Nummer = firma.Nummer,
                                      Navn = firma.Navn,
                                      Adresse1 = firma.Adresse1,
                                      Adresse2 = firma.Adresse2,
                                      PostnummerBy = firma.PostnummerBy,
                                      Telefon1 = firma.Telefon1,
                                      Telefon2 = firma.Telefon2,
                                      Telefax = firma.Telefax,
                                      Adressegruppe = firma.Adressegruppe.Nummer,
                                      Bekendtskab = firma.Bekendtskab,
                                      Mailadresse = firma.Mailadresse,
                                      Webadresse = firma.Webadresse,
                                      Betalingsbetingelse = -1,
                                      Udlånsfrist = firma.Udlånsfrist,
                                      FilofaxAdresselabel = firma.FilofaxAdresselabel
                                  };
                Assert.Throws<FaultException>(() => channel.FirmaModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

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
                Assert.That(adresser.Count(), Is.GreaterThan(0));
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
                Assert.That(postnumre.Count(), Is.GreaterThan(0));
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
                Assert.That(postnumre.Count(), Is.GreaterThan(0));
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
        /// Tester, at postnummer oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af postnummer er testet.")]
        public void TestAtPostnummerOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PostnummerAddCommand
                                  {
                                      Landekode = "DK",
                                      Postnummer = "_TEST",
                                      Bynavn = "_Test"
                                  };
                var result = channel.PostnummerAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Landekode, Is.Not.Null);
                Assert.That(result.Landekode, Is.EqualTo(command.Landekode));
                Assert.That(result.Postnummer, Is.Not.Null);
                Assert.That(result.Postnummer, Is.EqualTo(command.Postnummer));
                Assert.That(result.Bynavn, Is.Not.Null);
                Assert.That(result.Bynavn, Is.EqualTo(command.Bynavn));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis postnummer allerede findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisPostnummerAlleredeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PostnummerAddCommand
                                  {
                                      Landekode = "DK",
                                      Postnummer = "5700",
                                      Bynavn = "_Test"
                                  };
                Assert.Throws<FaultException>(() => channel.PostnummerAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis landekoden er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisLandekodePåPostnummerErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PostnummerAddCommand
                                  {
                                      Landekode = string.Empty,
                                      Postnummer = "_Test",
                                      Bynavn = "_Test"
                                  };
                Assert.Throws<FaultException>(() => channel.PostnummerAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis postnummeret er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisPostnummerPåPostnummerErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PostnummerAddCommand
                                  {
                                      Landekode = "DK",
                                      Postnummer = string.Empty,
                                      Bynavn = "_Test"
                                  };
                Assert.Throws<FaultException>(() => channel.PostnummerAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis bynavnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisBynavnPåPostnummerErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new PostnummerAddCommand
                                  {
                                      Landekode = "DK",
                                      Postnummer = "_Test",
                                      Bynavn = string.Empty
                                  };
                Assert.Throws<FaultException>(() => channel.PostnummerAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at postnummer opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af postnumre er testet.")]
        public void TestAtPostnummerOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PostnummerGetAllQuery();
                var postnumre = channel.PostnummerGetAll(query);
                Assert.That(postnumre, Is.Not.Null);
                Assert.That(postnumre.Count(), Is.GreaterThan(0));

                var postnummer = postnumre.SingleOrDefault(m => m.Landekode.CompareTo("DK") == 0 && m.Postnummer.CompareTo("7800") == 0);
                Assert.That(postnummer, Is.Not.Null);

                var command = new PostnummerModifyCommand
                                  {
                                      Landekode = postnummer.Landekode,
                                      Postnummer = postnummer.Postnummer,
                                      Bynavn = string.Format("_{0}", postnummer.Bynavn)
                                  };
                var result = channel.PostnummerModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Landekode, Is.Not.Null);
                Assert.That(result.Landekode, Is.EqualTo(command.Landekode));
                Assert.That(result.Postnummer, Is.Not.Null);
                Assert.That(result.Postnummer, Is.EqualTo(command.Postnummer));
                Assert.That(result.Bynavn, Is.Not.Null);
                Assert.That(result.Bynavn, Is.EqualTo(command.Bynavn));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis postnummer ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringHvisPostnummerIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PostnummerGetAllQuery();
                var postnumre = channel.PostnummerGetAll(query);
                Assert.That(postnumre, Is.Not.Null);
                Assert.That(postnumre.Count(), Is.GreaterThan(0));

                var postnummer = postnumre.SingleOrDefault(m => m.Landekode.CompareTo("DK") == 0 && m.Postnummer.CompareTo("7800") == 0);
                Assert.That(postnummer, Is.Not.Null);

                var command = new PostnummerModifyCommand
                                  {
                                      Landekode = postnummer.Landekode,
                                      Postnummer = "XYZ",
                                      Bynavn = string.Format("_{0}", postnummer.Bynavn)
                                  };
                Assert.Throws<FaultException>(() => channel.PostnummerModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, Tester, at der kastes en FaultException ved opdatering, hvis bynavnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringHvisBynavnPåPostnummerErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new PostnummerGetAllQuery();
                var postnumre = channel.PostnummerGetAll(query);
                Assert.That(postnumre, Is.Not.Null);
                Assert.That(postnumre.Count(), Is.GreaterThan(0));

                var postnummer = postnumre.SingleOrDefault(m => m.Landekode.CompareTo("DK") == 0 && m.Postnummer.CompareTo("7800") == 0);
                Assert.That(postnummer, Is.Not.Null);

                var command = new PostnummerModifyCommand
                                  {
                                      Landekode = postnummer.Landekode,
                                      Postnummer = postnummer.Postnummer,
                                      Bynavn = string.Empty
                                  };
                Assert.Throws<FaultException>(() => channel.PostnummerModify(command));
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
                Assert.That(adressegrupper.Count(), Is.GreaterThan(0));
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
        /// Tester, at adressegruppe oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af adressegruppe er testet.")]
        public void TestAtAdressegruppeOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new AdressegruppeAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = "_Test",
                                      AdressegruppeOswebdb = 99
                                  };
                var result = channel.AdressegruppeAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.AdressegruppeOswebdb, Is.EqualTo(command.AdressegruppeOswebdb));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis adressegruppen allerede findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisAdressegruppeAlleredeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new AdressegruppeAddCommand
                                  {
                                      Nummer = 1,
                                      Navn = "_Test",
                                      AdressegruppeOswebdb = 1
                                  };
                Assert.Throws<FaultException>(() => channel.AdressegruppeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisNavnPåAdressegruppeErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new AdressegruppeAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = string.Empty,
                                      AdressegruppeOswebdb = 99
                                  };
                Assert.Throws<FaultException>(() => channel.AdressegruppeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at adressegruppe opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af adressegrupper er testet.")]
        public void TestAtAdressegruppeOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new AdressegruppeGetAllQuery();
                var adressegrupper = channel.AdressegruppeGetAll(query);
                Assert.That(adressegrupper, Is.Not.Null);
                Assert.That(adressegrupper.Count(), Is.GreaterThan(0));

                var adressegruppe = adressegrupper.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(adressegruppe, Is.Not.Null);

                var command = new AdressegruppeModifyCommand
                                  {
                                      Nummer = adressegruppe.Nummer,
                                      Navn = string.Format("_{0}", adressegruppe.Navn),
                                      AdressegruppeOswebdb = adressegruppe.AdressegruppeOswebdb
                                  };
                var result = channel.AdressegruppeModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.AdressegruppeOswebdb, Is.EqualTo(command.AdressegruppeOswebdb));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis adressegruppe ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringHvisAdressegruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new AdressegruppeGetAllQuery();
                var adressegrupper = channel.AdressegruppeGetAll(query);
                Assert.That(adressegrupper, Is.Not.Null);
                Assert.That(adressegrupper.Count(), Is.GreaterThan(0));

                var adressegruppe = adressegrupper.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(adressegruppe, Is.Not.Null);

                var command = new AdressegruppeModifyCommand
                                  {
                                      Nummer = -1,
                                      Navn = string.Format("_{0}", adressegruppe.Navn),
                                      AdressegruppeOswebdb = adressegruppe.AdressegruppeOswebdb
                                  };
                Assert.Throws<FaultException>(() => channel.AdressegruppeModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, Tester, at der kastes en FaultException ved opdatering, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringHvisNavnPåAdressegruppeErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new AdressegruppeGetAllQuery();
                var adressegrupper = channel.AdressegruppeGetAll(query);
                Assert.That(adressegrupper, Is.Not.Null);
                Assert.That(adressegrupper.Count(), Is.GreaterThan(0));

                var adressegruppe = adressegrupper.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(adressegruppe, Is.Not.Null);

                var command = new AdressegruppeModifyCommand
                                  {
                                      Nummer = adressegruppe.Nummer,
                                      Navn = string.Empty,
                                      AdressegruppeOswebdb = adressegruppe.AdressegruppeOswebdb
                                  };
                Assert.Throws<FaultException>(() => channel.AdressegruppeModify(command));
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
                Assert.That(betalingsbetingelser.Count(), Is.GreaterThan(0));
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

        /// <summary>
        /// Tester, at betalingsbetingelse oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af betalingsbetingelse er testet.")]
        public void TestAtBetalingsbetingelseOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new BetalingsbetingelseAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = "_Test"
                                  };
                var result = channel.BetalingsbetingelseAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis betalingsbetingelsen allerede findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisBetalingsbetingelseAlleredeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new BetalingsbetingelseAddCommand
                                  {
                                      Nummer = 1,
                                      Navn = "_Test"
                                  };
                Assert.Throws<FaultException>(() => channel.BetalingsbetingelseAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisNavnPåBetalingsbetingelseErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var command = new BetalingsbetingelseAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = string.Empty
                                  };
                Assert.Throws<FaultException>(() => channel.BetalingsbetingelseAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at betalingsbetingelse opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af betalingsbetingelser er testet.")]
        public void TestAtBetalingsbetingelseOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new BetalingsbetingelseGetAllQuery();
                var betalingsbetingelser = channel.BetalingsbetingelseGetAll(query);
                Assert.That(betalingsbetingelser, Is.Not.Null);
                Assert.That(betalingsbetingelser.Count(), Is.GreaterThan(0));

                var betalingsbetingelse = betalingsbetingelser.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(betalingsbetingelse, Is.Not.Null);

                var command = new BetalingsbetingelseModifyCommand
                                  {
                                      Nummer = betalingsbetingelse.Nummer,
                                      Navn = string.Format("_{0}", betalingsbetingelse.Navn)
                                  };
                var result = channel.BetalingsbetingelseModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis betalingsbetingelse ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringHvisBetalingsbetingelseIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new BetalingsbetingelseGetAllQuery();
                var betalingsbetingelser = channel.BetalingsbetingelseGetAll(query);
                Assert.That(betalingsbetingelser, Is.Not.Null);
                Assert.That(betalingsbetingelser.Count(), Is.GreaterThan(0));

                var betalingsbetingelse = betalingsbetingelser.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(betalingsbetingelse, Is.Not.Null);

                var command = new BetalingsbetingelseModifyCommand
                                  {
                                      Nummer = -1,
                                      Navn = string.Format("_{0}", betalingsbetingelse.Navn)
                                  };
                Assert.Throws<FaultException>(() => channel.BetalingsbetingelseModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, Tester, at der kastes en FaultException ved opdatering, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringHvisNavnPåBetalingsbetingelseErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IAdresseRepositoryService>("AdresseRepositoryService");
            try
            {
                var query = new BetalingsbetingelseGetAllQuery();
                var betalingsbetingelser = channel.BetalingsbetingelseGetAll(query);
                Assert.That(betalingsbetingelser, Is.Not.Null);
                Assert.That(betalingsbetingelser.Count(), Is.GreaterThan(0));

                var betalingsbetingelse = betalingsbetingelser.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(betalingsbetingelse, Is.Not.Null);

                var command = new BetalingsbetingelseModifyCommand
                                  {
                                      Nummer = betalingsbetingelse.Nummer,
                                      Navn = string.Empty
                                  };
                Assert.Throws<FaultException>(() => channel.BetalingsbetingelseModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }
    }
}
