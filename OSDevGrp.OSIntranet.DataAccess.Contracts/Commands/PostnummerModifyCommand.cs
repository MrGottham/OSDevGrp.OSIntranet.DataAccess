using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af et postnummer.
    /// </summary>
    [DataContract(Name = "PostnummerModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PostnummerModifyCommand : ICommand
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

        /// <summary>
        /// Bynavn.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Bynavn
        {
            get;
            set;
        }
    }
}
