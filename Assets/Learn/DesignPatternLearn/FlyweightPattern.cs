using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 享元模式 
/// </summary>
public class FlyweightPattern : MonoBehaviour
{
    public interface IShape
    {
        void Draw();
    }

    public class Circle : IShape
    {
        private string _color;
        private int _x;
        private int _y;
        private int _radius;

        public Circle(string color)
        {
            _color = color;
        }

        public void SetX(int x)
        {
            _x = x;
        }

        public void SetY(int y)
        {
            _y = y;
        }

        public void SetRadius(int radius)
        {
            _radius = radius;
        }

        public void Draw()
        {
            Debug.Log("Draw Circle"
                + "color:" + _color
                + "x:" + _x
                + "y:" + _y
                + "radius:" + _radius);
        }
    }

    public class ShapeFactory
    {
        private static Dictionary<string, IShape> _circleDict = new Dictionary<string, IShape>();

        public static IShape GetCircle(string color)
        {
            IShape shape;
            if (_circleDict.TryGetValue(color, out shape))
            {
                return shape;
            }
            else
            {
                shape = new Circle(color);
                _circleDict.Add(color, shape);
            }
            return shape;
        }
    }

    private static string[] _colors = new string[] { "Red", "Green", "Blue" };
    public void Main()
    {
        for (int i = 0; i < 20; i++)
        {
            Circle circle = (Circle)ShapeFactory.GetCircle(_colors[Random.Range(0, 3)]);
            circle.SetX(Random.Range(1, 100));
            circle.SetY(Random.Range(1, 100));
            circle.SetRadius(Random.Range(1, 100));
            circle.Draw();
        }
    }
}
