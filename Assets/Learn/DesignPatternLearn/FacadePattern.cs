using UnityEngine;
/// <summary>
/// 外观模式
/// </summary>
public class FacadePattern : MonoBehaviour
{
    public interface IShape
    {
        void Draw();
    }

    public class Rectangle : IShape
    {
        public void Draw()
        {
            Debug.Log("Draw Rectangle");
        }
    }

    public class Circle : IShape
    {
        public void Draw()
        {
            Debug.Log("Draw Circle");
        }
    }

    public class Square : IShape
    {
        public void Draw()
        {
            Debug.Log("Draw Square");
        }
    }

    public class ShapeMaker
    {
        private IShape _circle;
        private IShape _rectangle;
        private IShape _square;

        public ShapeMaker()
        {
            _circle = new Circle();
            _rectangle = new Rectangle();
            _square = new Square();
        }

        public void DrawCircle()
        {
            _circle.Draw();
        }

        public void DrawRectangle()
        {
            _rectangle.Draw();
        }

        public void DrawSquare()
        {
            _square.Draw();
        }
    }

    public void Main()
    {
        ShapeMaker shapeMaker = new ShapeMaker();
        shapeMaker.DrawCircle();
        shapeMaker.DrawRectangle();
        shapeMaker.DrawSquare();
    }
}
