using System.Collections.Generic;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en adresse.
    /// </summary>
    [DataContract(Name = "Adresse", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdresseView : IView
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
        /// Navn.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
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
        /// Postnummer og by.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string PostnummerBy
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
        /// Bekendtskab.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Bekendtskab
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

        /// <summary>
        /// Webadresse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Webadresse
        {
            get;
            set;
        }

        /// <summary>
        /// Betalingsbetingelse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public BetalingsbetingelseView Betalingsbetingelse
        {
            get;
            set;
        }

        /// <summary>
        /// Udlånsfrist.
        /// </summary>
        [DataMember(IsRequired = false)]
        public int Udlånsfrist
        {
            get;
            set;
        }

        /// <summary>
        /// Markering for Filofax adresselabel.
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool FilofaxAdresselabel
        {
            get;
            set;
        }

        /// <summary>
        /// Bogføringslinjer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public IList<BogføringslinjeView> Bogføringslinjer
        {
            get;
            set;
        }
    }
}
