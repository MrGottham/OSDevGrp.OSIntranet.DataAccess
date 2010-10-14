using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Implementations
{
    /// <summary>
    /// Repositoryservice for finansstyring.
    /// </summary>
    public class FinansstyringRepositoryService : RepositoryServiceBase, IFinansstyringRepositoryService
    {
        #region Constructor

        /// <summary>
        /// Danner repositoryservice for finansstyring.
        /// </summary>
        /// <param name="logRepository">Logging repository.</param>
        public FinansstyringRepositoryService(ILogRepository logRepository)
            : base(logRepository)
        {
        }

        #endregion

        #region IFinansstyringRepositoryService Members

        public void Test()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
