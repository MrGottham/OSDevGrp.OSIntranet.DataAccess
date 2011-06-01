using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
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
    /// Queryhandler til håndtering af forespørgelsen: FirmaGetByNummerQuery.
    /// </summary>
    public class FirmaGetByNummerQueryHandler : AdresseQueryHandlerBase, IQueryHandler<FirmaGetByNummerQuery, FirmaView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: FirmaGetByNummerQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public FirmaGetByNummerQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _adresseRepository = adresseRepository;
            _finansstyringRepository = finansstyringRepository;
            _fællesRepository = fællesRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<FirmaGetByNummerQuery,FirmaView> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter et givent firma.</param>
        /// <returns>Firma.</returns>
        public FirmaView Query(FirmaGetByNummerQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var getBrevhoved = new Func<int, Brevhoved>(nummer => _fællesRepository.BrevhovedGetByNummer(nummer));
            var regnskaber = _finansstyringRepository.RegnskabGetAll(getBrevhoved);
            Firma firma;
            try
            {
                firma = _adresseRepository.AdresseGetAll(adresse => MergeInformations(adresse, regnskaber))
                    .Where(m => m is Firma)
                    .Cast<Firma>()
                    .ToArray()
                    .Single(m => m.Nummer == query.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Firma), query.Nummer),
                    ex);
            }
            return _objectMapper.Map<Firma, FirmaView>(firma);
        }

        #endregion
    }
}
