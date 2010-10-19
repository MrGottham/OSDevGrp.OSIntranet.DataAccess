using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en adresseliste.
    /// </summary>
    [DataContract(Name = "Adresseliste", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdresselisteView
    {
        /// <summary>
        /// Unik identifikation af adressen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på adressen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }

        /// <summary>
        /// Adressegruppe.
        /// </summary>
        [DataMember(IsRequired = true)]
        public AdressegruppeView Adressegruppe
        {
            get;
            set;
        }

        /// <summary>
        /// Adresse (linje 1).
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Adresse1
        {
            get;
            set;
        }

        /// <summary>
        /// Adresse (linje 2).
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Adresse2
        {
            get;
            set;
        }

        /// <summary>
        /// Postnummer og bynavn.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string PostnummerBy
        {
            get;
            set;
        }

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
        /// Mailadresse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Mailadresse
        {
            get;
            set;
        }
    }
}
