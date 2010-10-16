using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobject for et postnummer.
    /// </summary>
    [DataContract(Name = "Postnummer", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PostnummerView : IView
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
