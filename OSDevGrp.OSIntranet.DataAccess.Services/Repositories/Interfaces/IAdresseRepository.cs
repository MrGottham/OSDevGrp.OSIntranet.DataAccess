using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Interface til repository for adressekartoteket.
    /// </summary>
    public interface IAdresseRepository : IRepository
    {
        /// <summary>
        /// Henter alle adresser.
        /// </summary>
        /// <returns>Alle adresser.</returns>
        IEnumerable<AdresseBase> AdresseGetAll();

        /// <summary>
        /// Henter alle adresser.
        /// </summary>
        /// <param name="callback">Callbackmetode, til behandling af de enkelte adresser.</param>
        /// <returns>Alle adresser.</returns>
        IEnumerable<AdresseBase> AdresseGetAll(Action<AdresseBase> callback);

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <returns>Liste indeholdende alle postnumre.</returns>
        IEnumerable<Postnummer> PostnummerGetAll();

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle adressegrupper.</returns>
        IEnumerable<Adressegruppe> AdressegruppeGetAll();

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <returns>Liste indeholdende alle betalingsbetingelser.</returns>
        IEnumerable<Betalingsbetingelse> BetalingsbetingelserGetAll();

        /// <summary>
        /// TIlføjer en person.
        /// </summary>
        /// <param name="navn">Navn på personen.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="telefon">Telefonnummer.</param>
        /// <param name="mobil">Mobilnummer.</param>
        /// <param name="fødselsdato">Fødselsdato.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        /// <param name="firma">Firmatilknytning.</param>
        void PersonAdd(string navn, string adresse1, string adresse2, string postnrBy, string telefon, string mobil, DateTime? fødselsdato, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel, Firma firma);

        /// <summary>
        /// Opdaterer en given person.
        /// </summary>
        /// <param name="nummer">Unik identifikation af personen.</param>
        /// <param name="navn">Navn på personen.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="telefon">Telefonnummer.</param>
        /// <param name="mobil">Mobilnummer.</param>
        /// <param name="fødselsdato">Fødselsdato.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        /// <param name="firma">Firmatilknytning.</param>
        void PersonModify(int nummer, string navn, string adresse1, string adresse2, string postnrBy, string telefon, string mobil, DateTime? fødselsdato, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel, Firma firma);

        /// <summary>
        /// TIlføjer et firmat.
        /// </summary>
        /// <param name="navn">Navn på firmaet.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="telefon1">Primært telefonnummer.</param>
        /// <param name="telefon2">Sekundært telefonnummer.</param>
        /// <param name="telefax">Telefax.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        void FirmaAdd(string navn, string adresse1, string adresse2, string postnrBy, string telefon1, string telefon2, string telefax, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel);

        /// <summary>
        /// Opdaterer et givent firma.
        /// </summary>
        /// <param name="nummer">Unik identifikation af firmaet.</param>
        /// <param name="navn">Navn på firmaet.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="telefon1">Primært telefonnummer.</param>
        /// <param name="telefon2">Sekundært telefonnummer.</param>
        /// <param name="telefax">Telefax.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        void FirmaModify(int nummer, string navn, string adresse1, string adresse2, string postnrBy, string telefon1, string telefon2, string telefax, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel);

        /// <summary>
        /// Tilføjer et postnummer.
        /// </summary>
        /// <param name="landekode">Landekode.</param>
        /// <param name="postnr">Postnummer.</param>
        /// <param name="by">Bynavn.</param>
        void PostnummerAdd(string landekode, string postnr, string by);

        /// <summary>
        /// Opdaterer et givent postnummer.
        /// </summary>
        /// <param name="landekode">Landekode.</param>
        /// <param name="postnr">Postnummer.</param>
        /// <param name="by">Bynavn.</param>
        void PostnummerModify(string landekode, string postnr, string by);

        /// <summary>
        /// Tilføjer en adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressegruppen.</param>
        /// <param name="navn">Navn på adressegruppen.</param>
        /// <param name="adressegruppeOswebdb">Nummer på den tilsvarende adressegruppe i OSWEBDB.</param>
        void AdressegruppeAdd(int nummer, string navn, int adressegruppeOswebdb);

        /// <summary>
        /// Opdaterer en given adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressegruppen.</param>
        /// <param name="navn">Navn på adressegruppen.</param>
        /// <param name="adressegruppeOswebdb">Nummer på den tilsvarende adressegruppe i OSWEBDB.</param>
        void AdressegruppeModify(int nummer, string navn, int adressegruppeOswebdb);

        /// <summary>
        /// Tilføjer en betalingsbetingelse.
        /// </summary>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <param name="navn">Navn på betalingsbetingelsen.</param>
        void BetalingsbetingelseAdd(int nummer, string navn);

        /// <summary>
        /// Opdaterer en given adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <param name="navn">Navn på betalingsbetingelsen.</param>
        void BetalingsbetingelseModify(int nummer, string navn);
    }
}
