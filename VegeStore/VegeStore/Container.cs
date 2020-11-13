using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VegeStore
{
    public class Container
    {
        public int MaxCapacity { get; private set; }
        public double MaxWeight { get; private set; }
        private List<Box> Boxes;
        public int CurrentLoad{ get => Boxes.Count; }
        public double CurrentWeight { get => Boxes.Select(box => box.Weight).Sum(); }
        public double PriceWoDamage { get => Boxes.Select(box => box.Price).Sum(); }
        public double Damage { get; set; }
        public double PriceWDamage { get => PriceWoDamage * (1-Damage); }
        public Container()
        {
            Boxes = new List<Box>();
            MaxCapacity = new Random().Next(5, 30);
            MaxWeight = new Random().Next(70, 151);
            Damage = 0;
        }

        public Container(int numberOfBoxes, ref List<Box> boxesToAdd)
        {
            Boxes = new List<Box>();
            MaxCapacity = new Random().Next(5, 30);
            MaxWeight = new Random().Next(70, 151);
            Damage = 0;
            List<Box> sortedBoxesToAdd = boxesToAdd.OrderBy(box => box.Weight).Reverse().ToList();
            for (int curBoxIndex  = numberOfBoxes-1; curBoxIndex >= 0; --curBoxIndex)
            {
                if (AddBox(sortedBoxesToAdd[curBoxIndex]))
                {
                    sortedBoxesToAdd.RemoveAt(curBoxIndex);
                }
                else
                {
                    break;
                }
            }
            boxesToAdd = sortedBoxesToAdd;
        }

        public bool AddBox(Box boxToAdd)
        {
            if(boxToAdd.Weight + CurrentWeight <= MaxWeight)
            {
                if(CurrentLoad + 1 <= MaxCapacity)
                {
                    Boxes.Add(boxToAdd);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public List<Box> GetBoxes()
        {
            return Boxes;
        }
        public string GetInfo()
        {
            string message = ToString();
            Program.WriteLineColor(message, ConsoleColor.Yellow);
            return message;
        }
        public void SetRealPriceForContaining()
        {
            Boxes.ForEach(box=>box.PriceWDamage = box.Price * Damage);
        }
        public override string ToString()
        {
            return $"_____Контейнер_____\n" +
                $"Вместимость {CurrentLoad}/{MaxCapacity} ящиков\n" +
                $"Вес: {CurrentWeight}/{MaxWeight} кг\n" +
                $"Товара на сумму: {PriceWoDamage} руб\n" +
                $"Уровень повреждений: {Damage:F4}\n" +
                $"Цена с учетом повреждений: {PriceWDamage} руб" +
                "\n\n" + 
                String.Join("\n", Boxes.Select((box, index) => "    "+ (index).ToString() + ")" + box.ToString().Replace("\n", "\n    ")));
        }
        public static bool TryParse(string inputLine, out List<Box> boxesToReturn)
        {
            boxesToReturn = new List<Box>();
            // Container | *Price* *Weight* | *Price* *Weight* ...
            try
            {
                double tempBoxPrice, tempBoxWeight;
                string[] splittedInput = inputLine.Split('|').Select(element => element.Trim()).ToArray();
                if(splittedInput[0] == "Container")
                {
                    for(int i =1; i < splittedInput.Length; ++i)
                    {
                        string[] boxInfo = splittedInput[i].Split().Select(element => element.Trim()).ToArray();
                        tempBoxPrice = double.Parse(boxInfo[0]);
                        tempBoxWeight = double.Parse(boxInfo[1]);
                        Box tempBox = new Box(weight: tempBoxWeight, price: tempBoxPrice);
                        boxesToReturn.Add(tempBox);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
