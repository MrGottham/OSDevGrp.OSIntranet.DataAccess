using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en given adressegruppe.
    /// </summary>
    [DataContract(Name = "AdressegruppeGetByNummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdressegruppeGetByNummerQuery : IQuery
    {
        /// <summary>
        /// Nummer på adressegruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
