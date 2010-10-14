using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Queryhandler til håndtering af forespørgelsen: AdressegruppeGetAllQuery.
    /// </summary>
    public class AdressegruppeGetAllQueryHandler : IQueryHandler<AdressegruppeGetAllQuery, IList<AdressegruppeView>>
    {
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
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
