using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Queryhandler til håndtering af forespørgelsen: PostnummerGetAllQuery.
    /// </summary>
    public class PostnummerGetAllQueryHandler : IQueryHandler<PostnummerGetAllQuery, IList<PostnummerView>>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: PostnummerGetAllQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public PostnummerGetAllQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<PostnummerGetAllQuery,IList<PostnummerView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle postnumre.</param>
        /// <returns>Alle postnumre.</returns>
        public IList<PostnummerView> Query(PostnummerGetAllQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var postnumre = _adresseRepository.PostnummerGetAll();
            return _objectMapper.Map<IList<Postnummer>, IList<PostnummerView>>(postnumre);
        }

        #endregion
    }
}
