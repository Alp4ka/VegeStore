using System;
using System.Collections.Generic;
using System.Security;

namespace VegeStore
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            List<Box> boxes = new List<Box>();
            Storage storage = new Storage(200, 2);
            Container returnedContainer;
            Box box1 = new Box(12, 345);
            Box box2 = new Box(45, 765);
            Box box3 = new Box(1, 1);
            Box box4 = new Box(125, 465);
            boxes.Add(box1);
            boxes.Add(box2);
            boxes.Add(box3);
            boxes.Add(box4);

            Container container = new Container(boxes.Count, ref boxes);
            storage.Add(container, out returnedContainer);
            storage.GetInfo();
            storage[0].GetInfo();
        }
    }
}
