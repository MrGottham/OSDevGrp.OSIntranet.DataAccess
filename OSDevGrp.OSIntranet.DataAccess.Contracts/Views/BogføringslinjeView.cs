using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en bogføringslinje.
    /// </summary>
    [DataContract(Name = "Bogføringslinje", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BogføringslinjeView : IView
    {
        /// <summary>
        /// Unik identifikation af bogføringslinjen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Løbenummer
        {
            get;
            set;
        }

        /// <summary>
        /// Dato.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime Dato
        {
            get;
            set;
        }

        /// <summary>
        /// Bilagsnummer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Bilag
        {
            get;
            set;
        }

        /// <summary>
        /// Konto.
        /// </summary>
        [DataMember(IsRequired = true)]
        public KontoListeView Konto
        {
            get;
            set;
        }

        /// <summary>
        /// Tekst.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Tekst
        {
            get;
            set;
        }

        /// <summary>
        /// Budgetkonto.
        /// </summary>
        [DataMember(IsRequired = false)]
        public BudgetkontoListeView Budgetkontor
        {
            get;
            set;
        }

        /// <summary>
        /// Debitbeløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Debit
        {
            get;
            set;
        }

        /// <summary>
        /// Kreditbeløb.
        /// </summary>
        [DataMember(IsRequired = false)]
        public decimal Kredit
        {
            get;
            set;
        }

        /// <summary>
        /// Adressen, hvorpå linjen er bogført.
        /// </summary>
        [DataMember(IsRequired = false)]
        public AdressereferenceView Adresse
        {
            get;
            set;
        }
    }
}
