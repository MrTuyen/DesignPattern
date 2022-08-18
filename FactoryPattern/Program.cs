using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            ShapeFactory shapeFactory = new ShapeFactory();
            IShape circle = shapeFactory.GetShape("Circle");
            circle.Draw();

            Console.ReadLine();
        }
    }

    public interface IShape
    {
        void Draw();
    }

    public class Rectangle : IShape
    {
        void IShape.Draw()
        {
            Console.WriteLine("Rectangle");
        }
    }

    public class Circle : IShape
    {
        void IShape.Draw()
        {
            Console.WriteLine("Circle");
        }
    }

    public class ShapeFactory
    {
        public IShape GetShape(String shapeType)
        {
            if (shapeType == null)
            {
                return null;
            }
            if (shapeType.Equals("Circle"))
            {
                return new Circle();

            }
            else if (shapeType.Equals("Rectangle"))
            {
                return new Rectangle();

            }

            return null;
        }
    }
}
