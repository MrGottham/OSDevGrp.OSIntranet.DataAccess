using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
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

        /// <summary>
        /// Tilføjer en bogføringslinje.
        /// </summary>
        /// <param name="bogføringsdato">Bogføringsdato.</param>
        /// <param name="bilag">Bilagsnummer.</param>
        /// <param name="konto">Konto, hvorpå kontolinjen skal tilføjes.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkonto">Budgetkonto, hvorpå kontolinjen skal tilføjes.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        /// <param name="adresse">Adressen, hvorpå kontolinjen skal bogføres.</param>
        void BogføringslinjeAdd(DateTime bogføringsdato, string bilag, Konto konto, string tekst, Budgetkonto budgetkonto, decimal debit, decimal kredit, AdresseBase adresse);
    }
}
