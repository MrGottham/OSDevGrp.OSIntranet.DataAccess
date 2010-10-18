using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Queryhandler til håndtering af forespørgelsen: PostnummerGetByLandekodeQuery.
    /// </summary>
    public class PostnummerGetByLandekodeQueryHandler : IQueryHandler<PostnummerGetByLandekodeQuery, IList<PostnummerView>>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: PostnummerGetByLandekodeQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public PostnummerGetByLandekodeQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _adresseRepository = adresseRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<PostnummerGetByLandekodeQuery,IList<PostnummerView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle postnumre på en given landekode.</param>
        /// <returns>Alle postnumre for en given landekode.</returns>
        public IList<PostnummerView> Query(PostnummerGetByLandekodeQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var postnumre = _adresseRepository.PostnummerGetAll()
                .Where(m => m.Landekode.CompareTo(query.Landekode) == 0)
                .ToArray();
            return _objectMapper.Map<IList<Postnummer>, IList<PostnummerView>>(postnumre);
        }

        #endregion
    }
}
