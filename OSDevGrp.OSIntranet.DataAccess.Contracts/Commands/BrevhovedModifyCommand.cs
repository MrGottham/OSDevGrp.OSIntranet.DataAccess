using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af et givent brevhoved.
    /// </summary>
    [DataContract(Name = "BrevhovedModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BrevhovedModifyCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af brevhovedet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på brevhovedet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 1. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje1
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 2. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje2
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 3. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje3
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 4. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje4
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 5. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje5
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 6. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje6
        {
            get;
            set;
        }

        /// <summary>
        /// Brevhovedets 7. linje.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Linje7
        {
            get;
            set;
        }
    }
}
