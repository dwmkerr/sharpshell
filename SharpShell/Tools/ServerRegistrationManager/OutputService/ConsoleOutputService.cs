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
        public void WriteMessage(string message)
        {
            //  Set the colour.
            Console.ForegroundColor = ConsoleColor.Gray;
            
            //  Write the message.
            Console.WriteLine(message);
        }

        /// <summary>
        /// Writes an error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void WriteError(string error)
        {
            //  Set the colour.
            Console.ForegroundColor = ConsoleColor.Red;

            //  Write the message.
            Console.WriteLine(error);
        }
    }
}
