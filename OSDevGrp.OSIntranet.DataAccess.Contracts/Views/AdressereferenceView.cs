using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en adressereference.
    /// </summary>
    [DataContract(Name = "Adressereference", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdressereferenceView : IView
    {
        /// <summary>
        /// Unik identifikation af adressen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på adressen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }
    }
}
