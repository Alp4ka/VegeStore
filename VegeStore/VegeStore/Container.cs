using System;
using System.Collections.Generic;
using System.Text;

namespace VegeStore
{
    public class Container
    {
        public int Capacity { get; private set; }
        private List<Box> Boxes = new List<Box>();
        public Container()
        {
            Capacity = new Random().Next(50, 101);
        }
    }
}
