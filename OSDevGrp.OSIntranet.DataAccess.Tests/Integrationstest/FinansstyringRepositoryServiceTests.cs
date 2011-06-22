using System;
using System.Linq;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf.ChannelFactory;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Enums;
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
    public class FinansstyringRepositoryServiceTests
    {
        private const int RegnskabsnummerTilTest = 1;

        /// <summary>
        /// Tester, at regnskaber hentes.
        /// </summary>
        [Test]
        public void TestAtRegnskaberHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new RegnskabGetAllQuery();
                var regnskaber = channel.RegnskabGetAll(query);
                Assert.That(regnskaber, Is.Not.Null);
                Assert.That(regnskaber.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at et givent regnskab hentes.
        /// </summary>
        [Test]
        public void TestAtRegnskabHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new RegnskabGetByNummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest
                                };
                var regnskab = channel.RegnskabGetByNummer(query);
                Assert.That(regnskab, Is.Not.Null);
                Assert.That(regnskab.Nummer, Is.EqualTo(query.Regnskabsnummer));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes FaultException(s), hvis regnskab ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisRegnskabIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var regnskabQuery = new RegnskabGetByNummerQuery
                                {
                                    Regnskabsnummer = -1
                                };
                Assert.Throws<FaultException>(() => channel.RegnskabGetByNummer(regnskabQuery));
                var kontiQuery = new KontoGetByRegnskabQuery
                                     {
                                         Regnskabsnummer = -1
                                     };
                Assert.Throws<FaultException>(() => channel.KontoGetByRegnskab(kontiQuery));
                var kontoQuery = new KontoGetByRegnskabAndKontonummerQuery
                                     {
                                         Regnskabsnummer = -1,
                                         Kontonummer = "XYZ"
                                     };
                Assert.Throws<FaultException>(() => channel.KontoGetByRegnskabAndKontonummer(kontoQuery));
                var budgetkontiQuery = new BudgetkontoGetByRegnskabQuery
                                           {
                                               Regnskabsnummer = -1
                                           };
                Assert.Throws<FaultException>(() => channel.BudgetkontoGetByRegnskab(budgetkontiQuery));
                var budgetkontoQuery = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                           {
                                               Regnskabsnummer = -1,
                                               Kontonummer = "XYZ"
                                           };
                Assert.Throws<FaultException>(() => channel.BudgetkontoGetByRegnskabAndKontonummer(budgetkontoQuery));
                var bogføringslinjeQuery = new BogføringslinjeGetByRegnskabQuery
                                               {
                                                   Regnskabsnummer = -1
                                               };
                Assert.Throws<FaultException>(() => channel.BogføringslinjeGetByRegnskab(bogføringslinjeQuery));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at konti hentes for et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtKontiHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontoGetByRegnskabQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest
                                };
                var konti = channel.KontoGetByRegnskab(query);
                Assert.That(konti, Is.Not.Null);
                Assert.That(konti.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en konto hentes i et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtKontoHentes()
        {
            var random = new Random(DateTime.Now.Second);
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                // Hent liste af konti i regnskabet.
                var kontiQuery = new KontoGetByRegnskabQuery
                                     {
                                         Regnskabsnummer = RegnskabsnummerTilTest
                                     };
                var konti = channel.KontoGetByRegnskab(kontiQuery);
                Assert.That(konti, Is.Not.Null);
                Assert.That(konti.Count(), Is.GreaterThan(0));
                // Hent en given konto i regnskabet.
                var no = random.Next(0, konti.Count() - 1);
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = konti.ElementAt(no).Kontonummer
                                };
                var konto = channel.KontoGetByRegnskabAndKontonummer(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Regnskab, Is.Not.Null);
                Assert.That(konto.Regnskab.Nummer, Is.EqualTo(query.Regnskabsnummer));
                Assert.That(konto.Kontonummer, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.EqualTo(query.Kontonummer));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes FaultException, hvis konto ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisKontoIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "XYZ"
                                };
                Assert.Throws<FaultException>(() => channel.KontoGetByRegnskabAndKontonummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en konto oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af konto er testet.")]
        public void TestAtKontoOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "_KONTONUMMER",
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Kontogruppe = 1
                                  };
                var result = channel.KontoAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Regnskab, Is.Not.Null);
                Assert.That(result.Regnskab.Nummer, Is.EqualTo(command.Regnskabsnummer));
                Assert.That(result.Kontonummer, Is.Not.Null);
                Assert.That(result.Kontonummer, Is.EqualTo(command.Kontonummer));
                Assert.That(result.Kontonavn, Is.Not.Null);
                Assert.That(result.Kontonavn, Is.EqualTo(command.Kontonavn));
                Assert.That(result.Beskrivelse, Is.EqualTo(command.Beskrivelse));
                Assert.That(result.Note, Is.EqualTo(command.Note));
                Assert.That(result.Kontogruppe, Is.Not.Null);
                Assert.That(result.Kontogruppe.Nummer, Is.EqualTo(command.Kontogruppe));
                Assert.That(result.Kreditoplysninger, Is.Not.Null);
                Assert.That(result.Kreditoplysninger.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en konto, hvis kontonummer allerede findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfKontoHvisKontoAlleredeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "DANKORT",
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Kontogruppe = 1
                                  };
                Assert.Throws<FaultException>(() => channel.KontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en konto, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfKontoHvisRegnskabIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontoAddCommand
                                  {
                                      Regnskabsnummer = -1,
                                      Kontonummer = "_KONTONUMMER",
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Kontogruppe = 1
                                  };
                Assert.Throws<FaultException>(() => channel.KontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en konto, hvis kontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfKontoHvisKontogruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "_KONTONUMMER",
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Kontogruppe = -1
                                  };
                Assert.Throws<FaultException>(() => channel.KontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en konto, hvis kontonummeret er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfKontoHvisKontonummerErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = string.Empty,
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Kontogruppe = 1
                                  };
                Assert.Throws<FaultException>(() => channel.KontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en konto, hvis kontonavnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfKontoHvisKontonavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "_KONTONUMMER",
                                      Kontonavn = string.Empty,
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Kontogruppe = 1
                                  };
                Assert.Throws<FaultException>(() => channel.KontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en given konto opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af konti er testet.")]
        public void TestAtKontoOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "DANKORT"
                                };
                var konto = channel.KontoGetByRegnskabAndKontonummer(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new KontoModifyCommand
                                  {
                                      Regnskabsnummer = konto.Regnskab.Nummer,
                                      Kontonummer = konto.Kontonummer,
                                      Kontonavn = string.Format("_{0}", konto.Kontonavn),
                                      Beskrivelse = konto.Beskrivelse,
                                      Note = konto.Note,
                                      Kontogruppe = konto.Kontogruppe.Nummer
                                  };
                var result = channel.KontoModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Regnskab, Is.Not.Null);
                Assert.That(result.Regnskab.Nummer, Is.EqualTo(command.Regnskabsnummer));
                Assert.That(result.Kontonummer, Is.Not.Null);
                Assert.That(result.Kontonummer, Is.EqualTo(command.Kontonummer));
                Assert.That(result.Kontonavn, Is.Not.Null);
                Assert.That(result.Kontonavn, Is.EqualTo(command.Kontonavn));
                Assert.That(result.Beskrivelse, Is.EqualTo(command.Beskrivelse));
                Assert.That(result.Note, Is.EqualTo(command.Note));
                Assert.That(result.Kontogruppe, Is.Not.Null);
                Assert.That(result.Kontogruppe.Nummer, Is.EqualTo(command.Kontogruppe));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given konto, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKontoHvisRegnskabIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "DANKORT"
                                };
                var konto = channel.KontoGetByRegnskabAndKontonummer(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new KontoModifyCommand
                                  {
                                      Regnskabsnummer = -1,
                                      Kontonummer = konto.Kontonummer,
                                      Kontonavn = konto.Kontonavn,
                                      Beskrivelse = konto.Beskrivelse,
                                      Note = konto.Note,
                                      Kontogruppe = konto.Kontogruppe.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.KontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given konto, hvis kontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKontoHvisKontogruppenIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "DANKORT"
                                };
                var konto = channel.KontoGetByRegnskabAndKontonummer(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new KontoModifyCommand
                                  {
                                      Regnskabsnummer = konto.Regnskab.Nummer,
                                      Kontonummer = konto.Kontonummer,
                                      Kontonavn = konto.Kontonavn,
                                      Beskrivelse = konto.Beskrivelse,
                                      Note = konto.Note,
                                      Kontogruppe = -1
                                  };
                Assert.Throws<FaultException>(() => channel.KontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given konto, hvis kontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKontoHvisKontoIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "DANKORT"
                                };
                var konto = channel.KontoGetByRegnskabAndKontonummer(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new KontoModifyCommand
                                  {
                                      Regnskabsnummer = konto.Regnskab.Nummer,
                                      Kontonummer = "XYZ",
                                      Kontonavn = konto.Kontonavn,
                                      Beskrivelse = konto.Beskrivelse,
                                      Note = konto.Note,
                                      Kontogruppe = konto.Kontogruppe.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.KontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given konto, hvis kontonummeret er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKontoHvisKontonummerErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "DANKORT"
                                };
                var konto = channel.KontoGetByRegnskabAndKontonummer(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new KontoModifyCommand
                {
                    Regnskabsnummer = konto.Regnskab.Nummer,
                    Kontonummer = string.Empty,
                    Kontonavn = konto.Kontonavn,
                    Beskrivelse = konto.Beskrivelse,
                    Note = konto.Note,
                    Kontogruppe = konto.Kontogruppe.Nummer
                };
                Assert.Throws<FaultException>(() => channel.KontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given konto, hvis kontonavnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKontoHvisKontonavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "DANKORT"
                                };
                var konto = channel.KontoGetByRegnskabAndKontonummer(query);
                Assert.That(konto, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.Not.Null);
                Assert.That(konto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new KontoModifyCommand
                                  {
                                      Regnskabsnummer = konto.Regnskab.Nummer,
                                      Kontonummer = konto.Kontonummer,
                                      Kontonavn = string.Empty,
                                      Beskrivelse = konto.Beskrivelse,
                                      Note = konto.Note,
                                      Kontogruppe = konto.Kontogruppe.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.KontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at kreditoplysninger oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af kreditoplysninger er testet.")]
        public void TestAtKreditoplysningerOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var dato = DateTime.Now.AddYears(5);
                var command = new KreditoplysningerAddOrModifyCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "DANKORT",
                                      År = dato.Year,
                                      Måned = dato.Month,
                                      Kredit = 5000M
                                  };
                var result = channel.KreditoplysningerAddOrModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.År, Is.EqualTo(command.År));
                Assert.That(result.Måned, Is.EqualTo(command.Måned));
                Assert.That(result.Kredit, Is.EqualTo(command.Kredit));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at kreditoplysninger opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af kreditoplysninger er testet.")]
        public void TestAtKreditoplysningerOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var dato = DateTime.Now;
                var command = new KreditoplysningerAddOrModifyCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "DANKORT",
                                      År = dato.Year,
                                      Måned = dato.Month,
                                      Kredit = 5000M
                                  };
                var result = channel.KreditoplysningerAddOrModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.År, Is.EqualTo(command.År));
                Assert.That(result.Måned, Is.EqualTo(command.Måned));
                Assert.That(result.Kredit, Is.EqualTo(command.Kredit));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af kreditoplysninger, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKreditoplysningerHvisRegnskabIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var dato = DateTime.Now;
                var command = new KreditoplysningerAddOrModifyCommand
                                  {
                                      Regnskabsnummer = -1,
                                      Kontonummer = "DANKORT",
                                      År = dato.Year,
                                      Måned = dato.Month,
                                      Kredit = 0M
                                  };
                Assert.Throws<FaultException>(() => channel.KreditoplysningerAddOrModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af kreditoplysninger, hvis kontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKreditoplysningerHvisKontoIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var dato = DateTime.Now;
                var command = new KreditoplysningerAddOrModifyCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "XYZ",
                                      År = dato.Year,
                                      Måned = dato.Month,
                                      Kredit = 0M
                                  };
                Assert.Throws<FaultException>(() => channel.KreditoplysningerAddOrModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at budgetkonti hentes for et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtBudgetkontiHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontoGetByRegnskabQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest
                                };
                var budgetkonti = channel.BudgetkontoGetByRegnskab(query);
                Assert.That(budgetkonti, Is.Not.Null);
                Assert.That(budgetkonti.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en budgetkonto hentes i et givent regnskab.
        /// </summary>
        [Test]
        public void TestAtBudgetkontoHentes()
        {
            var random = new Random(DateTime.Now.Second);
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                // Hent liste af konti i regnskabet.
                var budgetkontiQuery = new BudgetkontoGetByRegnskabQuery
                                           {
                                               Regnskabsnummer = RegnskabsnummerTilTest
                                           };
                var budgetkonti = channel.BudgetkontoGetByRegnskab(budgetkontiQuery);
                Assert.That(budgetkonti, Is.Not.Null);
                Assert.That(budgetkonti.Count(), Is.GreaterThan(0));
                // Hent en given konto i regnskabet.
                var no = random.Next(0, budgetkonti.Count() - 1);
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = budgetkonti.ElementAt(no).Kontonummer
                                };
                var budgetkonto = channel.BudgetkontoGetByRegnskabAndKontonummer(query);
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Regnskab, Is.Not.Null);
                Assert.That(budgetkonto.Regnskab.Nummer, Is.EqualTo(query.Regnskabsnummer));
                Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.EqualTo(query.Kontonummer));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes FaultException, hvis budgetkonto ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisBudgetkontoIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "XYZ"
                                };
                Assert.Throws<FaultException>(() => channel.BudgetkontoGetByRegnskabAndKontonummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en budgetkonto oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af budgetkonto er testet.")]
        public void TestAtBudgetkontoOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "_KONTONUMMER",
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Budgetkontogruppe = 1
                                  };
                var result = channel.BudgetkontoAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Regnskab, Is.Not.Null);
                Assert.That(result.Regnskab.Nummer, Is.EqualTo(command.Regnskabsnummer));
                Assert.That(result.Kontonummer, Is.Not.Null);
                Assert.That(result.Kontonummer, Is.EqualTo(command.Kontonummer));
                Assert.That(result.Kontonavn, Is.Not.Null);
                Assert.That(result.Kontonavn, Is.EqualTo(command.Kontonavn));
                Assert.That(result.Beskrivelse, Is.EqualTo(command.Beskrivelse));
                Assert.That(result.Note, Is.EqualTo(command.Note));
                Assert.That(result.Budgetkontogruppe, Is.Not.Null);
                Assert.That(result.Budgetkontogruppe.Nummer, Is.EqualTo(command.Budgetkontogruppe));
                Assert.That(result.Budgetoplysninger, Is.Not.Null);
                Assert.That(result.Budgetoplysninger.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en budgetkonto, hvis kontonummer allerede findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBudgetkontoHvisBudgetkontoAlleredeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "8990",
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Budgetkontogruppe = 1
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en budgetkonto, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBudgetkontoHvisRegnskabIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontoAddCommand
                                  {
                                      Regnskabsnummer = -1,
                                      Kontonummer = "_KONTONUMMER",
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Budgetkontogruppe = 1
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en budgetkonto, hvis gruppen til budgetkonti ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBudgetkontoHvisBudgetkontogruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "_KONTONUMMER",
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Budgetkontogruppe = -1
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en budgetkonto, hvis kontonummeret er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBudgetkontoHvisBudgetkontonummerErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = string.Empty,
                                      Kontonavn = "_Kontonavn",
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Budgetkontogruppe = 1
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af en budgetkonto, hvis kontonavnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBudgetkontoHvisBudgetkontonavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontoAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Kontonummer = "_KONTONUMMER",
                                      Kontonavn = string.Empty,
                                      Beskrivelse = "_Beskrivelse",
                                      Note = "_Note",
                                      Budgetkontogruppe = 1
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en given budgetkonto opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af budgetkonti er testet.")]
        public void TestAtBudgetkontoOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "8990"
                                };
                var budgetkonto = channel.BudgetkontoGetByRegnskabAndKontonummer(query);
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new BudgetkontoModifyCommand
                                  {
                                      Regnskabsnummer = budgetkonto.Regnskab.Nummer,
                                      Kontonummer = budgetkonto.Kontonummer,
                                      Kontonavn = string.Format("_{0}", budgetkonto.Kontonavn),
                                      Beskrivelse = budgetkonto.Beskrivelse,
                                      Note = budgetkonto.Note,
                                      Budgetkontogruppe = budgetkonto.Budgetkontogruppe.Nummer
                                  };
                var result = channel.BudgetkontoModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Regnskab, Is.Not.Null);
                Assert.That(result.Regnskab.Nummer, Is.EqualTo(command.Regnskabsnummer));
                Assert.That(result.Kontonummer, Is.Not.Null);
                Assert.That(result.Kontonummer, Is.EqualTo(command.Kontonummer));
                Assert.That(result.Kontonavn, Is.Not.Null);
                Assert.That(result.Kontonavn, Is.EqualTo(command.Kontonavn));
                Assert.That(result.Beskrivelse, Is.EqualTo(command.Beskrivelse));
                Assert.That(result.Note, Is.EqualTo(command.Note));
                Assert.That(result.Budgetkontogruppe, Is.Not.Null);
                Assert.That(result.Budgetkontogruppe.Nummer, Is.EqualTo(command.Budgetkontogruppe));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given budgetkonto, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetkontoHvisRegnskabIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "8990"
                                };
                var budgetkonto = channel.BudgetkontoGetByRegnskabAndKontonummer(query);
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new BudgetkontoModifyCommand
                                  {
                                      Regnskabsnummer = -1,
                                      Kontonummer = budgetkonto.Kontonummer,
                                      Kontonavn = budgetkonto.Kontonavn,
                                      Beskrivelse = budgetkonto.Beskrivelse,
                                      Note = budgetkonto.Note,
                                      Budgetkontogruppe = budgetkonto.Budgetkontogruppe.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given budgetkonto, hvis gruppen til budgetkonti ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetkontoHvisBudgetkontogruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "8990"
                                };
                var budgetkonto = channel.BudgetkontoGetByRegnskabAndKontonummer(query);
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new BudgetkontoModifyCommand
                                  {
                                      Regnskabsnummer = budgetkonto.Regnskab.Nummer,
                                      Kontonummer = budgetkonto.Kontonummer,
                                      Kontonavn = budgetkonto.Kontonavn,
                                      Beskrivelse = budgetkonto.Beskrivelse,
                                      Note = budgetkonto.Note,
                                      Budgetkontogruppe = -1
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given budgetkonto, hvis budgetkontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetkontoHvisBudgetkontoIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "8990"
                                };
                var budgetkonto = channel.BudgetkontoGetByRegnskabAndKontonummer(query);
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new BudgetkontoModifyCommand
                                  {
                                      Regnskabsnummer = budgetkonto.Regnskab.Nummer,
                                      Kontonummer = "XYZ",
                                      Kontonavn = budgetkonto.Kontonavn,
                                      Beskrivelse = budgetkonto.Beskrivelse,
                                      Note = budgetkonto.Note,
                                      Budgetkontogruppe = budgetkonto.Budgetkontogruppe.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given budgetkonto, hvis kontonummeret er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetkontoHvisKontonummerErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "8990"
                                };
                var budgetkonto = channel.BudgetkontoGetByRegnskabAndKontonummer(query);
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new BudgetkontoModifyCommand
                                  {
                                      Regnskabsnummer = budgetkonto.Regnskab.Nummer,
                                      Kontonummer = string.Empty,
                                      Kontonavn = budgetkonto.Kontonavn,
                                      Beskrivelse = budgetkonto.Beskrivelse,
                                      Note = budgetkonto.Note,
                                      Budgetkontogruppe = budgetkonto.Budgetkontogruppe.Nummer
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af en given budgetkonto, hvis kontonavnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetkontoHvisKontonnavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = "8990"
                                };
                var budgetkonto = channel.BudgetkontoGetByRegnskabAndKontonummer(query);
                Assert.That(budgetkonto, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.Not.Null);
                Assert.That(budgetkonto.Kontonummer, Is.EqualTo(query.Kontonummer));

                var command = new BudgetkontoModifyCommand
                {
                    Regnskabsnummer = budgetkonto.Regnskab.Nummer,
                    Kontonummer = budgetkonto.Kontonummer,
                    Kontonavn = string.Empty,
                    Beskrivelse = budgetkonto.Beskrivelse,
                    Note = budgetkonto.Note,
                    Budgetkontogruppe = budgetkonto.Budgetkontogruppe.Nummer
                };
                Assert.Throws<FaultException>(() => channel.BudgetkontoModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at budgetoplysninger oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af budgetoplysninger er testet.")]
        public void TestAtBudgetoplysningerOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var dato = DateTime.Now.AddYears(5);
                var command = new BudgetoplysningerAddOrModifyCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Budgetkontonummer = "8990",
                                      År = dato.Year,
                                      Måned = dato.Month,
                                      Indtægter = 2500M,
                                      Udgifter = 5000M
                                  };
                var result = channel.BudgetoplysningerAddOrModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.År, Is.EqualTo(command.År));
                Assert.That(result.Måned, Is.EqualTo(command.Måned));
                Assert.That(result.Indtægter, Is.EqualTo(command.Indtægter));
                Assert.That(result.Udgifter, Is.EqualTo(command.Udgifter));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at budgetoplysninger opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af budgetoplysninger er testet.")]
        public void TestAtBudgetoplysningerOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var dato = DateTime.Now;
                var command = new BudgetoplysningerAddOrModifyCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Budgetkontonummer = "8990",
                                      År = dato.Year,
                                      Måned = dato.Month,
                                      Indtægter = 2500M,
                                      Udgifter = 5000M
                                  };
                var result = channel.BudgetoplysningerAddOrModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.År, Is.EqualTo(command.År));
                Assert.That(result.Måned, Is.EqualTo(command.Måned));
                Assert.That(result.Indtægter, Is.EqualTo(command.Indtægter));
                Assert.That(result.Udgifter, Is.EqualTo(command.Udgifter));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af budgetoplysninger, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetoplysningerHvisRegnskabIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var dato = DateTime.Now;
                var command = new BudgetoplysningerAddOrModifyCommand
                                  {
                                      Regnskabsnummer = -1,
                                      Budgetkontonummer = "8990",
                                      År = dato.Year,
                                      Måned = dato.Month,
                                      Indtægter = 0M,
                                      Udgifter = 0M
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetoplysningerAddOrModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af budgetoplysninger, hvis budgetkontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetoplysningerHvisBudgetkontoIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var dato = DateTime.Now;
                var command = new BudgetoplysningerAddOrModifyCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Budgetkontonummer = "XYZ",
                                      År = dato.Year,
                                      Måned = dato.Month,
                                      Indtægter = 0M,
                                      Udgifter = 0M
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetoplysningerAddOrModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at bogføringslinjer for et givent regnskab hentes.
        /// </summary>
        [Test]
        public void TestAtBogføringslinjerHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BogføringslinjeGetByRegnskabQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest
                                };
                var bogføringslinjer = channel.BogføringslinjeGetByRegnskab(query);
                Assert.That(bogføringslinjer, Is.Not.Null);
                Assert.That(bogføringslinjer.Count(), Is.GreaterThan(0));
                Assert.That(bogføringslinjer.Where(m => m.Konto != null).Count(), Is.GreaterThan(0));
                Assert.That(bogføringslinjer.Where(m => m.Budgetkonto != null).Count(), Is.GreaterThan(0));
                Assert.That(bogføringslinjer.Where(m => m.Adresse != null).Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at bogføringslinjer oprettes på et givent regnskab.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af bogføringslinjer er testet.")]
        public void TestAtBogføringslinjeOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                // Find antallet af bogførte linjer.
                var query = new BogføringslinjeGetByRegnskabQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest
                                };
                var bogførteLinjer = channel.BogføringslinjeGetByRegnskab(query).Count();
                Assert.That(bogførteLinjer, Is.GreaterThan(0));
                // Bogfør linje.
                var command = new BogføringslinjeAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Bogføringsdato = DateTime.Now,
                                      Bilag = null,
                                      Kontonummer = "DANKORT",
                                      Tekst = "Test af DataAccess",
                                      Budgetkontonummer = "8990",
                                      Debit = 1000M,
                                      Kredit = 0M,
                                      AdresseId = 1
                                  };
                var result = channel.BogføringslinjeAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Løbenummer, Is.GreaterThan(0));
                Assert.That(result.Dato, Is.EqualTo(command.Bogføringsdato).Within(1).Days);
                Assert.That(result.Bilag, Is.EqualTo(command.Bilag));
                Assert.That(result.Konto, Is.Not.Null);
                Assert.That(result.Konto.Kontonummer, Is.EqualTo(command.Kontonummer));
                Assert.That(result.Tekst, Is.Not.Null);
                Assert.That(result.Tekst, Is.EqualTo(command.Tekst));
                Assert.That(result.Budgetkonto, Is.Not.Null);
                Assert.That(result.Budgetkonto.Kontonummer, Is.EqualTo(command.Budgetkontonummer));
                Assert.That(result.Debit, Is.EqualTo(command.Debit));
                Assert.That(result.Kredit, Is.EqualTo(command.Kredit));
                Assert.That(result.Adresse, Is.Not.Null);
                Assert.That(result.Adresse.Nummer, Is.EqualTo(command.AdresseId));
                // Modpostér bogført linje. 
                command.Kredit = command.Debit;
                command.Debit = 0M;
                result = channel.BogføringslinjeAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Løbenummer, Is.GreaterThan(0));
                Assert.That(result.Dato, Is.EqualTo(command.Bogføringsdato).Within(1).Days);
                Assert.That(result.Bilag, Is.EqualTo(command.Bilag));
                Assert.That(result.Konto, Is.Not.Null);
                Assert.That(result.Konto.Kontonummer, Is.EqualTo(command.Kontonummer));
                Assert.That(result.Tekst, Is.Not.Null);
                Assert.That(result.Tekst, Is.EqualTo(command.Tekst));
                Assert.That(result.Budgetkonto, Is.Not.Null);
                Assert.That(result.Budgetkonto.Kontonummer, Is.EqualTo(command.Budgetkontonummer));
                Assert.That(result.Debit, Is.EqualTo(command.Debit));
                Assert.That(result.Kredit, Is.EqualTo(command.Kredit));
                Assert.That(result.Adresse, Is.Not.Null);
                Assert.That(result.Adresse.Nummer, Is.EqualTo(command.AdresseId));
                // Check at antallet af bogførte linjer er steget med 2.
                var antalLinjer = channel.BogføringslinjeGetByRegnskab(query).Count();
                Assert.That(antalLinjer, Is.GreaterThan(0));
                Assert.That(antalLinjer, Is.EqualTo(bogførteLinjer + 2));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af bogføringslinje, hvis teksten er tom.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBogføringslinjeHvisTekstErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BogføringslinjeAddCommand
                                  {
                                      Regnskabsnummer = 1,
                                      Bogføringsdato = DateTime.Now,
                                      Bilag = null,
                                      Kontonummer = "DANKORT",
                                      Tekst = string.Empty,
                                      Budgetkontonummer = "8990",
                                      Debit = 0M,
                                      Kredit = 0M,
                                      AdresseId = 1
                                  };
                Assert.Throws<FaultException>(() => channel.BogføringslinjeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af bogføringslinje, hvis regnskabet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBogføringslinjeHvisRegnskabIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BogføringslinjeAddCommand
                                  {
                                      Regnskabsnummer = -1,
                                      Bogføringsdato = DateTime.Now,
                                      Bilag = null,
                                      Kontonummer = "DANKORT",
                                      Tekst = "Test af DataAccess",
                                      Budgetkontonummer = "8990",
                                      Debit = 0M,
                                      Kredit = 0M,
                                      AdresseId = 1
                                  };
                Assert.Throws<FaultException>(() => channel.BogføringslinjeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af bogføringslinje, hvis kontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBogføringslinjeHvisKontoIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BogføringslinjeAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Bogføringsdato = DateTime.Now,
                                      Bilag = null,
                                      Kontonummer = "XYZ",
                                      Tekst = "Test af DataAccess",
                                      Budgetkontonummer = "8990",
                                      Debit = 0M,
                                      Kredit = 0M,
                                      AdresseId = 1
                                  };
                Assert.Throws<FaultException>(() => channel.BogføringslinjeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af bogføringslinje, hvis budgetkontoen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBogføringslinjeHvisBudgetkontoIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BogføringslinjeAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Bogføringsdato = DateTime.Now,
                                      Bilag = null,
                                      Kontonummer = "DANKORT",
                                      Tekst = "Test af DataAccess",
                                      Budgetkontonummer = "XYZ",
                                      Debit = 0M,
                                      Kredit = 0M,
                                      AdresseId = 1
                                  };
                Assert.Throws<FaultException>(() => channel.BogføringslinjeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af bogføringslinje, hvis adressen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBogføringslinjeHvisAdresseIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BogføringslinjeAddCommand
                                  {
                                      Regnskabsnummer = RegnskabsnummerTilTest,
                                      Bogføringsdato = DateTime.Now,
                                      Bilag = null,
                                      Kontonummer = "DANKORT",
                                      Tekst = "Test af DataAccess",
                                      Budgetkontonummer = "8990",
                                      Debit = 0M,
                                      Kredit = 0M,
                                      AdresseId = -1
                                  };
                Assert.Throws<FaultException>(() => channel.BogføringslinjeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at kontogrupper hentes.
        /// </summary>
        [Test]
        public void TestAtKontogrupperHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontogruppeGetAllQuery();
                var kontogrupper = channel.KontogruppeGetAll(query);
                Assert.That(kontogrupper, Is.Not.Null);
                Assert.That(kontogrupper.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en kontogruppe hentes.
        /// </summary>
        [Test]
        public void TestAtKontogruppeHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontogruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var kontogruppe = channel.KontogruppeGetByNummer(query);
                Assert.That(kontogruppe, Is.Not.Null);
                Assert.That(kontogruppe.Nummer, Is.EqualTo(query.Nummer));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException, hvis kontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisKontogruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontogruppeGetByNummerQuery
                                {
                                    Nummer = -1
                                };
                Assert.Throws<FaultException>(() => channel.KontogruppeGetByNummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at kontogruppe oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af kontogrupper er testet")]
        public void TestAtKontogruppeOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontogruppeAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = "_Test",
                                      KontogruppeType = KontogruppeType.Passiver
                                  };
                var result = channel.KontogruppeAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.KontogruppeType, Is.EqualTo(command.KontogruppeType));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af kontogruppe, der gruppen til kontogruppe allerede findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfKontogruppeHvisBudgetkontogruppeAlleredeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontogruppeAddCommand
                                  {
                                      Nummer = 1,
                                      Navn = "_Test",
                                      KontogruppeType = KontogruppeType.Passiver
                                  };
                Assert.Throws<FaultException>(() => channel.KontogruppeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af kontogruppe, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfKontogruppeHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new KontogruppeAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = string.Empty,
                                      KontogruppeType = KontogruppeType.Passiver
                                  };
                Assert.Throws<FaultException>(() => channel.KontogruppeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at kontogruppe opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af kontogrupper er testet.")]
        public void TestAtKontogruppeOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontogruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var kontogruppe = channel.KontogruppeGetByNummer(query);
                Assert.That(kontogruppe, Is.Not.Null);
                Assert.That(kontogruppe.Nummer, Is.EqualTo(query.Nummer));

                var command = new KontogruppeModifyCommand
                                  {
                                      Nummer = kontogruppe.Nummer,
                                      Navn = string.Format("_{0}", kontogruppe.Navn),
                                      KontogruppeType = kontogruppe.KontogruppeType
                                  };
                var result = channel.KontogruppeModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.KontogruppeType, Is.EqualTo(command.KontogruppeType));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af kontogruppe, hvis kontogruppen ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKontogruppeHvisKontogruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontogruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var kontogruppe = channel.KontogruppeGetByNummer(query);
                Assert.That(kontogruppe, Is.Not.Null);
                Assert.That(kontogruppe.Nummer, Is.EqualTo(query.Nummer));

                var command = new KontogruppeModifyCommand
                                  {
                                      Nummer = -1,
                                      Navn = kontogruppe.Navn,
                                      KontogruppeType = kontogruppe.KontogruppeType
                                  };
                Assert.Throws<FaultException>(() => channel.KontogruppeModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af kontogruppe, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfKontogruppeHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new KontogruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var kontogruppe = channel.KontogruppeGetByNummer(query);
                Assert.That(kontogruppe, Is.Not.Null);
                Assert.That(kontogruppe.Nummer, Is.EqualTo(query.Nummer));

                var command = new KontogruppeModifyCommand
                                  {
                                      Nummer = kontogruppe.Nummer,
                                      Navn = string.Empty,
                                      KontogruppeType = kontogruppe.KontogruppeType
                                  };
                Assert.Throws<FaultException>(() => channel.KontogruppeModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at grupper for budgetkonti hentes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogrupperHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontogruppeGetAllQuery();
                var budgetkontogrupper = channel.BudgetkontogruppeGetAll(query);
                Assert.That(budgetkontogrupper, Is.Not.Null);
                Assert.That(budgetkontogrupper.Count(), Is.GreaterThan(0));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at en gruppe for budgetkoni hentes.
        /// </summary>
        [Test]
        public void TestAtBudgetkontogruppeHentes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontogruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var budgetkontogruppe = channel.BudgetkontogruppeGetByNummer(query);
                Assert.That(budgetkontogruppe, Is.Not.Null);
                Assert.That(budgetkontogruppe.Nummer, Is.EqualTo(query.Nummer));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException, hvis gruppen for budgetkonti ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesHvisBudgetkontogruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontogruppeGetByNummerQuery
                                {
                                    Nummer = -1
                                };
                Assert.Throws<FaultException>(() => channel.BudgetkontogruppeGetByNummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at gruppe til budgetkonti oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af gruppe til budgetkonti er testet")]
        public void TestAtBudgetkontogruppeOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontogruppeAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = "_Test"
                                  };
                var result = channel.BudgetkontogruppeAdd(command);
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
        /// Tester, at der kastes en FaultException ved oprettelse af gruppe til budgetkonti, der gruppen til budgetkonti allerede findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBudgetkontogruppeHvisBudgetkontogruppeAlleredeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontogruppeAddCommand
                                  {
                                      Nummer = 1,
                                      Navn = "_Test"
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontogruppeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse af gruppe til budgetkonti, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseAfBudgetkontogruppeHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var command = new BudgetkontogruppeAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = string.Empty
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontogruppeAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at gruppe til budgetkonti opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af grupper til budgetkonti er testet.")]
        public void TestAtBudgetkontogruppeOpdateres()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontogruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var budgetkontogruppe = channel.BudgetkontogruppeGetByNummer(query);
                Assert.That(budgetkontogruppe, Is.Not.Null);
                Assert.That(budgetkontogruppe.Nummer, Is.EqualTo(query.Nummer));

                var command = new BudgetkontogruppeModifyCommand
                                  {
                                      Nummer = budgetkontogruppe.Nummer,
                                      Navn = string.Format("_{0}", budgetkontogruppe.Navn)
                                  };
                var result = channel.BudgetkontogruppeModify(command);
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
        /// Tester, at der kastes en FaultException ved opdatering af gruppe til budgetkonti, hvis gruppen til budgetkonti ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetkontogruppeHvisBudgetkontogruppeIkkeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontogruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var budgetkontogruppe = channel.BudgetkontogruppeGetByNummer(query);
                Assert.That(budgetkontogruppe, Is.Not.Null);
                Assert.That(budgetkontogruppe.Nummer, Is.EqualTo(query.Nummer));

                var command = new BudgetkontogruppeModifyCommand
                                  {
                                      Nummer = -1,
                                      Navn = budgetkontogruppe.Navn
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontogruppeModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering af gruppe til budgetkonti, hvis navnet er tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringAfBudgetkontogruppeHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFinansstyringRepositoryService>("FinansstyringRepositoryService");
            try
            {
                var query = new BudgetkontogruppeGetByNummerQuery
                                {
                                    Nummer = 1
                                };
                var budgetkontogruppe = channel.BudgetkontogruppeGetByNummer(query);
                Assert.That(budgetkontogruppe, Is.Not.Null);
                Assert.That(budgetkontogruppe.Nummer, Is.EqualTo(query.Nummer));

                var command = new BudgetkontogruppeModifyCommand
                                  {
                                      Nummer = budgetkontogruppe.Nummer,
                                      Navn = string.Empty
                                  };
                Assert.Throws<FaultException>(() => channel.BudgetkontogruppeModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }
    }
}
