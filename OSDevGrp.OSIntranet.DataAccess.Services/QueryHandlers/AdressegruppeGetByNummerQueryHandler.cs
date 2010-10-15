using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
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
    /// Queryhandler til håndtering af forespørgelsen: AdressegruppeGetByNummerQuery.
    /// </summary>
    public class AdressegruppeGetByNummerQueryHandler : IQueryHandler<AdressegruppeGetByNummerQuery, AdressegruppeView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: AdressegruppeGetByNummerQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public AdressegruppeGetByNummerQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<AdressegruppeGetByNummerQuery,AdressegruppeView> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter en given adressegruppe.</param>
        /// <returns>Adressegruppe.</returns>
        public AdressegruppeView Query(AdressegruppeGetByNummerQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            Adressegruppe adressegruppe;
            try
            {
                adressegruppe = _adresseRepository.AdressegruppeGetAll().Single(m => m.Nummer == query.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Adressegruppe),
                                                 query.Nummer), ex);
            }
            return _objectMapper.Map<Adressegruppe, AdressegruppeView>(adressegruppe);
        }

        #endregion
    }
}
