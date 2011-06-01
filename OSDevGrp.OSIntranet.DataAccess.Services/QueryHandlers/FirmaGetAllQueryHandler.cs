using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Queryhandler til håndtering af forespørgelsen: FirmaGetAllQuery.
    /// </summary>
    public class FirmaGetAllQueryHandler : AdresseQueryHandlerBase, IQueryHandler<FirmaGetAllQuery, IEnumerable<FirmaView>>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: FirmaGetAllQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public FirmaGetAllQueryHandler(IAdresseRepository adresseRepository, IFinansstyringRepository finansstyringRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<FirmaGetAllQuery,IEnumerable<FirmaView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle firmaer.</param>
        /// <returns>Alle firmaer.</returns>
        public IEnumerable<FirmaView> Query(FirmaGetAllQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var getBrevhoved = new Func<int, Brevhoved>(nummer => _fællesRepository.BrevhovedGetByNummer(nummer));
            var regnskaber = _finansstyringRepository.RegnskabGetAll(getBrevhoved);
            var firmaer = _adresseRepository.AdresseGetAll(adresse => MergeInformations(adresse, regnskaber))
                .Where(m => m is Firma)
                .Cast<Firma>()
                .ToArray();
            return _objectMapper.Map<IEnumerable<Firma>, IEnumerable<FirmaView>>(firmaer);
        }

        #endregion
    }
}
