using UnityEngine;

/// <summary>
/// 抽象工厂
/// </summary>
public class AbstractFactoryPattern : MonoBehaviour
{
    public interface IShape
    {
        void Draw();
    }

    public class Rectangle : IShape
    {
        public void Draw()
        {
            Debug.Log("Rectangle");
        }
    }

    public class Square : IShape
    {
        public void Draw()
        {
            Debug.Log("Square");
        }
    }

    public class Circle : IShape
    {
        public void Draw()
        {
            Debug.Log("Circle");
        }
    }





    public interface IColor
    {
        void Fill();
    }


    public class Red : IColor
    {
        public void Fill()
        {
            Debug.Log("Fill Red");
        }
    }

    public class Green : IColor
    {
        public void Fill()
        {
            Debug.Log("Fill Green");
        }
    }


    //为 Color 和 Shape 对象创建抽象类来获取工厂。
    public abstract class AbstractFactory
    {
        public abstract IColor GetColor(string color);
        public abstract IShape GetShape(string shape);
    }

    //创建扩展了 AbstractFactory 的工厂类，基于给定的信息生成实体类的对象。
    public class ShapeFactory : AbstractFactory
    {
        public override IColor GetColor(string color)
        {
            return null;
        }

        public override IShape GetShape(string shape)
        {
            switch (shape)
            {
                case "Rectangle": return new Rectangle();
                case "Square": return new Square();
                case "Circle": return new Circle();
                default: return null;
            }
        }
    }


    public class ColorFactory : AbstractFactory
    {
        public override IColor GetColor(string color)
        {

            switch (color)
            {
                case "Red": return new Red();
                case "Green": return new Green();
                default: return null;
            }
        }

        public override IShape GetShape(string shape)
        {
            return null;
        }
    }


    //创建一个工厂创造器/生成器类，通过传递形状或颜色信息来获取工厂。
    public AbstractFactory GetAbstractFactory(string choice)
    {
        switch (choice)
        {
            case "ShapeFactory": return new ShapeFactory();
            case "ColorFactory": return new ColorFactory();
            default: return null;
        }
    }


    public void Main()
    {
        var shapeFactory = GetAbstractFactory("ShapeFactory");
        var shape = shapeFactory.GetShape("Circle");
        shape.Draw();
    }
}
