using System;
using System.Diagnostics;

namespace DevServer
{
    public static class Log
    {
        public static void WriteSuccess(string format, params object[] args)
        {
            WriteTime();

            Console.ForegroundColor = ConsoleColor.Green;

            WriteLine(format, args);
        }

        public static void WriteWarning(string format, params object[] args)
        {
            WriteTime();

            Console.ForegroundColor = ConsoleColor.Yellow;

            WriteLine(format, args);
        }

        public static void WriteError(string format, params object[] args)
        {
            WriteTime();

            Console.ForegroundColor = ConsoleColor.Red;

            WriteLine(format, args);
        }

        public static void WriteInfo(string format, params object[] args)
        {
            WriteTime();

            WriteLine(format, args);
        }

        private static void WriteTime()
        {
            Console.Write("[{0}] ", DateTime.Now.ToLongTimeString());
        }

        private static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format.Replace("\n", "\n\t"), args);

            Console.ResetColor();

            Debug.WriteLine("[{0}] {1}", DateTime.Now.ToLongTimeString(), string.Format(format.Replace("\n", "\n\t\t"), args));
        }
    }
}