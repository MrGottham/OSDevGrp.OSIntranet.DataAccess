using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Interface til repository for finansstyring.
    /// </summary>
    public interface IFinansstyringRepository : IRepository
    {
        /// <summary>
        /// Henter alle regnskaber inklusiv konti, budgetkonti m.m.
        /// </summary>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        IList<Regnskab> RegnskabGetAll();

        /// <summary>
        /// Henter alle regnskaber inklusiv konti, budgetkonti m.m.
        /// </summary>
        /// <param name="callback">Callbackmetode, til behandling af de enkelte regnskaber.</param>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        IList<Regnskab> RegnskabGetAll(Action<Regnskab> callback);

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle kontogrupper.</returns>
        IList<Kontogruppe> KontogruppeGetAll();

        /// <summary>
        /// Henter alle budgetkontogrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle budgetkontogrupper.</returns>
        IList<Budgetkontogruppe> BudgetkontogrupperGetAll();
    }
}
