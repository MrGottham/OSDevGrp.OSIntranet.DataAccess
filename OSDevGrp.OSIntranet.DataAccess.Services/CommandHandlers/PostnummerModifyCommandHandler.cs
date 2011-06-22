using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
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
    /// Commandhandler til håndtering af kommandoen: PostnummerModifyCommand.
    /// </summary>
    public class PostnummerModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<PostnummerModifyCommand, PostnummerView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: PostnummerModifyCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public PostnummerModifyCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _adresseRepository = adresseRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region ICommandHandler<PostnummerModifyCommand, PostnummerView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af et givent postnummer.</param>
        /// <returns>Opdateret postnummer.</returns>
        public PostnummerView Execute(PostnummerModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Postnummer postnummer;
            try
            {
                postnummer = _adresseRepository.PostnummerGetAll()
                    .Single(m =>
                            m.Landekode.CompareTo(command.Landekode) == 0 &&
                            m.Postnr.CompareTo(command.Postnummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Postnummer),
                                                 string.Format("{0}-{1}", command.Landekode, command.Postnummer)), ex);
            }
            postnummer.SætBy(command.Bynavn);

            var opdateretPostnummer = _adresseRepository.PostnummerModify(postnummer.Landekode, postnummer.Postnr,
                                                                          postnummer.By);

            return _objectMapper.Map<Postnummer, PostnummerView>(opdateretPostnummer);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af et givent postnummer.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(PostnummerModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (PostnummerModifyCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
