using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
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

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repositoryservice for addressekartoteket.
        /// </summary>
        /// <param name="logRepository">Implementering af logging repository.</param>
        /// <param name="commandBus">Implementering af en CommandBus.</param>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public AdresseRepositoryService(ILogRepository logRepository, ICommandBus commandBus, IQueryBus queryBus)
            : base(logRepository)
        {
            if (commandBus == null)
            {
                throw new ArgumentNullException("commandBus");
            }
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region IAdresseRepositoryService Members

        /// <summary>
        /// Henter alle personer.
        /// </summary>
        /// <param name="personGetAllQuery">Query til forespørgelse efter alle personer.</param>
        /// <returns>Alle personer.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<PersonView> PersonGetAll(PersonGetAllQuery personGetAllQuery)
        {
            try
            {
                return _queryBus.Query<PersonGetAllQuery, IEnumerable<PersonView>>(personGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter en given person.
        /// </summary>
        /// <param name="personGetByNummerQuery">Query til forespørgelse efter en given person.</param>
        /// <returns>Person.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public PersonView PersonGetByNummer(PersonGetByNummerQuery personGetByNummerQuery)
        {
            try
            {
                return _queryBus.Query<PersonGetByNummerQuery, PersonView>(personGetByNummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle firmaer.
        /// </summary>
        /// <param name="firmaGetAllQuery">Query til forespørgelse efter alle firmaer.</param>
        /// <returns>Alle firmaer.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<FirmaView> FirmaGetAll(FirmaGetAllQuery firmaGetAllQuery)
        {
            try
            {
                return _queryBus.Query<FirmaGetAllQuery, IEnumerable<FirmaView>>(firmaGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter et givent firma.
        /// </summary>
        /// <param name="firmaGetByNummerQuery">Query til forespørgelse efter et givent firma.</param>
        /// <returns>Firma.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public FirmaView FirmaGetByNummer(FirmaGetByNummerQuery firmaGetByNummerQuery)
        {
            try
            {
                return _queryBus.Query<FirmaGetByNummerQuery, FirmaView>(firmaGetByNummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle adresser til en adresseliste.
        /// </summary>
        /// <param name="adresselisteGetAllQuery">Query til forespørgelse efter alle adresser til en adresseliste.</param>
        /// <returns>Alle adresser til en adresseliste.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<AdresselisteView> AdresselisteGetAll(AdresselisteGetAllQuery adresselisteGetAllQuery)
        {
            try
            {
                return _queryBus.Query<AdresselisteGetAllQuery, IEnumerable<AdresselisteView>>(adresselisteGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <param name="postnummerGetAllQuery">Query til forespørgelse efter alle postnumre.</param>
        /// <returns>Alle postnumre.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<PostnummerView> PostnummerGetAll(PostnummerGetAllQuery postnummerGetAllQuery)
        {
            try
            {
                return _queryBus.Query<PostnummerGetAllQuery, IEnumerable<PostnummerView>>(postnummerGetAllQuery);
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
        public IEnumerable<PostnummerView> PostnummerGetAllByLandekode(PostnummerGetByLandekodeQuery postnummerGetByLandekodeQuery)
        {
            try
            {
                return
                    _queryBus.Query<PostnummerGetByLandekodeQuery, IEnumerable<PostnummerView>>(
                        postnummerGetByLandekodeQuery);
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
        /// Tilføjer et postnummer.
        /// </summary>
        /// <param name="postnummerAddCommand">Command til tilføjelse af et postnummer.</param>
        [OperationBehavior(TransactionScopeRequired = false)]
        public void PostnummerAdd(PostnummerAddCommand postnummerAddCommand)
        {
            try
            {
                _commandBus.Publish(postnummerAddCommand);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Opdaterer et givent postnummer.
        /// </summary>
        /// <param name="postnummerModifyCommand">Command til opdatering af et givent postnummer.</param>
        [OperationBehavior(TransactionScopeRequired = false)]
        public void PostnummerModify(PostnummerModifyCommand postnummerModifyCommand)
        {
            try
            {
                _commandBus.Publish(postnummerModifyCommand);
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
        public IEnumerable<AdressegruppeView> AdressegruppeGetAll(AdressegruppeGetAllQuery adressegruppeGetAllQuery)
        {
            try
            {
                return
                    _queryBus.Query<AdressegruppeGetAllQuery, IEnumerable<AdressegruppeView>>(adressegruppeGetAllQuery);
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
        /// Tilføjer en adressegruppe.
        /// </summary>
        /// <param name="adressegruppeAddCommand">Command til tilføjelse af en adressegruppe.</param>
        [OperationBehavior(TransactionScopeRequired = false)]
        public void AdressegruppeAdd(AdressegruppeAddCommand adressegruppeAddCommand)
        {
            try
            {
                _commandBus.Publish(adressegruppeAddCommand);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Opdaterer en given adressegruppe.
        /// </summary>
        /// <param name="adressegruppeModifyCommand">Command til opdatering af en given adressegruppe.</param>
        [OperationBehavior(TransactionScopeRequired = false)]
        public void AdressegruppeModify(AdressegruppeModifyCommand adressegruppeModifyCommand)
        {
            try
            {
                _commandBus.Publish(adressegruppeModifyCommand);
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
        public IEnumerable<BetalingsbetingelseView> BetalingsbetingelseGetAll(BetalingsbetingelseGetAllQuery betalingsbetingelseGetAllQuery)
        {
            try
            {
                return
                    _queryBus.Query<BetalingsbetingelseGetAllQuery, IEnumerable<BetalingsbetingelseView>>(
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

        /// <summary>
        /// Tilføjer en betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseAddCommand">Command til tilføjelse af en betalingsbetingelse.</param>
        /// <returns>Tilføjet betalingsbetingelse.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BetalingsbetingelseView BetalingsbetingelseAdd(BetalingsbetingelseAddCommand betalingsbetingelseAddCommand)
        {
            try
            {
                return
                    _commandBus.Publish<BetalingsbetingelseAddCommand, BetalingsbetingelseView>(
                        betalingsbetingelseAddCommand);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        /// <summary>
        /// Opdaterer en given betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseModifyCommand">Command til opdatering af en given betalingsbetingelse.</param>
        /// <returns>Opdateret betalingsbetingelse.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BetalingsbetingelseView BetalingsbetingelseModify(BetalingsbetingelseModifyCommand betalingsbetingelseModifyCommand)
        {
            try
            {
                return
                    _commandBus.Publish<BetalingsbetingelseModifyCommand, BetalingsbetingelseView>(
                        betalingsbetingelseModifyCommand);
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
