using UnityEngine;
/// <summary>
/// 工厂模式
/// </summary>
public class FactoryPattern : MonoBehaviour
{
    public interface IShape
    {
        void Draw();
    }

    public class Rectangle : IShape
    {
        public void Draw()
        {
            Debug.Log("Inside Rectangle::draw() method.");
        }
    }

    public class Square : IShape
    {
        public void Draw()
        {
            Debug.Log("Inside Square::draw() method.");
        }
    }

    public class Circle : IShape
    {
        public void Draw()
        {
            Debug.Log("Inside Circle::draw() method.");
        }
    }


    public class ShapeFactory
    {
        public IShape GetShape(string shapeType)
        {

            switch (shapeType)
            {
                case "CIRCLE": return new Circle();
                case "RECTANGLE": return new Rectangle();
                case "SQUARE": return new Square();
                default: return null;
            }
        }
    }

    public void Main()
    {
        ShapeFactory shapeFactory = new ShapeFactory();
        IShape shape = shapeFactory.GetShape("CIRCLE");
        shape.Draw();
    }
}
