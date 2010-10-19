﻿using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;
using DBAX;

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
        /// Henter alle adresser.
        /// </summary>
        /// <returns>Aller adresser.</returns>
        public IList<AdresseBase> AdresserGetAll()
        {
            var dbHandle = OpenDatabase("ADRESSE.DBD", false, true);
            try
            {
                var searchHandle = dbHandle.CreateSearch();
                try
                {
                    var adressegrupper = AdressegruppeGetAll();
                    var betalingsbetingelser = BetalingsbetingelserGetAll();
                    var adresser = new List<AdresseBase>();
                    if (dbHandle.SetKey(searchHandle, "Navn"))
                    {
                        var keyStr = dbHandle.KeyStrInt(1010,
                                                        dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr")));
                        if (dbHandle.SetKeyInterval(searchHandle, keyStr, keyStr))
                        {
                            if (dbHandle.SearchFirst(searchHandle))
                            {
                                do
                                {
                                    var firma = new Firma(GetFieldValueAsInt(dbHandle, searchHandle, "Ident"),
                                                          GetFieldValueAsString(dbHandle, searchHandle, "Navn"),
                                                          GetAdressegruppe(adressegrupper,
                                                                           GetFieldValueAsInt(dbHandle, searchHandle,
                                                                                              "Gruppenummer")));
                                    InitialiserAdresseBase(firma, dbHandle, searchHandle, betalingsbetingelser);
                                    var telefon1 = GetFieldValueAsString(dbHandle, searchHandle, "Telefon");
                                    var telefon2 = GetFieldValueAsString(dbHandle, searchHandle, "Telefon2");
                                    var telefax = GetFieldValueAsString(dbHandle, searchHandle, "Telefon3");
                                    firma.SætTelefon(string.IsNullOrEmpty(telefon1) ? null : telefon1,
                                                     string.IsNullOrEmpty(telefon2) ? null : telefon2,
                                                     string.IsNullOrEmpty(telefax) ? null : telefax);
                                    adresser.Add(firma);
                                } while (dbHandle.SearchNext(searchHandle));
                            }
                            dbHandle.ClearKeyInterval(searchHandle);
                        }
                        keyStr = dbHandle.KeyStrInt(1000, dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr")));
                        if (dbHandle.SetKeyInterval(searchHandle, keyStr, keyStr))
                        {
                            if (dbHandle.SearchFirst(searchHandle))
                            {
                                do
                                {
                                    
                                } while (dbHandle.SearchNext(searchHandle));
                            }
                            dbHandle.ClearKeyInterval(searchHandle);
                        }
                    }
                    return adresser;
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
                            do
                            {
                                var landekode = GetFieldValueAsString(dbHandle, searchHandle, "Landekode");
                                var postnr = GetFieldValueAsString(dbHandle, searchHandle, "Postnummer");
                                var bynavn = GetFieldValueAsString(dbHandle, searchHandle, "By");
                                var postnummer = new Postnummer(landekode, postnr, bynavn);
                                postnumre.Add(postnummer);

                            } while (dbHandle.SearchNext(searchHandle));
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

        #region Methods

        /// <summary>
        /// Finder og returnerer en given adressegruppe.
        /// </summary>
        /// <param name="adressegrupper">Adressegrupper.</param>
        /// <param name="adressegruppeNummer">Unik identifikation af adressegruppen.</param>
        /// <returns>Adressegruppe.</returns>
        private static Adressegruppe GetAdressegruppe(IEnumerable<Adressegruppe> adressegrupper, int adressegruppeNummer)
        {
            if (adressegrupper == null)
            {
                throw new ArgumentNullException("adressegrupper");
            }
            try
            {
                return adressegrupper.Single(m => m.Nummer == adressegruppeNummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Adressegruppe),
                                                 adressegruppeNummer), ex);
            }
        }

        /// <summary>
        /// Finder og returnerer en given betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelser">Betalingsbetingelser.</param>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <returns>Betalingsbetingelse.</returns>
        private static Betalingsbetingelse GetBetalingsbetingelse(IEnumerable<Betalingsbetingelse> betalingsbetingelser, int nummer)
        {
            if (betalingsbetingelser == null)
            {
                throw new ArgumentNullException("betalingsbetingelser");
            }
            try
            {
                return betalingsbetingelser.Single(m => m.Nummer == nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Betalingsbetingelse),
                                                 nummer), ex);
            }
        }

        /// <summary>
        /// Initialiserer basisoplysinger for en adresse.
        /// </summary>
        /// <param name="adresse">Adresse.</param>
        /// <param name="dbHandle">DBAX databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="betalingsbetingelser">Betalingsbetingelser.</param>
        private void InitialiserAdresseBase(AdresseBase adresse, IDsiDbX dbHandle, int searchHandle, IEnumerable<Betalingsbetingelse> betalingsbetingelser)
        {
            if (adresse == null)
            {
                throw new ArgumentNullException("adresse");
            }
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            if (betalingsbetingelser == null)
            {
                throw new ArgumentNullException("betalingsbetingelser");
            }
            var adresse1 = GetFieldValueAsString(dbHandle, searchHandle, "Adresse1");
            var adresse2 = GetFieldValueAsString(dbHandle, searchHandle, "Adresse2");
            var postnummerBy = GetFieldValueAsString(dbHandle, searchHandle, "PostnummerBy");
            adresse.SætAdresseoplysninger(string.IsNullOrEmpty(adresse1) ? null : adresse1,
                                          string.IsNullOrEmpty(adresse2) ? null : adresse2,
                                          string.IsNullOrEmpty(postnummerBy) ? null : postnummerBy);
            var bekendtskab = GetFieldValueAsString(dbHandle, searchHandle, "Bekendtskab");
            if (!string.IsNullOrEmpty(bekendtskab))
            {
                adresse.SætBekendtskab(bekendtskab);
            }
            var mailadresse = GetFieldValueAsString(dbHandle, searchHandle, "Email");
            if (!string.IsNullOrEmpty(mailadresse))
            {
                adresse.SætMailadresse(mailadresse);
            }
            var webadresse = GetFieldValueAsString(dbHandle, searchHandle, "Web");
            if (!string.IsNullOrEmpty(webadresse))
            {
                adresse.SætWebadresse(webadresse);
            }
            adresse.SætBetalingsbetingelse(GetBetalingsbetingelse(betalingsbetingelser,
                                                                  GetFieldValueAsInt(dbHandle, searchHandle,
                                                                                     "Betalingsnummer")));
            adresse.SætUdlånsfrist(GetFieldValueAsInt(dbHandle, searchHandle, "Udlånsfrist"));
            adresse.SætFilofaxAdresselabel((GetFieldValueAsInt(dbHandle, searchHandle, "Andet") & 1) == 1);
        }

        #endregion
    }
}
