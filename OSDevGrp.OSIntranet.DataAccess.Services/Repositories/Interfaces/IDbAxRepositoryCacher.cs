namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Interface til caching på DBAX repositories.
    /// </summary>
    public interface IDbAxRepositoryCacher
    {
        /// <summary>
        /// Sletter cache.
        /// </summary>
        void ClearCache();

        /// <summary>
        /// Håndtering af ændring i et DBAX repository.
        /// </summary>
        /// <param name="databaseFileName">Navn på databasen, der er ændret.</param>
        void HandleRepositoryChange(string databaseFileName);
    }
}
