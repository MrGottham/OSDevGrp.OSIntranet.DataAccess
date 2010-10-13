using System;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Repository for finansstyring.
    /// </summary>
    public class FinansstyringRepository : RepositoryBase, IFinansstyringRepositoryService
    {
        #region Private variables

        private readonly ILogRepository _logRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository for finansstyring.
        /// </summary>
        /// <param name="logRepository">Logging repository.</param>
        public FinansstyringRepository(ILogRepository logRepository)
        {
            if (logRepository == null)
            {
                throw new ArgumentNullException("logRepository");
            }
            _logRepository = logRepository;
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
