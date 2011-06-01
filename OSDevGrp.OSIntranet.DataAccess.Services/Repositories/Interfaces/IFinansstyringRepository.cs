using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;

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
        /// <param name="getBrevhoved">Callbackmetode til hentning af brevhoved.</param>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        IEnumerable<Regnskab> RegnskabGetAll(Func<int, Brevhoved> getBrevhoved);

        /// <summary>
        /// Henter alle regnskaber inklusiv konti, budgetkonti m.m.
        /// </summary>
        /// <param name="getBrevhoved">Callbackmetode til hentning af brevhoved.</param>
        /// <param name="callback">Callbackmetode, til behandling af de enkelte regnskaber.</param>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        IEnumerable<Regnskab> RegnskabGetAll(Func<int, Brevhoved> getBrevhoved, Action<Regnskab> callback);

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle kontogrupper.</returns>
        IEnumerable<Kontogruppe> KontogruppeGetAll();

        /// <summary>
        /// Henter alle budgetkontogrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle budgetkontogrupper.</returns>
        IEnumerable<Budgetkontogruppe> BudgetkontogrupperGetAll();

        /// <summary>
        /// Tilføjer et regnskab.
        /// </summary>
        /// <param name="nummer">Nummer på regnskabet.</param>
        /// <param name="navn">Navn på regnskabet.</param>
        /// <param name="brevhoved">Brevhoved til regnskabet.</param>
        void RegnskabAdd(int nummer, string navn, Brevhoved brevhoved);

        /// <summary>
        /// Opdaterer et givent regnskab.
        /// </summary>
        /// <param name="nummer">Nummer på regnskabet.</param>
        /// <param name="navn">Navn på regnskabet.</param>
        /// <param name="brevhoved">Brevhoved til regnskabet.</param>
        void RegnskabModify(int nummer, string navn, Brevhoved brevhoved);

        /// <summary>
        /// Tilføjer en konto til et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, som kontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppe">Kontogruppe.</param>
        void KontoAdd(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Kontogruppe kontogruppe);

        /// <summary>
        /// Opdaterer en konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, hvori kontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppe">Kontogruppe.</param>
        void KontoModify(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Kontogruppe kontogruppe);

        /// <summary>
        /// Tilføjer en budgetkonto til et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, som kontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse</param>
        /// <param name="notat">Notat.</param>
        /// <param name="budgetkontogruppe">Budgetkontogruppe.</param>
        void BudgetkontoAdd(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Budgetkontogruppe budgetkontogruppe);

        /// <summary>
        /// Opdaterer en budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, hvori kontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="budgetkontogruppe">Budgetkontogruppe.</param>
        void BudgetkontoModify(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Budgetkontogruppe budgetkontogruppe);

        /// <summary>
        /// Opdaterer eller tilføjer kreditoplysninger til en given konto.
        /// </summary>
        /// <param name="konto">Konto, hvorpå kreditoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal.</param>
        /// <param name="måned">Måned.</param>
        /// <param name="kredit">Kredit.</param>
        void KreditoplysningerModifyOrAdd(Konto konto, int år, int måned, decimal kredit);

        /// <summary>
        /// Opdaterer eller tilføjer budgetoplysninger til en given budgetkonto.
        /// </summary>
        /// <param name="budgetkonto">Budgetkonto, hvorpå budgetoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal.</param>gi
        /// <param name="måned">Måned.</param>
        /// <param name="indtægter">Indtægter.</param>
        /// <param name="udgifter">Udgifter.</param>
        void BudgetoplysningerModifyOrAdd(Budgetkonto budgetkonto, int år, int måned, decimal indtægter, decimal udgifter);
        
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

        /// <summary>
        /// Tilføjer en kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Typen for kontogruppen.</param>
        void KontogruppeAdd(int nummer, string navn, KontogruppeType kontogruppeType);

        /// <summary>
        /// Opdaterer en given kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Typen for kontogruppen.</param>
        void KontogruppeModify(int nummer, string navn, KontogruppeType kontogruppeType);

        /// <summary>
        /// Tilføjer en gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <param name="navn">Navn på gruppen til budgetkonti.</param>
        void BudgetkontogruppeAdd(int nummer, string navn);

        /// <summary>
        /// Opdaterer en given gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <param name="navn">Navn på gruppen til budgetkonti.</param>
        void BudgetkontogruppeModify(int nummer, string navn);
    }
}
