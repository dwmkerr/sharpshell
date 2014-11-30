using System;
using System.Runtime.Serialization;

namespace SharpShell.Exceptions
{
    /// <summary>
    /// An exception that can be thrown during server registration. Typically thrown if
    /// servers are misconfigured.
    /// </summary>
    [Serializable]
    public class ServerRegistrationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ServerRegistrationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerRegistrationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ServerRegistrationException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerRegistrationException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ServerRegistrationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}