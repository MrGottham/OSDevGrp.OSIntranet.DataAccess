using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse alle bogføringslinjer på et givent regnskab.
    /// </summary>
    [DataContract(Name = "BogføringslinjeGetByRegnskabQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BogføringslinjeGetByRegnskabQuery : IQuery
    {
        /// <summary>
        /// Unik identifikation af regnskabet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
        {
            get;
            set;
        }
    }
}
