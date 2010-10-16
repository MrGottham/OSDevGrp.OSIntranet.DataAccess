using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter bynavnet for et givent postnummer på en given landekode.
    /// </summary>
    [DataContract(Name = "BynavnGetByLandekodeAndPostnummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BynavnGetByLandekodeAndPostnummerQuery : IQuery
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

        /// <summary>
        /// Postnummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Postnummer
        {
            get;
            set;
        }
    }
}
