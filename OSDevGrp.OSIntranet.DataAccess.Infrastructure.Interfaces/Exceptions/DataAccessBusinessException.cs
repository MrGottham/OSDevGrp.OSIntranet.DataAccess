using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Businessexception.
    /// </summary>
    public class DataAccessBusinessException : Exception
    {
        /// <summary>
        /// Danner en businessexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public DataAccessBusinessException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Danner en businessexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public DataAccessBusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Danner en businessexception.
        /// </summary>
        /// <param name="info">Serialation information.</param>
        /// <param name="context">Streaming context.</param>
        protected DataAccessBusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
