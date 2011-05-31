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
