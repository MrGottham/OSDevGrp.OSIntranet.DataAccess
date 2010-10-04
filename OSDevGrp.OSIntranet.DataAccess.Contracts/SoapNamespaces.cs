namespace OSDevGrp.OSIntranet.DataAccess.Contracts
{
    /// <summary>
    /// Indeholder en samlet registrering af de soap namespaces, 
    /// der anvendes i løsningen.
    /// </summary>
    public static class SoapNamespaces
    {
        /// <summary>
        /// Namespace for commands, queries, views m.m.
        /// </summary>
        public const string DataAccessNamespace = "urn:osdevgrp:osintranet.dataaccess:1.0.0";

        /// <summary>
        /// Navn på service til adressekartoteket.
        /// </summary>
        public const string AdresseRepositoryServiceName = "AdresseRepositoryService";

        /// <summary>
        /// Navn på service til finansstyring.
        /// </summary>
        public const string FinansstyringRepositoryServiceName = "FinansstyringRepositoryService";
    }
}
