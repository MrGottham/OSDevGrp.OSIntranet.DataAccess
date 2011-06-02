using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en adressegruppe.
    /// </summary>
    [DataContract(Name = "AdressegruppeAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdressegruppeAddCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af adressegruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på adressegruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }

        /// <summary>
        /// Nummeret på den tilsvarende adressegruppe i OSWEBDB.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int AdressegruppeOswebdb
        {
            get;
            set;
        }
    }
}
