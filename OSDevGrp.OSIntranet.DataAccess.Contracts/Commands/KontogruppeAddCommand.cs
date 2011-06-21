using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Enums;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en kontogruppe.
    /// </summary>
    [DataContract(Name = "KontogruppeAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontogruppeAddCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af kontogruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på kontogruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }

        /// <summary>
        /// Kontogruppetype.
        /// </summary>
        [DataMember(IsRequired = true)]
        public KontogruppeType KontogruppeType
        {
            get;
            set;
        }
    }
}
