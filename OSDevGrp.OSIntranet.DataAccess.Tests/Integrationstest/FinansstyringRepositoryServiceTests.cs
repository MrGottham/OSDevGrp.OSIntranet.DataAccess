using System;
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
                var query = new BudgetkontogruppeGetByNummerQuery()
                                {
                                    Nummer = -1
                                };
                Assert.Throws<FaultException>(() => channel.BudgetkontoGetByNummer(query));
            }
            finally
            {
                ChannelTools.CloseChannel(channel);
            }
        }

    }
}
