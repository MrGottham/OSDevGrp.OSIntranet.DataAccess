using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Interface til repository for fælles elementer.
    /// </summary>
    public interface IFællesRepository : IRepository
    {
        /// <summary>
        /// Henter alle brevhoveder.
        /// </summary>
        /// <returns>Liste indeholdende alle brevhoveder.</returns>
        IEnumerable<Brevhoved> BrevhovedGetAll();

        /// <summary>
        /// Henter et givent brevhoved.
        /// </summary>
        /// <param name="nummer">Unik identifikation af brevhovedet, der skal hentes.</param>
        /// <returns>Brevhoved.</returns>
        Brevhoved BrevhovedGetByNummer(int nummer);

        /// <summary>
        /// Tilføjer og returnerer et brevhoved.
        /// </summary>
        /// <param name="nummer">Unik identifikation af brevhovedet.</param>
        /// <param name="navn">Navn på brevhovedet.</param>
        /// <param name="linje1">Brevhovedets 1. linje.</param>
        /// <param name="linje2">Brevhovedets 2. linje.</param>
        /// <param name="linje3">Brevhovedets 3. linje.</param>
        /// <param name="linje4">Brevhovedets 4. linje.</param>
        /// <param name="linje5">Brevhovedets 5. linje.</param>
        /// <param name="linje6">Brevhovedets 6. linje.</param>
        /// <param name="linje7">Brevhovedets 7. linje.</param>
        /// <param name="cvrNr">CVR-nummer.</param>
        /// <returns>Det tilføjede brevhoved.</returns>
        Brevhoved BrevhovedAdd(int nummer, string navn, string linje1, string linje2, string linje3, string linje4, string linje5, string linje6, string linje7, string cvrNr);

        /// <summary>
        /// Opdaterer og returnerer et givent brevhoved.
        /// </summary>
        /// <param name="nummer">Unik identifikation af brevhovedet.</param>
        /// <param name="navn">Navn på brevhovedet.</param>
        /// <param name="linje1">Brevhovedets 1. linje.</param>
        /// <param name="linje2">Brevhovedets 2. linje.</param>
        /// <param name="linje3">Brevhovedets 3. linje.</param>
        /// <param name="linje4">Brevhovedets 4. linje.</param>
        /// <param name="linje5">Brevhovedets 5. linje.</param>
        /// <param name="linje6">Brevhovedets 6. linje.</param>
        /// <param name="linje7">Brevhovedets 7. linje.</param>
        /// <param name="cvrNr">CVR-nummer.</param>
        /// <returns>Det opdaterede brevhoved.</returns>
        Brevhoved BrevhovedModify(int nummer, string navn, string linje1, string linje2, string linje3, string linje4, string linje5, string linje6, string linje7, string cvrNr);
    }
}
