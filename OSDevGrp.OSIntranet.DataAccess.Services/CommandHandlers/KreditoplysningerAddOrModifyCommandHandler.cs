using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.CommandHandlers
{
    /// <summary>
    /// Commandhandler til håndtering af kommandoen: KreditoplysningerAddOrModifyCommand.
    /// </summary>
    public class KreditoplysningerAddOrModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<KreditoplysningerAddOrModifyCommand, KreditoplysningerView>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: KreditoplysningerAddOrModifyCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KreditoplysningerAddOrModifyCommandHandler(IFinansstyringRepository finansstyringRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _finansstyringRepository = finansstyringRepository;
            _fællesRepository = fællesRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region ICommandHandler<KreditoplysningerAddOrModifyCommand,KreditoplysningerView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering eller tilføjelse af kreditoplysninger.</param>
        /// <returns>Opdateret eller tilføjet kreditoplysninger.</returns>
        public KreditoplysningerView Execute(KreditoplysningerAddOrModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Regnskab regnskab;
            try
            {
                var getBrevhoved = new Func<int, Brevhoved>(nummer => _fællesRepository.BrevhovedGetByNummer(nummer));
                regnskab = _finansstyringRepository.RegnskabGetAll(getBrevhoved)
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

            var kreditoplysninger = _finansstyringRepository.KreditoplysningerModifyOrAdd(konto, command.År,
                                                                                          command.Måned, command.Kredit);

            return _objectMapper.Map<Kreditoplysninger, KreditoplysningerView>(kreditoplysninger);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering eller tilføjelse af kreditoplysninger.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(KreditoplysningerAddOrModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler,
                                             typeof (KreditoplysningerAddOrModifyCommand), exception.Message), exception);
        }

        #endregion
    }
}
