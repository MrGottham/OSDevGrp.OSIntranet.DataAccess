using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en betalingsbetingelse.
    /// </summary>
    [DataContract(Name = "BetalingsbetingelseAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BetalingsbetingelseAddCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af betalingsbetingelsen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på betalingsbetingelsen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }
    }
}
