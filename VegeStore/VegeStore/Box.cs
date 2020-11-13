using System;

namespace VegeStore
{
    public class Box
    {
        private double weight;
        private double pricePerKg;
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
        public double PriceWDamage { get; set; }
        public Box(double weight, double price)
        {
            Weight = weight;
            PricePerKg = price;
        }
        public string GetInfo()
        {
            Program.WriteLineColor(ToString(), ConsoleColor.Magenta);
            return ToString();
        }
        public override string ToString()
        {
            return $"_____Ящик овощей_____\n" +
                $"Вес: {Weight}кг\n" +
                $"Цена: {PricePerKg} рублей/кг\n" +
                $"Общая стоимость: {Price}";
        }
    }

}
