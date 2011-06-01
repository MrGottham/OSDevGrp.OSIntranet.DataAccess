using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
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
    /// Commandhandler til håndtering af kommandoen: BogføringslinjeAddCommand.
    /// </summary>
    public class BogføringslinjeAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BogføringslinjeAddCommand>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IAdresseRepository _adresseRepository;
        private readonly IFællesRepository _fællesRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BogføringslinjeAddCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        public BogføringslinjeAddCommandHandler(IFinansstyringRepository finansstyringRepository, IAdresseRepository adresseRepository, IFællesRepository fællesRepository)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (fællesRepository == null)
            {
                throw new ArgumentNullException("fællesRepository");
            }
            _finansstyringRepository = finansstyringRepository;
            _adresseRepository = adresseRepository;
            _fællesRepository = fællesRepository;
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
            Budgetkonto budgetkonto = null;
            if (!string.IsNullOrEmpty(command.Budgetkontonummer))
            {
                try
                {
                    budgetkonto = regnskab.Konti
                        .OfType<Budgetkonto>()
                        .Single(m => m.Kontonummer.CompareTo(command.Budgetkontonummer) == 0);
                }
                catch (InvalidOperationException ex)
                {
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Budgetkonto),
                                                     command.Budgetkontonummer), ex);
                }
            }
            AdresseBase adresse = null;
            if (command.AdresseId != 0)
            {
                try
                {
                    adresse = _adresseRepository.AdresseGetAll()
                        .Single(m => m.Nummer == command.AdresseId);
                }
                catch (InvalidOperationException ex)
                {
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (AdresseBase),
                                                     command.AdresseId), ex);
                }
            }
            _finansstyringRepository.BogføringslinjeAdd(command.Bogføringsdato, command.Bilag, konto, command.Tekst,
                                                        budgetkonto, command.Debit, command.Kredit, adresse);
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
