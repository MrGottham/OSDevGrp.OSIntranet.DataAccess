using System;
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
    /// Commandhandler til håndtering af kommandoen: BrevhovedAddCommand.
    /// </summary>
    public class BrevhovedAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BrevhovedAddCommand>
    {
        #region Private variables

        private readonly IFællesRepository _fællesRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BrevhovedAddCommand.
        /// </summary>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        public BrevhovedAddCommandHandler(IFællesRepository fællesRepository)
        {
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            _fællesRepository = fællesRepository;
        }

        #endregion

        #region ICommandHandler<BrevhovedAddCommand> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til oprettelse af et brevhoved.</param>
        public void Execute(BrevhovedAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var brevhoved = new Brevhoved(command.Nummer, command.Navn);
            brevhoved.SætLinje1(command.Linje1);
            brevhoved.SætLinje2(command.Linje2);
            brevhoved.SætLinje3(command.Linje3);
            brevhoved.SætLinje4(command.Linje4);
            brevhoved.SætLinje5(command.Linje5);
            brevhoved.SætLinje6(command.Linje6);
            brevhoved.SætLinje7(command.Linje7);

            _fællesRepository.BrevhovedAdd(brevhoved.Nummer, brevhoved.Navn, brevhoved.Linje1, brevhoved.Linje2,
                                           brevhoved.Linje3, brevhoved.Linje4, brevhoved.Linje5, brevhoved.Linje6,
                                           brevhoved.Linje7);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til oprettelse af et brevhoved.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(BrevhovedAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (BrevhovedAddCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
