using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en basiskonto.
    /// </summary>
    [DataContract(Name = "KontoAddCommandBase", Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class KontoAddCommandBase : ICommand
    {
        /// <summary>
        /// Unik identifikation af regnskabet for kontoen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonavn.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonavn
        {
            get;
            set;
        }

        /// <summary>
        /// Beskrivelse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Beskrivelse
        {
            get;
            set;
        }

        /// <summary>
        /// Notat.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Note
        {
            get;
            set;
        }
    }
}
