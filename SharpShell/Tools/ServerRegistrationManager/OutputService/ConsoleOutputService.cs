using SharpShell.Diagnostics;
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

            //  If required, write to the SharpShell log.
            if (log)
                Logging.Log(message);
        }

        /// <summary>
        /// Writes the success.
        /// </summary>
        /// <param name="messabe">The message.</param>
        /// <param name="log">if set to <c>true</c> [log].</param>
        public void WriteSuccess(string message, bool log = false)
        {
            //  Set the colour.
            Console.ForegroundColor = ConsoleColor.Green;

            //  Write the message.
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;

            //  If required, write to the SharpShell log.
            if (log)
                Logging.Log(message);
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

            //  If required, write to the SharpShell log.
            if (log)
                Logging.Error(error);
        }
    }
}
