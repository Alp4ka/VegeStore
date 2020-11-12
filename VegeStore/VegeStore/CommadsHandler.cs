using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace VegeStore
{
    public class CommadsHandler
    {
        public delegate void ToRead(ref List<Container> containers, ref Storage storage);
        public ToRead toRead;
        public string Path{ get => "./Result.txt"; }
        //public void AddContainer
        public void ReadFromFiles(ref List<Container> containers, ref Storage storage)
        {
            if (!Storage.TryParse(File.ReadAllLines("./StorageInfo.txt")[0], out storage))
            {
                Program.WriteLineColor("Ошибка в считывании информации о Storage!", ConsoleColor.Red);
                return;
            }
            else
            {
                Program.WriteLineColor("Storage создан!", ConsoleColor.Green);
                storage.GetInfo();
            }

            string[] ContainersInfoLines = File.ReadAllLines("./Containers.txt");
            for (int i = 0; i < ContainersInfoLines.Length; ++i)
            {
                List<Box> boxes;
                if (!Container.TryParse(ContainersInfoLines[i], out boxes))
                {
                    Program.WriteLineColor($"Ошибка в считывании информации о Container\n Строка: {i}!", ConsoleColor.Red);
                    return;
                }
                else
                {
                    Program.WriteLineColor($"Контейнер {i} считан!", ConsoleColor.Green);
                    containers.Add(new Container(boxes.Count, ref boxes));
                    containers.Last().GetInfo();
                    if (boxes.Count > 0)
                    {
                        Program.WriteLineColor($"Некоторые ящики не были включены в Контейнер {i}:\n", ConsoleColor.Red);
                        Program.WriteLineColor(String.Join("\n\n", boxes.Select(b => b.ToString()).ToArray()), ConsoleColor.Red);
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
        }
        public void ReadFromConsole(ref List<Container> containers, ref Storage storage)
        {
            if (!Storage.TryParse(Console.ReadLine(), out storage))
            {
                Program.WriteLineColor("Ошибка в считывании информации о Storage!", ConsoleColor.Red);
                return;
            }
            else
            {
                Program.WriteLineColor("Storage создан!", ConsoleColor.Green);
                storage.GetInfo();
            }
            string input = Console.ReadLine();
            if(input.TryParse())
            
            string[] ContainersInfoLines = File.ReadAllLines("./Containers.txt");
            for (int i = 0; i < ContainersInfoLines.Length; ++i)
            {
                List<Box> boxes;
                if (!Container.TryParse(ContainersInfoLines[i], out boxes))
                {
                    Program.WriteLineColor($"Ошибка в считывании информации о Container\n Строка: {i}!", ConsoleColor.Red);
                    return;
                }
                else
                {
                    Program.WriteLineColor($"Контейнер {i} считан!", ConsoleColor.Green);
                    containers.Add(new Container(boxes.Count, ref boxes));
                    containers.Last().GetInfo();
                    if (boxes.Count > 0)
                    {
                        Program.WriteLineColor($"Некоторые ящики не были включены в Контейнер {i}:\n", ConsoleColor.Red);
                        Program.WriteLineColor(String.Join("\n\n", boxes.Select(b => b.ToString()).ToArray()), ConsoleColor.Red);
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }

        }
        public void WriteToFile(string message)
        {
            File.AppendAllLines(Path, new string[] { message});
        }

        //true = file, false = console
        public void ChooseMethodOfInput(bool choice = false)
        {
            if (choice)
            {
                toRead = ReadFromFiles;
            }
            else
            {
                toRead = ReadFromConsole;
            }
        }
    }
}
