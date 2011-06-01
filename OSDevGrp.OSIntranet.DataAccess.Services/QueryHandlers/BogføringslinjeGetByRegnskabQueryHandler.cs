using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
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
    /// Queryhandler til håndtering af forespørgelsen: BogføringslinjeGetByRegnskabQuery.
    /// </summary>
    public class BogføringslinjeGetByRegnskabQueryHandler : RegnskabQueryHandleBase, IQueryHandler<BogføringslinjeGetByRegnskabQuery, IEnumerable<BogføringslinjeView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IAdresseRepository _adresseRepository;
        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: BogføringslinjeGetByRegnskabQuery.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public BogføringslinjeGetByRegnskabQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _finansstyringRepository = finansstyringRepository;
            _adresseRepository = adresseRepository;
            _fællesRepository = fællesRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<BogføringslinjeGetByRegnskabQuery,IEnumerable<BogføringslinjeView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle bogføringslinjer i et givent regnskab.</param>
        /// <returns>Alle bogføringslinjer i et givent regnskab.</returns>
        public IEnumerable<BogføringslinjeView> Query(BogføringslinjeGetByRegnskabQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var adresser = _adresseRepository.AdresseGetAll();
            var getBrevhoved = new Func<int, Brevhoved>(nummer => _fællesRepository.BrevhovedGetByNummer(nummer));
            var regnskaber = _finansstyringRepository.RegnskabGetAll(getBrevhoved, r => MergeInformations(r, adresser));
            Regnskab regnskab;
            try
            {
                regnskab = regnskaber.Single(m => m.Nummer == query.Regnskabsnummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Regnskab),
                                                 query.Regnskabsnummer), ex);
            }
            var comparer = new BogføringslinjeComparer();
            var bogføringslinjer = regnskab.Konti
                .Where(m => m is Konto)
                .Cast<Konto>()
                .SelectMany(m => m.Bogføringslinjer)
                .OrderByDescending(m => m, comparer)
                .ToArray();
            return _objectMapper.Map<IEnumerable<Bogføringslinje>, IEnumerable<BogføringslinjeView>>(bogføringslinjer);
        }

        #endregion
    }
}
