using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle postnumre for en given landekode.
    /// </summary>
    [DataContract(Name = "PostnummerGetByLandekodeQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PostnummerGetByLandekodeQuery : IQuery
    {
        /// <summary>
        /// Landekode.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Landekode
        {
            get;
            set;
        }
    }
}
