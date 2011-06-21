using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering eller tilføjelse af kreditoplysninger.
    /// </summary>
    [DataContract(Name = "BudgetoplysningerAddOrModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetoplysningerAddOrModifyCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af regnskabet, hvorpå budgetoplysninger skal opdateres eller tilføjes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonummer på budgetkontoen, hvorpå budgetoplysninger skal opdateres eller tilføjes.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Budgetkontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Årstal for budgetoplysninger.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int År
        {
            get;
            set;
        }

        /// <summary>
        /// Måned for budgetoplysninger.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Måned
        {
            get;
            set;
        }

        /// <summary>
        /// Indtægter.
        /// </summary>
        [DataMember(IsRequired = true)]
        public decimal Indtægter
        {
            get;
            set;
        }

        /// <summary>
        /// Udgifter.
        /// </summary>
        [DataMember(IsRequired = true)]
        public decimal Udgifter
        {
            get;
            set;
        }
    }
}
