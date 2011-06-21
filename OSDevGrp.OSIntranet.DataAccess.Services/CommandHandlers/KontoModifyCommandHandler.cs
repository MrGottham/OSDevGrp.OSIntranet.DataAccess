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
    /// Commandhandler til håndtering af kommandoen: KontoModifyCommand.
    /// </summary>
    public class KontoModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<KontoModifyCommand, KontoView>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: KontoModifyCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public KontoModifyCommandHandler(IFinansstyringRepository finansstyringRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<KontoModifyCommand,KontoView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af en given konto.</param>
        /// <returns>Opdateret konto.</returns>
        public KontoView Execute(KontoModifyCommand command)
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
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Regnskab),
                                                 command.Regnskabsnummer), ex);
            }
            Kontogruppe kontogruppe;
            try
            {
                kontogruppe = _finansstyringRepository.KontogruppeGetAll().Single(m => m.Nummer == command.Kontogruppe);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Kontogruppe),
                                                 command.Kontogruppe), ex);
            }

            // TODO: Find konto? Det bør gøres her, således at vi sikrer, at kontoen findes inden rettelse.

            var konto = _finansstyringRepository.KontoModify(regnskab, command.Kontonummer, command.Kontonnavn,
                                                             command.Beskrivelse, command.Note, kontogruppe);

            return _objectMapper.Map<Konto, KontoView>(konto);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af en given konto.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(KontoModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (KontoModifyCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
