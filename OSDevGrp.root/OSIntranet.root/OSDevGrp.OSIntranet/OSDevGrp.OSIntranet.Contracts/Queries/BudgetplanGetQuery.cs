﻿using System;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en budgetplan.
    /// </summary>
    [DataContract(Name = "BudgetplanGetQuery", Namespace = SoapNamespaces.IntranetNamespace)]
    public class BudgetplanGetQuery : IQuery
    {
        /// <summary>
        /// Regnskabsnummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
        {
            get;
            set;
        }

        /// <summary>
        /// Statusdato.
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime StatusDato
        {
            get;
            set;
        }

        /// <summary>
        /// Antal måneder inklusiv indeværende måned, der skal indgår i budgethistorikken.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Budgethistorik
        {
            get;
            set;
        }
    }
}
