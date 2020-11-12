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
            MaxCapacity = new Random().Next(50, 101);
            MaxWeight = new Random().Next(70, 151);
            Damage = 0;
        }

        public Container(int numberOfBoxes, ref List<Box> boxesToAdd)
        {
            Boxes = new List<Box>();
            MaxCapacity = new Random().Next(50, 101);
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
            string message = ToString() + "\n\n" + String.Join("\n", Boxes.Select((box, index) => (index+1).ToString() + ")" + box));
            Program.WriteLineColor(message, ConsoleColor.Yellow);
            return message;
        }
        public void SetRealPriceForContaining()
        {
            Boxes.ForEach(box=>box.PriceWDamage = box.Price * Damage);
        }
        public override string ToString()
        {
            return $"Контейнер\n" +
                $"Вместимость {CurrentLoad}/{MaxCapacity} ящиков\n" +
                $"Вес: {CurrentWeight}/{MaxWeight} кг\n" +
                $"Товара на сумму: {PriceWoDamage} руб\n" +
                $"Уровень повреждений: {Damage:F4}\n" +
                $"Цена с учетом повреждений: {PriceWDamage} руб";
        }
    }
}
