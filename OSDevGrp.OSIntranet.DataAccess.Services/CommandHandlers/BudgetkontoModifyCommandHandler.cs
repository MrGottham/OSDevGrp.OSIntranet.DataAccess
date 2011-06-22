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
    /// Commandhandler til håndtering af kommandoen: BudgetkontoModifyCommand.
    /// </summary>
    public class BudgetkontoModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BudgetkontoModifyCommand, BudgetkontoView>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IFællesRepository _fællesRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BudgetkontoModifyCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="fællesRepository">Implementering af repository til fælles elementer.</param>
        /// <param name="objectMapper">Implementering af objectmapper.</param>
        public BudgetkontoModifyCommandHandler(IFinansstyringRepository finansstyringRepository, IFællesRepository fællesRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<BudgetkontoModifyCommand,BudgetkontoView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af en given budgetkonto.</param>
        /// <returns>Opdateret budgetkonto.</returns>
        public BudgetkontoView Execute(BudgetkontoModifyCommand command)
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
            Budgetkontogruppe budgetkontogruppe;
            try
            {
                budgetkontogruppe = _finansstyringRepository.BudgetkontogrupperGetAll()
                    .Single(m => m.Nummer == command.Budgetkontogruppe);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Budgetkontogruppe),
                                                 command.Budgetkontogruppe), ex);
            }

            Budgetkonto budgetkonto;
            try
            {
                budgetkonto = regnskab.Konti
                    .OfType<Budgetkonto>()
                    .Single(m => m.Kontonummer.CompareTo(command.Kontonummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Budgetkonto),
                                                 command.Kontonummer), ex);
            }
            budgetkonto.SætKontonavn(command.Kontonavn);
            budgetkonto.SætBeskrivelse(command.Beskrivelse);
            budgetkonto.SætNote(command.Note);
            budgetkonto.SætBudgetkontogruppe(budgetkontogruppe);

            var opdateretBudgetkonto = _finansstyringRepository.BudgetkontoModify(regnskab, budgetkonto.Kontonummer,
                                                                                  budgetkonto.Kontonavn,
                                                                                  budgetkonto.Beskrivelse,
                                                                                  budgetkonto.Note,
                                                                                  budgetkonto.Budgetkontogruppe);

            return _objectMapper.Map<Budgetkonto, BudgetkontoView>(opdateretBudgetkonto);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af en given budgetkonto.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(BudgetkontoModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (BudgetkontoModifyCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
