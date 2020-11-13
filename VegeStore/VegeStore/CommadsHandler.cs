using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace VegeStore
{
    public class CommadsHandler
    {
        public delegate void ToRead(ref List<Container> containers, ref Storage storage, ref List<Box> boxesToChange);
        public ToRead toRead;
        /// <summary>
        /// Путь до Result.txt.
        /// </summary>
        public string Path{ get => "./Result.txt"; }
        /// <summary>
        /// Если пользователь выбрал работу с файлами, данный метод запускает главный цикл в этом режиме.
        /// </summary>
        /// <param name="containers"> Лист с контейнерами, хранящимися на улице. </param>
        /// <param name="storage"> Склад. </param>
        /// <param name="boxesToChange"> Лист с коробками, хранящимися на улице. </param>
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
                Program.WriteLineColor($">>> {actionsLines[i]}");
                if (!TryParse(actionsLines[i], ref storage, ref containers, ref boxesToChange))
                {
                    Program.WriteLineColor($"Ошибка обработки строки {i}! Help для помощи. ", ConsoleColor.Red);

                }
            }
            WriteToFile(storage.GetInfo());
        }
        public void Start(ref List<Container> containers, ref Storage storage, ref List<Box> boxesRemain)
        {
            toRead(ref containers, ref storage, ref boxesRemain);
        }
        /// <summary>
        /// Пробует понять, че хочет пользователь.
        /// </summary>
        /// <param name="input"> Строка с входными данными. </param>
        /// <param name="storage"> Склад. </param>
        /// <param name="containers"> Лист с контейнерами, хранящимися на улице. </param>
        /// <param name="boxes"> Лист с коробками, хранящимися на улице. </param>
        /// <returns> true, если входные данные оказались адекватными, false - в противном случае. </returns>
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
            catch (IndexOutOfRangeException)
            {
                Program.WriteLineColor("Неверное количество аргументов!", ConsoleColor.Red);
                return false;
            }
            catch (Exception ex)
            {
                Program.WriteLineColor(ex.Message, ConsoleColor.Red);
                return false;
            }
        }
        /// <summary>
        /// Если пользователь выбрал работу с консолью, данный метод запускает главный цикл в этом режиме.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="storage"></param>
        /// <param name="boxesToChange"></param>
        public void ReadFromConsole(ref List<Container> containers, ref Storage storage, ref List<Box> boxesToChange)
        {
            Program.WriteLineColor("Хочу заметить, что текстовые файлы с тестами лежат в папке Debug/netcoreapp3.1\n" +
               "Result.txt - результат работы программы.", ConsoleColor.Red);
            Program.WriteLineColor("Первым нужно ввести Storage. Ввод должен быть в формате(звездочки не указывать):\n" +
                "Storage | *Тариф_double* *Вместимость_int*");
            while (!Storage.TryParse(Console.ReadLine(), out storage))
            {
                Program.WriteLineColor("Ошибка в считывании информации о Storage! Введите повторно", ConsoleColor.Red);
            }
                Program.WriteLineColor("Storage создан!", ConsoleColor.Green);
                storage.GetInfo();
            string input;
            Help();
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
                    Program.WriteLineColor("Ошибка обработки строки! Help для помощи", ConsoleColor.Red);
                }
            }
        }
        /// <summary>
        /// Создать ящик.
        /// </summary>
        /// <param name="boxes"> Лист с коробками, хранящимися на улице. </param>
        /// <param name="price"> Требуемая цена ящика. </param>
        /// <param name="weight"> Требуемый вес ящика. </param>
        public static void CreateBox(ref List<Box> boxes, double price, double weight)
        {
            Box box = new Box(weight, price);
            boxes.Add(box);
            Program.WriteLineColor("Коробка успешно добавлена:\n" +
                 $"{box}", ConsoleColor.Green);
        }
        /// <summary>
        /// Создать ящик.
        /// </summary>
        /// <param name="containers"> Лист с контейнерами, хранящимися на улице. </param>
        public static void CreateContainer(ref List<Container> containers)
        {
            Container container = new Container();
            containers.Add(container);
            Program.WriteLineColor("Контейнер успешно добавлен:\n" +
                 $"{container}", ConsoleColor.Green);
        }
        /// <summary>
        /// Утилизировать уличный ящик с нуждным индексом.
        /// </summary>
        /// <param name="boxes"> Лист с ящиками. </param>
        /// <param name="index"> Индекс ящика. </param>
        public static void DeleteBox(ref List<Box> boxes, int index)
        {
            boxes.RemoveAt(index);
            Program.WriteLineColor($"Ящик {index} успешно удален!", ConsoleColor.Green);
        }
        /// <summary>
        /// Утилизровать контейнер с нужным индексом.
        /// </summary>
        /// <param name="containers"> Лист с контейнерами. </param>
        /// <param name="index"> Индекс контейнера. </param>
        public static void DeleteContainer(ref List<Container> containers, int index)
        {
            containers.RemoveAt(index);
            Program.WriteLineColor($"Контейнер {index} успешно удален!", ConsoleColor.Green);
        }
        /// <summary>
        /// Доставить контейнер на склад.
        /// </summary>
        /// <param name="storage"> Склад. </param>
        /// <param name="index"> Индекс контейнера на улице. </param>
        /// <param name="containers"> Лист с контейнеарми. </param>
        public static void DeliverContainer(ref Storage storage, int index, ref List<Container> containers)
        {
            Container returnedContainer;
            storage.Add(containers[index], out returnedContainer);
            containers.RemoveAt(index);
            Program.WriteLineColor($"Контейнер {index} успешно доставлен на склад!", ConsoleColor.Green);
        }
        /// <summary>
        /// Положить ящик в контейнер.
        /// </summary>
        /// <param name="container"> Контейнер. </param>
        /// <param name="boxIndex"> Индекс ящика на улице. </param>
        /// <param name="boxes"> Лист с ящиками. </param>
        public static void BoxToContainer(ref Container container, int boxIndex, ref List<Box> boxes)
        {
            if (container.AddBox(boxes[boxIndex]))
            {
                boxes.RemoveAt(boxIndex);
                Program.WriteLineColor($"Ящик {boxIndex} помещен в контейнер!", ConsoleColor.Green);
            }
            Program.WriteLineColor($"Похоже, что ящик {boxIndex} остался на улице!", ConsoleColor.Red);
        }
        /// <summary>
        /// Вышвырнуть контейнер со склада.
        /// </summary>
        /// <param name="storage"> Склад. </param>
        /// <param name="containerIndex">Индекс контейнера на складе. </param>
        /// <param name="containers"> Лист с контейнерами. </param>
        public static void ContainerFromStorage(ref Storage storage, int containerIndex, ref List<Container> containers)
        {
            containers.Add(storage[containerIndex]);
            storage.RemoveAt(containerIndex);
            Program.WriteLineColor($"Контейнер {containerIndex} успешно выкинули со склада и отправили на улицу!", ConsoleColor.Green);
        }
        /// <summary>
        /// Показать список ящиков на улице.
        /// </summary>
        /// <param name="boxes"> Лист ящиков. </param>
        public static void ShowBoxes(List<Box> boxes)
        {
            Program.WriteLineColor(String.Join("\n", boxes.Select((box, index) => (index).ToString() + ")" + box)), ConsoleColor.Magenta);
        }
        /// <summary>
        /// Показать список контейнеров на улице.
        /// </summary>
        /// <param name="containers"> Лист контейнеров. </param>
        public static void ShowContainers(List<Container> containers)
        {
            Program.WriteLineColor(String.Join("\n", containers.Select((container, index) => (index).ToString() + ")" + container)), ConsoleColor.Yellow);
        }
        /// <summary>
        /// Показать информацию о складе.
        /// </summary>
        /// <param name="storage"></param>
        public static void StorageInfo(Storage storage)
        {
            storage.GetInfo();
        }
        /// <summary>
        /// Метод с выводом всех доступных команд.
        /// </summary>
        public static void Help()
        {
            Program.WriteLineColor("Список команд, которые могут тебе помочь указаны ниже(звездочки писать не надо):\n" +
                "CreateBox *ценаЗаКг* *вес* - создает новый ящик с овощами, отправляет на уличное хранение.\n" +
                "CreateContainer - создает новый контейнер со случайными параметрами, отправляет на уличное хранение.\n" +
                "ShowBoxes - показать все ящики на улице с их индексами.\n" +
                "ShowContainers - показать все контейнеры, хранящиеся на улице.\n" +
                "DeleteContainer *индексКонтейнераНаУлице* - утилизировать контейнер с нужным индексом.\n" +
                "DeleteBox *индексЯщикаНаУлице* - утилизировать ящик с нужным индексом.\n" +
                "BoxToContainer *индексКонтейнераНаУлице* *индексЯщикаНаУлице* - переложить ящик в контейнер.\n" +
                "DeliverContainer *индексКонтейнераНаУлице* - доставить контейнер на склад.\n" +
                "ContainerFromStorage *индексКонтейнераНаСкладе* - вышвырнуть контейнер со склада.\n" +
                "StorageInfo - вывести информацию о складе.\n" +
                "Write - записать ВСЕ изменения в файл Result.txt.\n" +
                "Help - вывести списоке команд с их описанием.\n", ConsoleColor.Green);
        }
        /// <summary>
        /// Запись в файл Result.txt.
        /// </summary>
        /// <param name="message"> Что нужно записать. </param>
        public void WriteToFile(string message)
        {
            File.WriteAllText(Path,  message);
        }

        /// <summary>
        /// Выбрать метод чтения.
        /// </summary>
        /// <param name="choice"> true - чтение из файла. false - чтение из консоли. </param>
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
