using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for et brevhoved.
    /// </summary>
    [DataContract(Name = "Brevhoved", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BrevhovedView : TabelView
    {
        /// <summary>
        /// Brevhovedet 1. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje1
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedet 2. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje2
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedet 3. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje3
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedet 4. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje4
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedet 5. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje5
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedet 6. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje6
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedet 7. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje7
        {
            get;
            set;
        }
    }
}
