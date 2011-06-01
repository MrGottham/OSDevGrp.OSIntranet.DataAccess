using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Queryhandler til håndtering af forespørgelsen: AdresselisteGetAllQuery.
    /// </summary>
    public class AdresselisteGetAllQueryHandler : AdresseQueryHandlerBase, IQueryHandler<AdresselisteGetAllQuery, IEnumerable<AdresselisteView>>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: AdresselisteGetAllQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public AdresselisteGetAllQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper, IFællesRepository fællesRepository)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            _adresseRepository = adresseRepository;
            _finansstyringRepository = finansstyringRepository;
            _objectMapper = objectMapper;
            _fællesRepository = fællesRepository;
        }

        #endregion

        #region IQueryHandler<AdresselisteGetAllQuery,IEnumerable<AdresselisteView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle adresser til en adresseliste.</param>
        /// <returns>Alle adresser til en adresseliste.</returns>
        public IEnumerable<AdresselisteView> Query(AdresselisteGetAllQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var getBrevhoved = new Func<int, Brevhoved>(nummer => _fællesRepository.BrevhovedGetByNummer(nummer));
            var regnskaber = _finansstyringRepository.RegnskabGetAll(getBrevhoved);
            var adresser = _adresseRepository.AdresseGetAll(adresse => MergeInformations(adresse, regnskaber));
            return _objectMapper.Map<IEnumerable<AdresseBase>, IEnumerable<AdresselisteView>>(adresser);
        }

        #endregion
    }
}
