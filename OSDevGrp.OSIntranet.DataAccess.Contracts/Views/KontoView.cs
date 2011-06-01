using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en konto.
    /// </summary>
    [DataContract(Name = "Konto", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontoView : KontoListeView
    {
        /// <summary>
        /// Kreditoplysninger.
        /// </summary>
        [DataMember(IsRequired = false)]
        public IEnumerable<KreditoplysningerView> Kreditoplysninger
        {
            get;
            set;
        }

        /// <summary>
        /// Bogføringslinjer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public IEnumerable<BogføringslinjeView> Bogføringslinjer
        {
            get;
            set;
        }
    }
}
