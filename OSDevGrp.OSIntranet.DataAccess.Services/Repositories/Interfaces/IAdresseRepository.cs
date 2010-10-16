using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Interface til repository for adressekartoteket.
    /// </summary>
    public interface IAdresseRepository : IRepository
    {
        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <returns>Liste indeholdende alle postnumre.</returns>
        IList<Postnummer> PostnummerGetAll();

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle adressegrupper.</returns>
        IList<Adressegruppe> AdressegruppeGetAll();

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <returns>Liste indeholdende alle betalingsbetingelser.</returns>
        IList<Betalingsbetingelse> BetalingsbetingelserGetAll();
    }
}
