using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Basis viewobjekt for en konto- og budgetkontoliste.
    /// </summary>
    [DataContract(Name = "KontoListeViewBase", Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class KontoListeViewBase : IView
    {
        /// <summary>
        /// Regnskab.
        /// </summary>
        [DataMember(IsRequired = true)]
        public RegnskabListeView Regnskab
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }

        /// <summary>
        /// Kontonavn.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonavn
        {
            get;
            set;
        }

        /// <summary>
        /// Beskrivelse.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Beskrivelse
        {
            get;
            set;
        }

        /// <summary>
        /// Notat.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Note
        {
            get;
            set;
        }
    }
}
