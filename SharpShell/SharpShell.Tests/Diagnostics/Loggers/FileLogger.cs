using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SharpShell.Diagnostics.Loggers;

namespace SharpShell.Tests.Diagnostics.Loggers
{
    public class FileLoggerTests
    {
        private string LogLine(string message)
        {
            //  Create a temp file path and a file logger.
            var tempPath = Path.GetTempFileName();
            var logger = new FileLogger(tempPath);

            //  Write a message, grab the content.
            logger.LogMessage(message);
            var lines = File.ReadAllLines(tempPath);

            //  Clean up the file and return the log message.
            File.Delete(tempPath);
            return lines.Any() ? lines.Last() : string.Empty;
        }

        [Test]
        public void Log_Starts_With_A_DateTime()
        {
            //  Log a line of text.
            var line = LogLine("Test message");

            //  Expect an ISO8601-ish datetime (ish as we have a space rather than a 'T'.
            var rexDatetime = new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}\:\d{2}\:\d{2}");
            Assert.That(rexDatetime.Matches(line ?? string.Empty).Count, Is.GreaterThan(0));
        }

        [Test]
        public void Log_Includes_Process_Name()
        {
            //  Log a line of text.
            var line = LogLine("Test message");

            //  Expect the process name is present.
            Assert.That(line.IndexOf(Process.GetCurrentProcess().ProcessName, StringComparison.Ordinal), Is.Not.EqualTo(-1));
        }

        [Test]
        public void Log_Includes_Message()
        {
            //  Log a line of text.
            var line = LogLine("Test message");

            //  Expect the process name is present.
            Assert.That(line.IndexOf("Test message", StringComparison.Ordinal), Is.Not.EqualTo(-1));
        }
    }
}