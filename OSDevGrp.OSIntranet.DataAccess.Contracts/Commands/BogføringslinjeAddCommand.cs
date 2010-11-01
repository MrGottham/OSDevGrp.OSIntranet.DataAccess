using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en bogføringslinje.
    /// </summary>
    [DataContract(Name = "BogføringslinjeAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BogføringslinjeAddCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af regnskabet, hvorpå bogføringslinjen skal oprettes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
        {
            get;
            set;
        }

        /// <summary>
        /// Bogføringsdato.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime Bogføringsdato
        {
            get;
            set;
        }

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Bilag
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonummer, hvorpå bogføringslinjen skal bogføres.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Tekst.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Tekst
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonummer på budgetkontoen, hvor bogføringslinej skal bogføres.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Budgetkontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Debit
        {
            get;
            set;
        }

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Kredit
        {
            get;
            set;
        }

        /// <summary>
        /// Unik identifikation af adressen, hvortil bogføringslinjen skal bogføres.
        /// </summary>
        [DataMember(IsRequired = false)]
        public int AdresseId
        {
            get;
            set;
        }
    }
}
