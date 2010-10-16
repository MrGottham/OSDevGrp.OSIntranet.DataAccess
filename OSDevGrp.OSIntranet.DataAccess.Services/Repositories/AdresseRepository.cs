using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Repository for adressekartoteket.
    /// </summary>
    public class AdresseRepository : DbAxRepositoryBase, IAdresseRepository
    {
        #region Constructor

        /// <summary>
        /// Danner repository for adressekartoteket.
        /// </summary>
        /// <param name="dbAxConfiguration">Konfiguration for DBAX.</param>
        public AdresseRepository(IDbAxConfiguration dbAxConfiguration)
            : base(dbAxConfiguration)
        {
        }

        #endregion

        #region IAdresseRepository Members

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <returns>Liste indeholdende alle postnumre.</returns>
        public IList<Postnummer> PostnummerGetAll()
        {
            var dbHandle = OpenDatabase("POSTNR.DBD", false, true);
            try
            {
                var searchHandle = dbHandle.CreateSearch();
                try
                {
                    var postnumre = new List<Postnummer>();
                    if (dbHandle.SetKey(searchHandle, "Postnummer"))
                    {
                        if (dbHandle.SearchFirst(searchHandle))
                        {
                            
                        }
                    }
                    return postnumre;
                }
                finally
                {
                    dbHandle.DeleteSearch(searchHandle);
                }
            }
            finally
            {
                dbHandle.CloseDatabase();
            }
        }

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle adressegrupper.</returns>
        public IList<Adressegruppe> AdressegruppeGetAll()
        {
            return GetTableContentFromTabel<Adressegruppe>(1030, (dbHandle, searchHandle, list) =>
                                                                     {
                                                                         var nummer = GetFieldValueAsInt(dbHandle,
                                                                                                         searchHandle,
                                                                                                         "Nummer");
                                                                         var navn = GetFieldValueAsString(dbHandle,
                                                                                                          searchHandle,
                                                                                                          "Tekst");
                                                                         var adrgrp = GetFieldValueAsInt(dbHandle,
                                                                                                         searchHandle,
                                                                                                         "Adressegruppe");
                                                                         var adressegruppe = new Adressegruppe(nummer,
                                                                                                               navn,
                                                                                                               adrgrp);
                                                                         list.Add(adressegruppe);
                                                                     });
        }

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <returns>Liste indeholdende alle betalingsbetingelser.</returns>
        public IList<Betalingsbetingelse> BetalingsbetingelserGetAll()
        {
            return GetTableContentFromTabel<Betalingsbetingelse>(1040, (dbHandle, searchHandle, list) =>
                                                                           {
                                                                               var nummer = GetFieldValueAsInt(
                                                                                   dbHandle, searchHandle, "Nummer");
                                                                               var navn = GetFieldValueAsString(
                                                                                   dbHandle, searchHandle, "Tekst");
                                                                               var betalingsbetingelse =
                                                                                   new Betalingsbetingelse(nummer, navn);
                                                                               list.Add(betalingsbetingelse);
                                                                           });
        }

        #endregion
    }
}
