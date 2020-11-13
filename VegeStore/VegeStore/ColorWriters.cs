using System;

namespace VegeStore
{
    public partial class Program
    {
        /// <summary>
        /// Аналог Console.WriteLine() с возможностью выбора цвета сообщения.
        /// </summary>
        /// <param name="message"> Сообщение для печати. </param>
        /// <param name="color"> Цвет, выбранный для печати. </param>
        public static void WriteLineColor(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Аналог Console.Write() с возможностью выбора цвета сообщения.
        /// </summary>
        /// <param name="message"> Сообщение для печати. </param>
        /// <param name="color"> Цвет, выбранный для печати. </param>
        public static void WriteColor(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
