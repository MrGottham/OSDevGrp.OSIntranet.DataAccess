using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Enums
{
    /// <summary>
    /// Typer af kontogrupper.
    /// </summary>
    [DataContract(Name = "KontogruppeType", Namespace = SoapNamespaces.DataAccessNamespace)]
    public enum KontogruppeType
    {
        /// <summary>
        /// Aktiver.
        /// </summary>
        [EnumMember]
        Aktiver,

        /// <summary>
        /// Passiver.
        /// </summary>
        [EnumMember]
        Passiver
    }
}
