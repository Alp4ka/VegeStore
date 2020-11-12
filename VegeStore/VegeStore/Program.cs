using System;
using System.Collections.Generic;
using System.Security;
using System.IO;
using System.Linq;

namespace VegeStore
{
    public partial class Program
    {
        
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            Storage storage = null;
            List<Container> containers = new List<Container>();
            CommadsHandler handler = new CommadsHandler();
            
            Program.WriteLineColor("Введите 1, если хотите считывать информацию о складе из файлов; 0, если из консоли.");
            string input = Console.ReadLine();
            while(true)
            {
                if(input == "1" || input == "0")
                {
                    break;
                }
                Program.WriteLineColor("Вы ошиблись вводом.", ConsoleColor.Red);
                input = Console.ReadLine();
            }
            handler.ChooseMethodOfInput(input == "1"? true: false);
            handler.toRead(ref containers, ref storage);
           
        }
    }
}
