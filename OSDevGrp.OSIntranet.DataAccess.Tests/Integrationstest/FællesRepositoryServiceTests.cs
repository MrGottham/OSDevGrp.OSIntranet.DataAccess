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

        /// <summary>
        /// Tester, at et brevhoved oprettes.
        /// </summary>
        [Test]
        [Ignore("Oprettelse af brevhoved er testet.")]
        public void TestAtBrevhovedOprettes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFællesRepositoryService>("FællesRepositoryService");
            try
            {
                var command = new BrevhovedAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = "_Test",
                                      Linje1 = "_Test 1",
                                      Linje2 = "_Test 2",
                                      Linje3 = "_Test 3",
                                      Linje4 = "_Test 4",
                                      Linje5 = "_Test 5",
                                      Linje6 = "_Test 6",
                                      Linje7 = "_Test 7",
                                      CvrNr = "_Test CVR"
                                  };
                var result = channel.BrevhovedAdd(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.Linje1, Is.EqualTo(command.Linje1));
                Assert.That(result.Linje2, Is.EqualTo(command.Linje2));
                Assert.That(result.Linje3, Is.EqualTo(command.Linje3));
                Assert.That(result.Linje4, Is.EqualTo(command.Linje4));
                Assert.That(result.Linje5, Is.EqualTo(command.Linje5));
                Assert.That(result.Linje6, Is.EqualTo(command.Linje6));
                Assert.That(result.Linje7, Is.EqualTo(command.Linje7));
                Assert.That(result.CvrNr, Is.EqualTo(command.CvrNr));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }
        
        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis brevhovedet allerede findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisBrevhovedAlleredeFindes()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFællesRepositoryService>("FællesRepositoryService");
            try
            {
                var command = new BrevhovedAddCommand
                                  {
                                      Nummer = 1,
                                      Navn = "_Test",
                                      Linje1 = "_Test 1",
                                      Linje2 = "_Test 2",
                                      Linje3 = "_Test 3",
                                      Linje4 = "_Test 4",
                                      Linje5 = "_Test 5",
                                      Linje6 = "_Test 6",
                                      Linje7 = "_Test 7",
                                  };
                Assert.Throws<FaultException>(() => channel.BrevhovedAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved oprettelse, hvis navnet et tomt.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOprettelseHvisNavnErEmpty()
        {
            var container = ContainerFactory.Create();
            var channelFactory = container.Resolve<IChannelFactory>();
            var channel = channelFactory.CreateChannel<IFællesRepositoryService>("FællesRepositoryService");
            try
            {
                var command = new BrevhovedAddCommand
                                  {
                                      Nummer = 99,
                                      Navn = string.Empty,
                                      Linje1 = "_Test 1",
                                      Linje2 = "_Test 2",
                                      Linje3 = "_Test 3",
                                      Linje4 = "_Test 4",
                                      Linje5 = "_Test 5",
                                      Linje6 = "_Test 6",
                                      Linje7 = "_Test 7",
                                  };
                Assert.Throws<FaultException>(() => channel.BrevhovedAdd(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at brevhoved opdateres.
        /// </summary>
        [Test]
        [Ignore("Opdatering af brevhoveder er testet.")]
        public void TestAtBrevhovedOpdateres()
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

                var brevhoved = brevhoveder.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(brevhoved, Is.Not.Null);

                var command = new BrevhovedModifyCommand
                                  {
                                      Nummer = brevhoved.Nummer,
                                      Navn = string.Format("_{0}", brevhoved.Navn),
                                      Linje1 = brevhoved.Linje1,
                                      Linje2 = brevhoved.Linje2,
                                      Linje3 = brevhoved.Linje3,
                                      Linje4 = brevhoved.Linje4,
                                      Linje5 = brevhoved.Linje5,
                                      Linje6 = brevhoved.Linje6,
                                      Linje7 = brevhoved.Linje7,
                                      CvrNr = brevhoved.CvrNr
                                  };
                var result = channel.BrevhovedModify(command);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Nummer, Is.EqualTo(command.Nummer));
                Assert.That(result.Navn, Is.Not.Null);
                Assert.That(result.Navn, Is.EqualTo(command.Navn));
                Assert.That(result.Linje1, Is.EqualTo(command.Linje1));
                Assert.That(result.Linje2, Is.EqualTo(command.Linje2));
                Assert.That(result.Linje3, Is.EqualTo(command.Linje3));
                Assert.That(result.Linje4, Is.EqualTo(command.Linje4));
                Assert.That(result.Linje5, Is.EqualTo(command.Linje5));
                Assert.That(result.Linje6, Is.EqualTo(command.Linje6));
                Assert.That(result.Linje7, Is.EqualTo(command.Linje7));
                Assert.That(result.CvrNr, Is.EqualTo(command.CvrNr));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

        /// <summary>
        /// Tester, at der kastes en FaultException ved opdatering, hvis brevhovedet ikke findes.
        /// </summary>
        [Test]
        public void TestAtFaultExceptionKastesVedOpdateringHvisBrevhovedIkkeFindes()
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

                var brevhoved = brevhoveder.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(brevhoved, Is.Not.Null);

                var command = new BrevhovedModifyCommand
                                  {
                                      Nummer = -1,
                                      Navn = string.Format("_{0}", brevhoved.Navn),
                                      Linje1 = brevhoved.Linje1,
                                      Linje2 = brevhoved.Linje2,
                                      Linje3 = brevhoved.Linje3,
                                      Linje4 = brevhoved.Linje4,
                                      Linje5 = brevhoved.Linje5,
                                      Linje6 = brevhoved.Linje6,
                                      Linje7 = brevhoved.Linje7,
                                  };
                Assert.Throws<FaultException>(() => channel.BrevhovedModify(command));
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
        public void TestAtFaultExceptionKastesVedOpdateringHvisNavnErEmpty()
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

                var brevhoved = brevhoveder.SingleOrDefault(m => m.Nummer == 1);
                Assert.That(brevhoved, Is.Not.Null);

                var command = new BrevhovedModifyCommand
                                  {
                                      Nummer = brevhoved.Nummer,
                                      Navn = string.Empty,
                                      Linje1 = brevhoved.Linje1,
                                      Linje2 = brevhoved.Linje2,
                                      Linje3 = brevhoved.Linje3,
                                      Linje4 = brevhoved.Linje4,
                                      Linje5 = brevhoved.Linje5,
                                      Linje6 = brevhoved.Linje6,
                                      Linje7 = brevhoved.Linje7,
                                  };
                Assert.Throws<FaultException>(() => channel.BrevhovedModify(command));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }
    }
}
