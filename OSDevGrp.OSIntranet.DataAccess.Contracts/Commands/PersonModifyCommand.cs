using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af en given person.
    /// </summary>
    [DataContract(Name = "PersonModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PersonModifyCommand : AdresseModifyCommandBase
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
        /// Unik identifikation af firmaet, som personen er tilknyttet.
        /// </summary>
        [DataMember(IsRequired = false)]
        public int Firma
        {
            get;
            set;
        }
    }
}
