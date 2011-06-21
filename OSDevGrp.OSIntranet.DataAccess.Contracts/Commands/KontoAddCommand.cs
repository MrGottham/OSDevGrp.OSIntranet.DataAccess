using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en konto.
    /// </summary>
    [DataContract(Name = "KontoAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontoAddCommand : KontoAddCommandBase
    {
        /// <summary>
        /// Unik identifikation af kontogruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Kontogruppe
        {
            get;
            set;
        }
    }
}
