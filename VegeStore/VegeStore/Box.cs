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
                if(price > 0)
                {
                    price = value;
                }
                else
                {
                    throw new ArgumentException($"Неверное значение для цены ящика: {value}");
                }
            }
        }
        public Box(double weight, double price)
        {
            Weight = weight;
            Price = price;
        }

        public override string ToString()
        {
            return $"Ящик овощей.\nВес: {Weight}кг\nЦена: {Price} рублей/кг";
        }
    }

}
