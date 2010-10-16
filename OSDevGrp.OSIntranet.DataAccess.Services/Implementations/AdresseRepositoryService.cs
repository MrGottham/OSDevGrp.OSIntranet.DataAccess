using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Implementations
{
    /// <summary>
    /// Repositoryservice for adressekartoteket.
    /// </summary>
    public class AdresseRepositoryService : RepositoryServiceBase, IAdresseRepositoryService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repositoryservice for addressekartoteket.
        /// </summary>
        /// <param name="logRepository">Implementering af logging repository.</param>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public AdresseRepositoryService(ILogRepository logRepository, IQueryBus queryBus)
            : base(logRepository)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion

        #region IAdresseRepositoryService Members

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <param name="postnummerGetAllQuery">Query til forespørgelse efter alle postnumre.</param>
        /// <returns>Alle postnumre.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IList<PostnummerView> PostnummerGetAll(PostnummerGetAllQuery postnummerGetAllQuery)
        {
            try
            {
                return _queryBus.Query<PostnummerGetAllQuery, IList<PostnummerView>>(postnummerGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle postnumre for en given landekode.
        /// </summary>
        /// <param name="postnummerGetByLandekodeQuery">Query til forespørgelse efter alle postnumre for en given landekode.</param>
        /// <returns>Alle postnumre for den givne landekode.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IList<PostnummerView> PostnummerGetAllByLandekode(PostnummerGetByLandekodeQuery postnummerGetByLandekodeQuery)
        {
            try
            {
                return
                    _queryBus.Query<PostnummerGetByLandekodeQuery, IList<PostnummerView>>(postnummerGetByLandekodeQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter bynavnet til et givent postnummer på en given landekode.
        /// </summary>
        /// <param name="bynavnGetByLandekodeAndPostnummerQuery">Query til forespørgelse efter bynavnet for et givent postnummer på en given landekode.</param>
        /// <returns>Landekode, postnummer og bynavn</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public PostnummerView BynavnGetByLandekodeAndPostnummre(BynavnGetByLandekodeAndPostnummerQuery bynavnGetByLandekodeAndPostnummerQuery)
        {
            try
            {
                return
                    _queryBus.Query<BynavnGetByLandekodeAndPostnummerQuery, PostnummerView>(
                        bynavnGetByLandekodeAndPostnummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <param name="adressegruppeGetAllQuery">Query til forespørgelse efter alle adressegrupper.</param>
        /// <returns>Alle adressegrupper.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IList<AdressegruppeView> AdressegruppeGetAll(AdressegruppeGetAllQuery adressegruppeGetAllQuery)
        {
            try
            {
                return _queryBus.Query<AdressegruppeGetAllQuery, IList<AdressegruppeView>>(adressegruppeGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter en given adressegruppe.
        /// </summary>
        /// <param name="adressegruppeGetByNummerQuery">Query til forespørgelse efter en given adressegruppe.</param>
        /// <returns>Adressegruppe.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public AdressegruppeView AdressegruppeGetByNummer(AdressegruppeGetByNummerQuery adressegruppeGetByNummerQuery)
        {
            try
            {
                return _queryBus.Query<AdressegruppeGetByNummerQuery, AdressegruppeView>(adressegruppeGetByNummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <param name="betalingsbetingelseGetAllQuery">Query til forespørgelse efter alle betalingsbetingelser.</param>
        /// <returns>Alle betalingsbetingelser.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IList<BetalingsbetingelseView> BetalingsbetingelseGetAll(BetalingsbetingelseGetAllQuery betalingsbetingelseGetAllQuery)
        {
            try
            {
                return
                    _queryBus.Query<BetalingsbetingelseGetAllQuery, IList<BetalingsbetingelseView>>(
                        betalingsbetingelseGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter en given betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseGetByNummerQuery">Query til forespørgelse efter en given betalingsbetingelse.</param>
        /// <returns>Betalingsbetingelse.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BetalingsbetingelseView BetalingsbetingelseGetByNummer(BetalingsbetingelseGetByNummerQuery betalingsbetingelseGetByNummerQuery)
        {
            try
            {
                return
                    _queryBus.Query<BetalingsbetingelseGetByNummerQuery, BetalingsbetingelseView>(
                        betalingsbetingelseGetByNummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        #endregion
    }
}
