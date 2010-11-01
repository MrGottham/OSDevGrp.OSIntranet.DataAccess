using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.CommandHandlers
{
    /// <summary>
    /// Commandhandler til håndtering af kommandoen: BogføringslinjeAddCommand.
    /// </summary>
    public class BogføringslinjeAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BogføringslinjeAddCommand>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IAdresseRepository _adresseRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BogføringslinjeAddCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        public BogføringslinjeAddCommandHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            _finansstyringRepository = finansstyringRepository;
            _adresseRepository = adresseRepository;
        }

        #endregion

        #region ICommandHandler<BogføringslinjeAddCommand> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Kommand til oprettelse af en bogføringslinje.</param>
        public void Execute(BogføringslinjeAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            Regnskab regnskab;
            try
            {
                regnskab = _finansstyringRepository.RegnskabGetAll()
                    .Single(m => m.Nummer == command.Regnskabsnummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Regnskab),
                                                 command.Regnskabsnummer), ex);
            }
            Konto konto;
            try
            {
                konto = regnskab.Konti
                    .OfType<Konto>()
                    .Single(m => m.Kontonummer.CompareTo(command.Kontonummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Konto),
                                                 command.Kontonummer), ex);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Kommando til oprettelse af en bogføringslinje.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(BogføringslinjeAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (BogføringslinjeAddCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
