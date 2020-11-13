﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace VegeStore
{
    public class CommadsHandler
    {
        public delegate void ToRead(ref List<Container> containers, ref Storage storage, ref List<Box> boxesToChange);
        public ToRead toRead;
        public string Path{ get => "./Result.txt"; }
        //public void AddContainer
        public void ReadFromFiles(ref List<Container> containers, ref Storage storage, ref List<Box> boxesToChange)
        {
            Program.WriteLineColor("Хочу заметить, что текстовые файлы с тестами лежат в папке Debug/netcoreapp3.1\n" +
                "StorageInfo.txt - информация о Storage с указанием его тарифа и вместимости.\n" +
                "Containers.txt - информация о Контейнерах с указанием ящиков, которые в них хотят запихнуть.\n" +
                "Result.txt - результат работы программы.\n" +
                "Actions.txt - команды, которые нужно выполнить.", ConsoleColor.Red);
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

            string[] containersInfoLines = File.ReadAllLines("./Containers.txt");
            for (int i = 0; i < containersInfoLines.Length; ++i)
            {
                List<Box> boxes;
                if (!Container.TryParse(containersInfoLines[i], out boxes))
                {
                    Program.WriteLineColor($"Ошибка в считывании информации о Контенйнере\n Строка: {i}!", ConsoleColor.Red);
                    return;
                }
                else
                {
                    Program.WriteLineColor($"Контейнер {i} считан!", ConsoleColor.Green);
                    containers.Add(new Container(boxes.Count, ref boxes));
                    containers.Last().GetInfo();
                    if (boxes.Count > 0)
                    {
                        boxesToChange.AddRange(boxes);
                        Program.WriteLineColor($"Некоторые ящики не были включены в Контейнер {i}:\n", ConsoleColor.Red);
                        Program.WriteLineColor(String.Join("\n\n", boxes.Select(b => b.ToString()).ToArray()), ConsoleColor.Red);
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }
            string[] actionsLines = File.ReadAllLines("./Actions.txt");
            for (int i = 0; i < actionsLines.Length; ++i)
            {
                if (!TryParse(actionsLines[i], ref storage, ref containers, ref boxesToChange))
                {
                    Program.WriteLineColor($"Ошибка обработки строки {i}!", ConsoleColor.Red);

                }
            }
            WriteToFile(storage.GetInfo());
        }
        public bool TryParse(string input, ref Storage storage, ref List<Container> containers, ref List<Box> boxes)
        {
            try
            {
                string[] splittedInput = input.Trim().Split(' ');
                switch (splittedInput[0])
                {
                    case "CreateBox":
                        CreateBox(ref boxes, price: double.Parse(splittedInput[1]), weight: double.Parse(splittedInput[2]));
                        break;
                    case "CreateContainer":
                        CreateContainer(ref containers);
                        break;
                    case "DeleteBox":
                        DeleteBox(ref boxes, Int32.Parse(splittedInput[1]));
                        break;
                    case "DeleteContainer":
                        DeleteContainer(ref containers, Int32.Parse(splittedInput[1]));
                        break;
                    case "DeliverContainer":
                        DeliverContainer(ref storage, Int32.Parse(splittedInput[1]), ref containers);
                        break;
                    case "BoxToContainer":
                        Container tempC = containers[Int32.Parse(splittedInput[1])];
                        BoxToContainer(ref tempC, Int32.Parse(splittedInput[2]), ref boxes);
                        break;
                    case "ContainerFromStorage":
                        ContainerFromStorage(ref storage, Int32.Parse(splittedInput[1]), ref containers);
                        break;
                    case "ShowBoxes":
                        ShowBoxes(boxes);
                        break;
                    case "ShowContainers":
                        ShowContainers(containers);
                        break;
                    case "StorageInfo":
                        StorageInfo(storage);
                        break;
                    case "Write":
                        WriteToFile(storage.GetInfo());
                        break;
                    case "Help":
                        Help();
                        break;
                    default:
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void ReadFromConsole(ref List<Container> containers, ref Storage storage, ref List<Box> boxesToChange)
        {
            Program.WriteLineColor("Хочу заметить, что текстовые файлы с тестами лежат в папке Debug/netcoreapp3.1\n" +
               "Result.txt - результат работы программы.", ConsoleColor.Red);
            Program.WriteLineColor("Первым нужно ввести Storage. Ввод должен быть в формате(звездочки не указывать):\n" +
                "Storage | *Тариф_double* *Вместимость_int*");
            while (!Storage.TryParse(Console.ReadLine(), out storage))
            {
                Program.WriteLineColor("Ошибка в считывании информации о Storage!", ConsoleColor.Red);
            }
                Program.WriteLineColor("Storage создан!", ConsoleColor.Green);
                storage.GetInfo();
            string input;
            Program.WriteLineColor("Вводите команды. Чтобы завершить работу программы, сделайте пустой ввод.");
            while(true)
            {
                Program.WriteColor(">>> ");
                input = Console.ReadLine();
                if(input == "")
                {
                    return;
                }
                if (!TryParse(input, ref storage, ref containers, ref boxesToChange))
                {
                    Program.WriteLineColor("Ошибка обработки строки!", ConsoleColor.Red);
                }
            }
        }
        public static void CreateBox(ref List<Box> boxes, double price, double weight)
        {
            Box box = new Box(weight, price);
            boxes.Add(box);
            Program.WriteLineColor("Коробка успешно добавлена:\n" +
                 $"{box}", ConsoleColor.Green);
        }
        public static void CreateContainer(ref List<Container> containers)
        {
            Container container = new Container();
            containers.Add(container);
            Program.WriteLineColor("Контейнер успешно добавлен:\n" +
                 $"{container}", ConsoleColor.Green);
        }
        public static void DeleteBox(ref List<Box> boxes, int index)
        {
            boxes.RemoveAt(index);
            Program.WriteLineColor($"Ящик {index} успешно удален!", ConsoleColor.Green);
        }
        public static void DeleteContainer(ref List<Container> containers, int index)
        {
            containers.RemoveAt(index);
            Program.WriteLineColor($"Контейнер {index} успешно удален!", ConsoleColor.Green);
        }
        public static void DeliverContainer(ref Storage storage, int index, ref List<Container> containers)
        {
            Container returnedContainer;
            storage.Add(containers[index], out returnedContainer);
            containers.RemoveAt(index);
            Program.WriteLineColor($"Контейнер {index} успешно доставлен на склад!", ConsoleColor.Green);
        }
        public static void BoxToContainer(ref Container container, int boxIndex, ref List<Box> boxes)
        {
            container.AddBox(boxes[boxIndex]);
            boxes.RemoveAt(boxIndex);
            Program.WriteLineColor($"Ящик {boxIndex} помещен в контейнер!", ConsoleColor.Green);
        }
        public static void ContainerFromStorage(ref Storage storage, int containerIndex, ref List<Container> containers)
        {
            containers.Add(storage[containerIndex]);
            storage.RemoveAt(containerIndex);
            Program.WriteLineColor($"Контейнер {containerIndex} успешно выкинули со склада и отправили на улицу!", ConsoleColor.Green);
        }
        public static void ShowBoxes(List<Box> boxes)
        {
            Program.WriteLineColor(String.Join("\n", boxes.Select((box, index) => (index).ToString() + ")" + box)), ConsoleColor.Magenta);
        }
        public static void ShowContainers(List<Container> containers)
        {
            Program.WriteLineColor(String.Join("\n", containers.Select((container, index) => (index).ToString() + ")" + container)), ConsoleColor.Yellow);
        }
        public static void StorageInfo(Storage storage)
        {
            storage.GetInfo();
        }
        public static void Help()
        {
            Program.WriteLineColor("");
        }
        public void WriteToFile(string message)
        {
            File.WriteAllText(Path,  message);
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
