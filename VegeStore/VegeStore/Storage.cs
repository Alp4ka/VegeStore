using System;
using System.Collections.Generic;
using System.Linq;

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
        public Storage(double tariffPrice)
        {
            TariffPrice = tariffPrice;
            MaxCapacity = (new Random()).Next(50, 151);
        }
        public Storage(double tariffPrice, int maxCapacity)
        {
            TariffPrice = tariffPrice;
            MaxCapacity = maxCapacity;
        }
        /// <summary>
        /// Возвращает информацию о складе.
        /// </summary>
        /// <returns> string с информацией. </returns>
        public string GetInfo()
        {
            string message = ToString() + "\n\n" + String.Join("\n", base.ToArray().Select((container, index) => (index).ToString() + ")" + container));
            Program.WriteLineColor(message, ConsoleColor.DarkCyan);
            return message;
        }
        /// <summary>
        /// Возвращает информацию о складе.
        /// </summary>
        /// <returns> string с информацией. </returns>
        public override string ToString()
        {
            return $"_____Склад_____\n" +
                $"Вместимость {base.Count}/{MaxCapacity} контейнеров\n" +
                $"Тариф: {TariffPrice} руб/контейнер\n" +
                $"Стоимость всего товара: {FullPrice:F4} руб";
        }
        /// <summary>
        /// Добавить контейнер.
        /// </summary>
        /// <param name="containerToAdd"></param>
        /// <param name="returnedContainer"> Контейнер, который пришлось выслат на улицу. </param>
        /// <returns></returns>
        public bool Add(Container containerToAdd, out Container returnedContainer)
        {
            if(base.Count + 1 <= MaxCapacity)
            {
                if(ReturnDeltaOfPriceAndTariff(containerToAdd) > 0)
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
                base.RemoveAt(0);
                base.Add(containerToAdd);
                return true;
            }
        }
        /// <summary>
        /// Вернуть разницу в стоиомтьс контейнера и тарифа.
        /// </summary>
        /// <param name="containerToCheck"> Контейнер. </param>
        /// <returns> double - дельта между тарифом и стоимостью контейнера. </returns>
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
        /// <summary>
        /// Пробует запарсить инфу о складе.
        /// </summary>
        /// <param name="inputLine"></param>
        /// <param name="storage"></param>
        /// <returns> true, если все ок, false - иначе. </returns>
        public static bool TryParse(string inputLine, out Storage storage)
        {
            storage = null;
            // Storage | *TariffPrice* *MaxCapacity*
            try
            {
                double tempTariffPrice; int tempMaxCapacity;
                string[] splittedInput = inputLine.Split('|').Select(element => element.Trim()).ToArray();
                if (splittedInput[0] == "Storage")
                {
                    string[] storageInfo = splittedInput[1].Split().Select(element => element.Trim()).ToArray();
                    tempTariffPrice = double.Parse(storageInfo[0]);
                    tempMaxCapacity = Int32.Parse(storageInfo[1]);
                    storage = new Storage(tempTariffPrice, tempMaxCapacity);
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
