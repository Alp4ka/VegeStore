using System;
using System.Collections.Generic;
namespace VegeStore
{
    public partial class Program
    {
        
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            List<Box> boxesRemain = new List<Box>();
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
            Console.Clear();
            handler.ChooseMethodOfInput(input == "1"? true: false);
            handler.toRead(ref containers, ref storage, ref boxesRemain);
        }
    }
}
