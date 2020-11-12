using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VegeStore
{
    public class Storage : List<Container>
    {
        private double tariffPrice;
        public double TariffPrice { 
            get => tariffPrice;
            set
            {
                if(value > 0)
                {
                    tariffPrice = value;
                }
                else
                {
                    throw new ArgumentException($"Неверное значение для тарифа: {tariffPrice}.");
                }
            }
        }
        private int maxCapacity;
        public double FullPrice
        {
            get
            {
                double summ = 0;
                foreach(Container container in base.ToArray())
                {
                    summ += container.PriceWDamage;
                }
                return summ;
            }
        }
        public int MaxCapacity{
            get => maxCapacity;
            set
            {
                if(value > 0)
                {
                    maxCapacity = value;
                }
                else
                {
                    throw new ArgumentException($"Неверное значение для вместимости склада: {maxCapacity}.");
                }
            }
        }
        public Storage(int tariffPrice)
        {
            TariffPrice = tariffPrice;
            MaxCapacity = (new Random()).Next(50, 151);
        }
        public Storage(int tariffPrice, int maxCapacity)
        {
            TariffPrice = tariffPrice;
            MaxCapacity = maxCapacity;
        }
        public string GetInfo()
        {
            string message = ToString() + "\n\n" + String.Join("\n", base.ToArray().Select((box, index) => (index + 1).ToString() + ")" + box));
            Program.WriteLineColor(message, ConsoleColor.DarkCyan);
            return message;
        }
        public override string ToString()
        {
            return $"Склад\n" +
                $"Вместимость {base.Count}/{MaxCapacity} контейнеров\n" +
                $"Тариф: {TariffPrice} руб\n" +
                $"Стоимость всего товара: {FullPrice:F4} руб";
        }
        public bool Add(Container containerToAdd, out Container returnedContainer)
        {
            if(base.Count + 1 <= MaxCapacity)
            {
                if(ReturnDeltaOfPriceAndTariff(containerToAdd) >= 0)
                {
                    containerToAdd.Damage = Math.Round(new Random().NextDouble() * (0.5), 4);
                    containerToAdd.SetRealPriceForContaining();
                    base.Add(containerToAdd);
                    returnedContainer = null;
                    return true;
                }
                else
                {
                    returnedContainer = null;
                    return false;
                }
            }
            else
            {
                returnedContainer = base[0];
                base[0] = containerToAdd;
                return true;
            }
        }
        public double ReturnDeltaOfPriceAndTariff(Container containerToCheck)
        {
            return containerToCheck.PriceWDamage - TariffPrice;
        }
        public new void RemoveAt(int index)
        {
            if(index < 0 || index >= MaxCapacity || index >= base.Count)
            {
                throw new IndexOutOfRangeException($"Вы не можете удалить ящик {index}, так как его не существует!");
            }
            else
            {
                base.RemoveAt(index);
            }
        }
    }
}
