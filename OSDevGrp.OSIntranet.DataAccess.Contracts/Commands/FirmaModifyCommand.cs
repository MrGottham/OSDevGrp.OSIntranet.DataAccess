using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af et givent firma.
    /// </summary>
    [DataContract(Name = "FirmaModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class FirmaModifyCommand : AdresseModifyCommandBase
    {
        /// <summary>
        /// Telefonnummer (1. nummer).
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Telefon1
        {
            get;
            set;
        }

        /// <summary>
        /// Telefonnummer (2. nummer).
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Telefon2
        {
            get;
            set;
        }

        /// <summary>
        /// Telefax.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Telefax
        {
            get;
            set;
        }
    }
}
