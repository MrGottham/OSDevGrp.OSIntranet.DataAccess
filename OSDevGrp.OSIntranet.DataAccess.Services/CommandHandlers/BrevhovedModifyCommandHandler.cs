using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.CommandHandlers
{
    /// <summary>
    /// Commandhandler til håndtering af kommandoen: BrevhovedModifyCommand.
    /// </summary>
    public class BrevhovedModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BrevhovedModifyCommand>
    {
        #region Private variables

        private readonly IFællesRepository _fællesRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BrevhovedModifyCommand.
        /// </summary>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        public BrevhovedModifyCommandHandler(IFællesRepository fællesRepository)
        {
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            _fællesRepository = fællesRepository;
        }

        #endregion

        #region ICommandHandler<BrevhovedModifyCommand> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af et givent brevhoved.</param>
        public void Execute(BrevhovedModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Brevhoved brevhoved;
            try
            {
                brevhoved = _fællesRepository.BrevhovedGetAll().Single(m => m.Nummer == command.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Brevhoved),
                                                 command.Nummer), ex);
            }
            brevhoved.SætNavn(command.Navn);
            brevhoved.SætLinje1(command.Linje1);
            brevhoved.SætLinje2(command.Linje2);
            brevhoved.SætLinje3(command.Linje3);
            brevhoved.SætLinje4(command.Linje4);
            brevhoved.SætLinje5(command.Linje5);
            brevhoved.SætLinje6(command.Linje6);
            brevhoved.SætLinje7(command.Linje7);

            _fællesRepository.BrevhovedModify(brevhoved.Nummer, brevhoved.Navn, brevhoved.Linje1, brevhoved.Linje2,
                                              brevhoved.Linje3, brevhoved.Linje4, brevhoved.Linje5, brevhoved.Linje6,
                                              brevhoved.Linje7);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af et givent brevhoved.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(BrevhovedModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (BrevhovedModifyCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
