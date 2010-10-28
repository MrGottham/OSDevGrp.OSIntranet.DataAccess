using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for kreditoplysninger.
    /// </summary>
    [DataContract(Name = "Kreditoplysninger", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KreditoplysningerView : MånedsoplysningerView
    {
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
