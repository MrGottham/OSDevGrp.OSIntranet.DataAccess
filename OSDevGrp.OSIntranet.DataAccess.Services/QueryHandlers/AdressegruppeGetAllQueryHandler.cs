using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Queryhandler til håndtering af forespørgelsen: AdressegruppeGetAllQuery.
    /// </summary>
    public class AdressegruppeGetAllQueryHandler : IQueryHandler<AdressegruppeGetAllQuery, IList<AdressegruppeView>>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Queryhandler til håndtering af forespørgelsen: AdressegruppeGetAllQuery.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartoteket.</param>
        public AdressegruppeGetAllQueryHandler(IAdresseRepository adresseRepository)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            _adresseRepository = adresseRepository;
        }

        #endregion

        #region IQueryHandler<AdressegruppeGetAllQuery,IList<AdressegruppeView>> Members

        /// <summary>
        /// Udfører forespørgelse.
        /// </summary>
        /// <param name="query">Forespørgelse efter alle adressegrupper.</param>
        /// <returns>Alle adressegrupper.</returns>
        public IList<AdressegruppeView> Query(AdressegruppeGetAllQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            var adressegrupper = _adresseRepository.AdressegruppeGetAll();

            throw new System.NotImplementedException();
        }

        #endregion
    }
}
