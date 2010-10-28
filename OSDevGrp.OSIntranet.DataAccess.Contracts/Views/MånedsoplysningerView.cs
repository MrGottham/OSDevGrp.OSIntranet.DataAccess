using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for månedsoplysninger.
    /// </summary>
    [DataContract(Name = "Månedsoplysninger", Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class MånedsoplysningerView : IView
    {
        /// <summary>
        /// Årtsal
        /// </summary>
        [DataMember(IsRequired = true)]
        public int År
        {
            get;
            set;
        }

        /// <summary>
        /// Måned.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Måned
        {
            get;
            set;
        }
    }
}
