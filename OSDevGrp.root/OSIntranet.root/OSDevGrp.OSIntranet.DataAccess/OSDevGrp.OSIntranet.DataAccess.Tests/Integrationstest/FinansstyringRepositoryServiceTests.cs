﻿using System;
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
                Assert.That(regnskaber.Count, Is.GreaterThan(0));
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
                Assert.That(konti.Count, Is.GreaterThan(0));
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
                Assert.That(konti.Count, Is.GreaterThan(0));
                // Hent en given konto i regnskabet.
                var no = random.Next(0, konti.Count - 1);
                var query = new KontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = konti[no].Kontonummer
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
                Assert.That(budgetkonti.Count, Is.GreaterThan(0));
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
                Assert.That(budgetkonti.Count, Is.GreaterThan(0));
                // Hent en given konto i regnskabet.
                var no = random.Next(0, budgetkonti.Count - 1);
                var query = new BudgetkontoGetByRegnskabAndKontonummerQuery
                                {
                                    Regnskabsnummer = RegnskabsnummerTilTest,
                                    Kontonummer = budgetkonti[no].Kontonummer
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
                Assert.That(bogføringslinjer.Count, Is.GreaterThan(0));
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
                Assert.That(kontogrupper.Count, Is.GreaterThan(0));
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
                Assert.That(budgetkontogrupper.Count, Is.GreaterThan(0));
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
    }
}
