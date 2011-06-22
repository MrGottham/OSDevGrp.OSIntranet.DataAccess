using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af et givent regnskab.
    /// </summary>
    [DataContract(Name = "RegnskabModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class RegnskabModifyCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af regnskabet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på regnskabet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }

        /// <summary>
        /// Unik identifikation af brevhovedet til regnskabet.
        /// </summary>
        [DataMember(IsRequired = false)]
        public int Brevhoved
        {
            get;
            set;
        }
    }
}
