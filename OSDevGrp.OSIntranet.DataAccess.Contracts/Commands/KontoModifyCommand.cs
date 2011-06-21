using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af en given konto.
    /// </summary>
    [DataContract(Name = "KontoModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontoModifyCommand : KontoModifyCommandBase
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
