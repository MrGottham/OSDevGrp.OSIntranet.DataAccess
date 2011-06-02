using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af en betalingsbetingelse.
    /// </summary>
    [DataContract(Name = "BetalingsbetingelseModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BetalingsbetingelseModifyCommand : ICommand
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
