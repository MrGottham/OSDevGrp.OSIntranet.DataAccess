using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en given person.
    /// </summary>
    [DataContract(Name = "PersonGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PersonGetByNummerQuery : IQuery
    {
        /// <summary>
        /// Unik identifikation af personen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
