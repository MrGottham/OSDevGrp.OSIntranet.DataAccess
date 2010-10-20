using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en person.
    /// </summary>
    [DataContract(Name = "Person", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PersonView : AdresseView
    {
        /// <summary>
        /// Telefonnummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Telefon
        {
            get;
            set;
        }

        /// <summary>
        /// Mobilnummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Mobil
        {
            get;
            set;
        }

        /// <summary>
        /// Fødselsdato.
        /// </summary>
        [DataMember(IsRequired = false)]
        public DateTime? Fødselsdato
        {
            get;
            set;
        }

        /// <summary>
        /// Firma.
        /// </summary>
        [DataMember(IsRequired = false)]
        public FirmaView Firma
        {
            get;
            set;
        }
    }
}
