using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af et postnummer.
    /// </summary>
    [DataContract(Name = "PostnummerAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PostnummerAddCommand : ICommand
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
