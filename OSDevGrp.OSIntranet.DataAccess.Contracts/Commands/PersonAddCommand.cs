using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en person.
    /// </summary>
    [DataContract(Name = "PersonAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PersonAddCommand : AdresseAddCommandBase
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
