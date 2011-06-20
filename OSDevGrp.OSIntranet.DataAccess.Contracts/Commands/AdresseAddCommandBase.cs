using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en baisadresse.
    /// </summary>
    [DataContract(Name = "AdresseAddCommandBase", Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class AdresseAddCommandBase : ICommand
    {
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
        /// Unik identifikation af adressegruppe.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Adressegruppe
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
        /// Unik identifikation af betalingsbetingelse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public int Betalingsbetingelse
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
    }
}
