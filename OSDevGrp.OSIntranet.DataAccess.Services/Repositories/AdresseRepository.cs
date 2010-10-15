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
        #region

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
                                                                                   dbHandle, searchHandle, "Navn");
                                                                               var betalingsbetingelse =
                                                                                   new Betalingsbetingelse(nummer, navn);
                                                                               list.Add(betalingsbetingelse);
                                                                           });
        }

        #endregion
    }
}
