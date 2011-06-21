using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering eller tilføjelse af kreditoplysninger.
    /// </summary>
    [DataContract(Name = "KreditoplysningerAddOrModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KreditoplysningerAddOrModifyCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af regnskabet, hvorpå kreditoplysninger skal opdateres eller tilføjes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonummer på kontoen, hvorpå kreditoplysninger skal opdateres eller tilføjes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Årstal for kreditoplysninger.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int År
        {
            get;
            set;
        }

        /// <summary>
        /// Måned for kreditoplysninger.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Måned
        {
            get;
            set;
        }

        /// <summary>
        /// Kredit.
        /// </summary>
        [DataMember(IsRequired = true)]
        public decimal Kredit
        {
            get;
            set;
        }
    }
}
