using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en budgetoplysninger.
    /// </summary>
    [DataContract(Name = "Budgetoplysninger", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetoplysningerView : MånedsoplysningerView
    {
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
