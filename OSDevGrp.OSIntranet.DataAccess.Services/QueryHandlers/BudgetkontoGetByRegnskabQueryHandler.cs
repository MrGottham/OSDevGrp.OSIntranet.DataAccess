using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Queryhandler til håndtering af forespørgelsen: BudgetkontoGetByRegnskabQuery.
    /// </summary>
    public class BudgetkontoGetByRegnskabQueryHandler : RegnskabQueryHandleBase, IQueryHandler<BudgetkontoGetByRegnskabQuery, IList<BudgetkontoListeView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: BudgetkontoGetByRegnskabQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public BudgetkontoGetByRegnskabQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IObjectMapper objectMapper)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _finansstyringRepository = finansstyringRepository;
            _adresseRepository = adresseRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<BudgetkontoGetByRegnskabQuery,IList<BudgetkontoListeView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle budgetkonti i et givent regnskab.</param>
        /// <returns>Alle budgetkonti i et givent regnskab.</returns>
        public IList<BudgetkontoListeView> Query(BudgetkontoGetByRegnskabQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var adresser = _adresseRepository.AdresseGetAll();
            var regnskaber = _finansstyringRepository.RegnskabGetAll(r => MergeInformations(r, adresser));
            Regnskab regnskab;
            try
            {
                regnskab = regnskaber.Single(m => m.Nummer == query.Regnskabsnummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Regnskab),
                                                 query.Regnskabsnummer), ex);
            }
            var konti = regnskab.Konti
                .Where(m => m is Budgetkonto)
                .Cast<Budgetkonto>()
                .ToArray();
            return _objectMapper.Map<IList<Budgetkonto>, IList<BudgetkontoListeView>>(konti);
        }

        #endregion
    }
}
