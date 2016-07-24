﻿#region

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using PokemonGo.RocketAPI.Logging;

#endregion

namespace PokemonGo.RocketAPI
{
    /// <summary>
    ///     Generic logger which can be used across the projects.
    ///     Logger should be set to properly log.
    /// </summary>
    public static class Logger
    {
        private static ILogger _logger;

        /// <summary>
        ///     Set the logger. All future requests to <see cref="Write(string,LogLevel,ConsoleColor)" /> will use that logger, any
        ///     old will be
        ///     unset.
        /// </summary>
        /// <param name="logger"></param>
        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
            Log($"Initializing Rocket logger at time {DateTime.Now}...");
        }

        /// <summary>
        ///     Log a specific message to the logger setup by <see cref="SetLogger(ILogger)" /> .
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="level">Optional level to log. Default <see cref="LogLevel.Info" />.</param>
        /// <param name="color">Optional. Default is automatic color.</param>
        public static void Write(string message, LogLevel level = LogLevel.Info, ConsoleColor color = ConsoleColor.Black)
        {
            if (_logger == null)
                return;
            _logger.Write(message, level, color);
            Log(string.Concat($"[{DateTime.Now.ToString("HH:mm:ss")}] ", message));
        }

        private static void Log(string message)
        {
            // maybe do a new log rather than appending?
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Logs");
            string logPath = Directory.GetCurrentDirectory() + $"\\Logs\\NecroBot-{DateTime.Today.ToString("yyyy-dd")}-{DateTime.Now.ToString("HH-mm")}.txt";
            using (var log = File.AppendText(logPath))
            {
                log.WriteLine(message);
                log.Flush();
            }
            Thread.CurrentThread.CurrentCulture = originalCulture;
        }
    }

    public enum LogLevel
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Pokestop = 3,
        Farming = 4,
        Recycling = 5,
        Berry = 6,
        Caught = 7,
        Transfer = 8,
        Evolve = 9,
        Info = 10,
        Debug = 11
    }
}
