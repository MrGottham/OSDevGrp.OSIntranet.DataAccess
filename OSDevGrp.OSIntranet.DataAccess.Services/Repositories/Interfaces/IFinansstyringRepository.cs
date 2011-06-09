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
        /// Tilføjer og returnerer et regnskab.
        /// </summary>
        /// <param name="getBrevhoved">Callbackmetode til hentning af brevhoved.</param>
        /// <param name="nummer">Nummer på regnskabet.</param>
        /// <param name="navn">Navn på regnskabet.</param>
        /// <param name="brevhoved">Brevhoved til regnskabet.</param>
        /// <returns>Det tilføjede regnskab.</returns>
        Regnskab RegnskabAdd(Func<int, Brevhoved> getBrevhoved, int nummer, string navn, Brevhoved brevhoved);

        /// <summary>
        /// Opdaterer og returnerer et givent regnskab.
        /// </summary>
        /// <param name="getBrevhoved">Callbackmetode til hentning af brevhoved.</param>
        /// <param name="nummer">Nummer på regnskabet.</param>
        /// <param name="navn">Navn på regnskabet.</param>
        /// <param name="brevhoved">Brevhoved til regnskabet.</param>
        /// <returns>Det opdaterede regnskab.</returns>
        Regnskab RegnskabModify(Func<int, Brevhoved> getBrevhoved, int nummer, string navn, Brevhoved brevhoved);

        /// <summary>
        /// Tilføjer og returnerer en konto til et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, som kontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppe">Kontogruppe.</param>
        /// <returns>Den tilføjede konto.</returns>
        Konto KontoAdd(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Kontogruppe kontogruppe);

        /// <summary>
        /// Opdaterer og returnerer en konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, hvori kontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppe">Kontogruppe.</param>
        /// <returns>Den opdaterede konto.</returns>
        Konto KontoModify(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Kontogruppe kontogruppe);

        /// <summary>
        /// Tilføjer og returnerer en budgetkonto til et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, som kontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse</param>
        /// <param name="notat">Notat.</param>
        /// <param name="budgetkontogruppe">Budgetkontogruppe.</param>
        /// <returns>Den tilføjede budgetkonto.</returns>
        Budgetkonto BudgetkontoAdd(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Budgetkontogruppe budgetkontogruppe);

        /// <summary>
        /// Opdaterer og returnerer en budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, hvori kontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="budgetkontogruppe">Budgetkontogruppe.</param>
        /// <returns>Den opdaterede budgetkonto.</returns>
        Budgetkonto BudgetkontoModify(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Budgetkontogruppe budgetkontogruppe);

        /// <summary>
        /// Opdaterer eller tilføjer kreditoplysninger til en given konto.
        /// </summary>
        /// <param name="konto">Konto, hvorpå kreditoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal.</param>
        /// <param name="måned">Måned.</param>
        /// <param name="kredit">Kredit.</param>
        /// <returns>De opdaterede eller tilføjede kreditoplysninger.</returns>
        Kreditoplysninger KreditoplysningerModifyOrAdd(Konto konto, int år, int måned, decimal kredit);

        /// <summary>
        /// Opdaterer eller tilføjer budgetoplysninger til en given budgetkonto.
        /// </summary>
        /// <param name="budgetkonto">Budgetkonto, hvorpå budgetoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal.</param>gi
        /// <param name="måned">Måned.</param>
        /// <param name="indtægter">Indtægter.</param>
        /// <param name="udgifter">Udgifter.</param>
        /// <returns>De opdaterede eller tilføjede budgetoplysninger.</returns>
        Budgetoplysninger BudgetoplysningerModifyOrAdd(Budgetkonto budgetkonto, int år, int måned, decimal indtægter, decimal udgifter);
        
        /// <summary>
        /// Tilføjer og returnerer en bogføringslinje.
        /// </summary>
        /// <param name="bogføringsdato">Bogføringsdato.</param>
        /// <param name="bilag">Bilagsnummer.</param>
        /// <param name="konto">Konto, hvorpå kontolinjen skal tilføjes.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkonto">Budgetkonto, hvorpå kontolinjen skal tilføjes.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        /// <param name="adresse">Adressen, hvorpå kontolinjen skal bogføres.</param>
        /// <returns>Den tilføjede bogføringslinje.</returns>
        Bogføringslinje BogføringslinjeAdd(DateTime bogføringsdato, string bilag, Konto konto, string tekst, Budgetkonto budgetkonto, decimal debit, decimal kredit, AdresseBase adresse);

        /// <summary>
        /// Tilføjer og returnerer en kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Typen for kontogruppen.</param>
        /// <returns>Den tilføjede kontogruppe.</returns>
        Kontogruppe KontogruppeAdd(int nummer, string navn, KontogruppeType kontogruppeType);

        /// <summary>
        /// Opdaterer og returnerer en given kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Typen for kontogruppen.</param>
        /// <returns>Den opdaterede kontogruppe.</returns>
        Kontogruppe KontogruppeModify(int nummer, string navn, KontogruppeType kontogruppeType);

        /// <summary>
        /// Tilføjer og returnerer en gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <param name="navn">Navn på gruppen til budgetkonti.</param>
        /// <returns>Den tilføjede gruppe til budgetkonti.</returns>
        Budgetkontogruppe BudgetkontogruppeAdd(int nummer, string navn);

        /// <summary>
        /// Opdaterer og returnerer en given gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <param name="navn">Navn på gruppen til budgetkonti.</param>
        /// <returns>Den opdaterede gruppe til budgetkonti.</returns>
        Budgetkontogruppe BudgetkontogruppeModify(int nummer, string navn);
    }
}
