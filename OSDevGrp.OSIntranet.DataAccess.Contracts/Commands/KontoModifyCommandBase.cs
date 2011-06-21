using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af en basiskonto.
    /// </summary>
    [DataContract(Name = "KontoModifyCommandBase", Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class KontoModifyCommandBase : ICommand
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
        public string Kontonnavn
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
