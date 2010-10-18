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
    /// Queryhandler til håndtering af forespørgelsen: BynavnGetByLandekodeAndPostnummerQuery.
    /// </summary>
    public class BynavnGetByLandekodeAndPostnummerQueryHandler : IQueryHandler<BynavnGetByLandekodeAndPostnummerQuery, PostnummerView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner queryhandler til håndtering af forespørgelsen: BynavnGetByLandekodeAndPostnummerQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        /// <param name="objectMapper">Implementering af objektmapper.</param>
        public BynavnGetByLandekodeAndPostnummerQueryHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region IQueryHandler<BynavnGetByLandekodeAndPostnummerQuery,PostnummerView> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter et bynavn for et givent postnummer på en given landekode.</param>
        /// <returns>Landekode, postnummer og bynavn.</returns>
        public PostnummerView Query(BynavnGetByLandekodeAndPostnummerQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            Postnummer postnummer;
            try
            {
                postnummer = _adresseRepository.PostnummerGetAll()
                    .Single(m => m.Landekode.CompareTo(query.Landekode) == 0 && m.Postnr.CompareTo(query.Postnummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Postnummer),
                                                 string.Format("{0}-{1}", query.Landekode, query.Postnummer)), ex);
            }
            return _objectMapper.Map<Postnummer, PostnummerView>(postnummer);
        }

        #endregion
    }
}
