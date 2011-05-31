using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Queryhandler til håndtering af forespørgelsen: RegnskabGetAllQueryHandler.
    /// </summary>
    public class RegnskabGetAllQueryHandler : RegnskabQueryHandleBase, IQueryHandler<RegnskabGetAllQuery, IList<RegnskabListeView>>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: RegnskabGetAllQueryHandler.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public RegnskabGetAllQueryHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<RegnskabGetAllQuery,IList<RegnskabListeView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle regnskaber.</param>
        /// <returns>Alle regnskaber.</returns>
        public IList<RegnskabListeView> Query(RegnskabGetAllQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var adresser = _adresseRepository.AdresseGetAll();
            var regnskaber = _finansstyringRepository.RegnskabGetAll(r => MergeInformations(r, adresser));
            return _objectMapper.Map<IEnumerable<Regnskab>, IList<RegnskabListeView>>(regnskaber);
        }

        #endregion
    }
}
