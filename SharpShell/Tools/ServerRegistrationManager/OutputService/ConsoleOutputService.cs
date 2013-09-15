using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerRegistrationManager.OutputService
{
    /// <summary>
    /// Implements the <see cref="IOutputService"/> contract and writes
    /// output to the Console.
    /// </summary>
    public class ConsoleOutputService : IOutputService
    {
        /// <summary>
        /// Writes a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="log">if set to <c>true</c> the message is also logged.</param>
        public void WriteMessage(string message, bool log = false)
        {
            //  Set the colour.
            Console.ForegroundColor = ConsoleColor.Gray;
            
            //  Write the message.
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes the success.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="log">if set to <c>true</c> [log].</param>
        public void WriteSuccess(string error, bool log = false)
        {
            //  Set the colour.
            Console.ForegroundColor = ConsoleColor.Green;

            //  Write the message.
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Writes an error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="log">if set to <c>true</c> the message is also logged.</param>
        public void WriteError(string error, bool log = false)
        {
            //  Set the colour.
            Console.ForegroundColor = ConsoleColor.Red;

            //  Write the message.
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
