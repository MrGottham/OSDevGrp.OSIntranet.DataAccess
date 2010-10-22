using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle grupper for budgetkonti.
    /// </summary>
    [DataContract(Name = "BudgetkontogruppeGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontogruppeGetAllQuery : IQuery
    {
    }
}
