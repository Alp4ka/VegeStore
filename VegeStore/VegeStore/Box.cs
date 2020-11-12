using System;

namespace VegeStore
{
    public class Box
    {
        private double weight;
        private double price;
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
        public double Price {
            get => price;
            set
            {
                if(value > 0)
                {
                    price = value;
                }
                else
                {
                    throw new ArgumentException($"Неверное значение для цены ящика: {value}");
                }
            }
        }
        public double PriceWDamage { get; set; }
        public Box(double weight, double price)
        {
            Weight = weight;
            Price = price;
        }

        public override string ToString()
        {
            return $"Ящик овощей\n" +
                $"Вес: {Weight}кг\n" +
                $"Цена: {Price} рублей/кг";
        }
    }

}
