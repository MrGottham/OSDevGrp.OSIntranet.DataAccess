using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af et firma.
    /// </summary>
    [DataContract(Name = "FirmaAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class FirmaAddCommand : AdresseAddCommandBase
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
