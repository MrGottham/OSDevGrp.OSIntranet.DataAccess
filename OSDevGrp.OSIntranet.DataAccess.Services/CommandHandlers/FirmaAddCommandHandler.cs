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
    /// Commandhandler til håndtering af kommandoen: FirmaAddCommand.
    /// </summary>
    public class FirmaAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<FirmaAddCommand, FirmaView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: FirmaAddCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public FirmaAddCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<FirmaAddCommand,FirmaView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til tilføjelse af et firma.</param>
        /// <returns>Oprettet firma.</returns>
        public FirmaView Execute(FirmaAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Adressegruppe adressegruppe;
            try
            {
                adressegruppe = _adresseRepository.AdressegruppeGetAll().Single(m => m.Nummer == command.Adressegruppe);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Adressegruppe),
                                                 command.Adressegruppe), ex);
            }
            Betalingsbetingelse betalingsbetingelse = null;
            if (command.Betalingsbetingelse != 0)
            {
                try
                {
                    betalingsbetingelse = _adresseRepository.BetalingsbetingelserGetAll()
                        .Single(m => m.Nummer == command.Betalingsbetingelse);
                }
                catch (InvalidOperationException ex)
                {
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId,
                                                     typeof(Betalingsbetingelse), command.Betalingsbetingelse), ex);
                }
            }

            var firma = new Firma(0, command.Navn, adressegruppe);
            firma.SætAdresseoplysninger(command.Adresse1, command.Adresse2, command.PostnummerBy);
            firma.SætTelefon(command.Telefon1, command.Telefon2, command.Telefax);
            firma.SætBekendtskab(command.Bekendtskab);
            firma.SætMailadresse(command.Mailadresse);
            firma.SætWebadresse(command.Webadresse);
            firma.SætBetalingsbetingelse(betalingsbetingelse);
            firma.SætUdlånsfrist(command.Udlånsfrist);
            firma.SætFilofaxAdresselabel(command.FilofaxAdresselabel);

            var oprettetFirma = _adresseRepository.FirmaAdd(firma.Navn, firma.Adresse1, firma.Adresse2, firma.PostnrBy,
                                                            firma.Telefon1, firma.Telefon2, firma.Telefax,
                                                            firma.Adressegruppe, firma.Bekendtskab, firma.Mailadresse,
                                                            firma.Webadresse, firma.Betalingsbetingelse,
                                                            firma.Udlånsfrist, firma.FilofaxAdresselabel);

            return _objectMapper.Map<Firma, FirmaView>(oprettetFirma);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til tilføjelse af et firma.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(FirmaAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (FirmaAddCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
