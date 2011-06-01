using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for et firma.
    /// </summary>
    [DataContract(Name = "Firma", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class FirmaView : AdresseView
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

        /// <summary>
        /// Personer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public IEnumerable<PersonView> Personer
        {
            get;
            set;
        }
    }
}
