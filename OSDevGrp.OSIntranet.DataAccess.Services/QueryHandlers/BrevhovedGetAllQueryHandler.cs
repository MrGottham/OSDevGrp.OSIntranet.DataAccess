using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Queryhandler til håndtering af forespørgelsen: BrevhovedGetAllQuery.
    /// </summary>
    public class BrevhovedGetAllQueryHandler : IQueryHandler<BrevhovedGetAllQuery, IEnumerable<BrevhovedView>>
    {
        #region Private variables

        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: BrevhovedGetAllQuery.
        /// </summary>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public BrevhovedGetAllQueryHandler(IFællesRepository fællesRepository, IObjectMapper objectMapper)
        {
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _fællesRepository = fællesRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region IQueryHandler<BrevhovedGetAllQuery,IEnumerable<BrevhovedView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle brevhoveder.</param>
        /// <returns>Alle brevhoveder.</returns>
        public IEnumerable<BrevhovedView> Query(BrevhovedGetAllQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var brevhoveder = _fællesRepository.BrevhovedGetAll();
            return _objectMapper.Map<IEnumerable<Brevhoved>, IEnumerable<BrevhovedView>>(brevhoveder);
        }

        #endregion
    }
}
