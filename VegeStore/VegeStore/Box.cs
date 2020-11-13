using System;

namespace VegeStore
{
    public class Box
    {
        private double weight;
        private double pricePerKg;
        /// <summary>
        /// Вес ящика.
        /// </summary>
        public double Weight {
            get => weight;
            private set
            {
                if (value > 0)
                {
                    weight = value;
                }
                else
                {
                    throw new ArgumentException($"Неверное значение для веса ящика: {value}");
                }
            }
        }
        /// <summary>
        /// Цена за кг.
        /// </summary>
        public double PricePerKg {
            get => pricePerKg;
            set
            {
                if(value > 0)
                {
                    pricePerKg = value;
                }
                else
                {
                    throw new ArgumentException($"Неверное значение для цены за кг: {value}");
                }
            }
        }
        public double Price { get => PricePerKg * Weight; }
        /// <summary>
        /// Цена с учетом повреждений.
        /// </summary>
        public double PriceWDamage { get; set; }
        /// <summary>
        /// Конструктор ящика.
        /// </summary>
        /// <param name="weight"> Требуемый вес. </param>
        /// <param name="price"> Требуемая цена. </param>
        public Box(double weight, double price)
        {
            Weight = weight;
            PricePerKg = price;
        }
        /// <summary>
        /// Вывод информации о ящике.
        /// </summary>
        /// <returns> Возвращает string с информацией о ящике. </returns>
        public string GetInfo()
        {
            Program.WriteLineColor(ToString(), ConsoleColor.Magenta);
            return ToString();
        }
        /// <summary>
        /// Информация о ящике без вывода на экран.
        /// </summary>
        /// <returns> Возвращает string с информацией о ящике.</returns>
        public override string ToString()
        {
            return $"_____Ящик овощей_____\n" +
                $"Вес: {Weight}кг\n" +
                $"Цена: {PricePerKg} рублей/кг\n" +
                $"Общая стоимость: {Price}";
        }
    }

}
