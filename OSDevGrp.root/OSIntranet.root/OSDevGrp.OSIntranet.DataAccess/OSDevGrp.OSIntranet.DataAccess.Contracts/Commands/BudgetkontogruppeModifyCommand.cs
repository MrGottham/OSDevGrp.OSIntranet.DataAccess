﻿using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af en given gruppe til budgetkonti.
    /// </summary>
    [DataContract(Name = "BudgetkontogruppeModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontogruppeModifyCommand : ICommand
    {
        /// <summary>
        /// Unik identifikation af gruppen til budgetkonti.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på gruppen til budgetkonti.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }
    }
}
