using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til repository for adressekartoteket.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.AdresseRepositoryServiceName, Namespace = SoapNamespaces.DataAccessNamespace)]
    public interface IAdresseRepositoryService : IRepositoryService
    {
        /// <summary>
        /// Henter alle personer.
        /// </summary>
        /// <param name="personGetAllQuery">Query til forespørgelse efter alle personer.</param>
        /// <returns>Alle personer.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<PersonView> PersonGetAll(PersonGetAllQuery personGetAllQuery);

        /// <summary>
        /// Henter en given person.
        /// </summary>
        /// <param name="personGetByNummerQuery">Query til forespørgelse efter en given person.</param>
        /// <returns>Person.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PersonView PersonGetByNummer(PersonGetByNummerQuery personGetByNummerQuery);

        /// <summary>
        /// Tilføjer en person.
        /// </summary>
        /// <param name="personAddCommand">Command til tilføjelse af en person.</param>
        /// <returns>Tilføjet person.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PersonView PersonAdd(PersonAddCommand personAddCommand);

        /// <summary>
        /// Opdaterer en given person.
        /// </summary>
        /// <param name="personModifyCommand">Command til opdatering af en given person.</param>
        /// <returns>Opdateret person.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PersonView PersonModify(PersonModifyCommand personModifyCommand);

        /// <summary>
        /// Henter alle firmaer.
        /// </summary>
        /// <param name="firmaGetAllQuery">Query til forespørgelse efter alle firmaer.</param>
        /// <returns>Alle firmaer.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<FirmaView> FirmaGetAll(FirmaGetAllQuery firmaGetAllQuery);

        /// <summary>
        /// Henter et givent firma.
        /// </summary>
        /// <param name="firmaGetByNummerQuery">Query til forespørgelse efter et givent firma.</param>
        /// <returns>Firma.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        FirmaView FirmaGetByNummer(FirmaGetByNummerQuery firmaGetByNummerQuery);

        /// <summary>
        /// Tilføjer et firma.
        /// </summary>
        /// <param name="firmaAddCommand">Command til tilføjelse af et firma.</param>
        /// <returns>Tilføjet firma.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        FirmaView FirmaAdd(FirmaAddCommand firmaAddCommand);

        /// <summary>
        /// Opdaterer et givent firma.
        /// </summary>
        /// <param name="firmaModifyCommand">Command til opdatering af et givent firma.</param>
        /// <returns>Opdateret firma.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        FirmaView FirmaModify(FirmaModifyCommand firmaModifyCommand);

        /// <summary>
        /// Henter alle adresser til en adresseliste.
        /// </summary>
        /// <param name="adresselisteGetAllQuery">Query til forespørgelse efter alle adresser til en adresseliste.</param>
        /// <returns>Alle adresser til en adresseliste.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<AdresselisteView> AdresselisteGetAll(AdresselisteGetAllQuery adresselisteGetAllQuery);

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <param name="postnummerGetAllQuery">Query til forespørgelse efter alle postnumre.</param>
        /// <returns>Alle postnumre.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<PostnummerView> PostnummerGetAll(PostnummerGetAllQuery postnummerGetAllQuery);

        /// <summary>
        /// Henter alle postnumre for en given landekode.
        /// </summary>
        /// <param name="postnummerGetByLandekodeQuery">Query til forespørgelse efter alle postnumre for en given landekode.</param>
        /// <returns>Alle postnumre for den givne landekode.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<PostnummerView> PostnummerGetAllByLandekode(PostnummerGetByLandekodeQuery postnummerGetByLandekodeQuery);

        /// <summary>
        /// Henter bynavnet til et givent postnummer på en given landekode.
        /// </summary>
        /// <param name="bynavnGetByLandekodeAndPostnummerQuery">Query til forespørgelse efter bynavnet for et givent postnummer på en given landekode.</param>
        /// <returns>Landekode, postnummer og bynavn</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PostnummerView BynavnGetByLandekodeAndPostnummre(BynavnGetByLandekodeAndPostnummerQuery bynavnGetByLandekodeAndPostnummerQuery);

        /// <summary>
        /// Tilføjer et postnummer.
        /// </summary>
        /// <param name="postnummerAddCommand">Command til tilføjelse af et postnummer.</param>
        /// <returns>Tilføjet postnummer.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PostnummerView PostnummerAdd(PostnummerAddCommand postnummerAddCommand);

        /// <summary>
        /// Opdaterer et givent postnummer.
        /// </summary>
        /// <param name="postnummerModifyCommand">Command til opdatering af et givent postnummer.</param>
        /// <returns>Opdateret postnummer.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        PostnummerView PostnummerModify(PostnummerModifyCommand postnummerModifyCommand);

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <param name="adressegruppeGetAllQuery">Query til forespørgelse efter alle adressegrupper.</param>
        /// <returns>Alle adressegrupper.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<AdressegruppeView> AdressegruppeGetAll(AdressegruppeGetAllQuery adressegruppeGetAllQuery);

        /// <summary>
        /// Henter en given adressegruppe.
        /// </summary>
        /// <param name="adressegruppeGetByNummerQuery">Query til forespørgelse efter en given adressegruppe.</param>
        /// <returns>Adressegruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        AdressegruppeView AdressegruppeGetByNummer(AdressegruppeGetByNummerQuery adressegruppeGetByNummerQuery);

        /// <summary>
        /// Tilføjer en adressegruppe.
        /// </summary>
        /// <param name="adressegruppeAddCommand">Command til tilføjelse af en adressegruppe.</param>
        /// <returns>Tilføjet adressegruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        AdressegruppeView AdressegruppeAdd(AdressegruppeAddCommand adressegruppeAddCommand);

        /// <summary>
        /// Opdaterer en given adressegruppe.
        /// </summary>
        /// <param name="adressegruppeModifyCommand">Command til opdatering af en given adressegruppe.</param>
        /// <returns>Opdateret adressegruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        AdressegruppeView AdressegruppeModify(AdressegruppeModifyCommand adressegruppeModifyCommand);

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <param name="betalingsbetingelseGetAllQuery">Query til forespørgelse efter alle betalingsbetingelser.</param>
        /// <returns>Alle betalingsbetingelser.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BetalingsbetingelseView> BetalingsbetingelseGetAll(BetalingsbetingelseGetAllQuery betalingsbetingelseGetAllQuery);

        /// <summary>
        /// Henter en given betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseGetByNummerQuery">Query til forespørgelse efter en given betalingsbetingelse.</param>
        /// <returns>Betalingsbetingelse.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BetalingsbetingelseView BetalingsbetingelseGetByNummer(BetalingsbetingelseGetByNummerQuery betalingsbetingelseGetByNummerQuery);

        /// <summary>
        /// Tilføjer en betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseAddCommand">Command til tilføjelse af en betalingsbetingelse.</param>
        /// <returns>Tilføjet betalingsbetingelse.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BetalingsbetingelseView BetalingsbetingelseAdd(BetalingsbetingelseAddCommand betalingsbetingelseAddCommand);

        /// <summary>
        /// Opdaterer en given betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseModifyCommand">Command til opdatering af en given betalingsbetingelse.</param>
        /// <returns>Opdateret betalingsbetingelse.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BetalingsbetingelseView BetalingsbetingelseModify(BetalingsbetingelseModifyCommand betalingsbetingelseModifyCommand);
    }
}
