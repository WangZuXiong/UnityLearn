using UnityEngine;

/// <summary>
/// 桥接模式
/// </summary>
public class Bridge : MonoBehaviour
{
    public interface IDraw
    {
        void Draw(int radius, int x, int y);
    }

    public class RedCircle : IDraw
    {
        public void Draw(int radius, int x, int y)
        {
            Debug.Log("Red:" + "radius:" + radius + "x:" + x + "y:" + y);
        }
    }

    public class GreenCircle : IDraw
    {
        public void Draw(int radius, int x, int y)
        {
            Debug.Log("Green:" + "radius:" + radius + "x:" + x + "y:" + y);
        }
    }


    public abstract class Shape
    {
        protected IDraw draw;

        public Shape(IDraw draw)
        {
            this.draw = draw;
        }

        public abstract void Draw();
    }

    public class Circle : Shape
    {
        private int _radius;
        private int _x;
        private int _y;
        private IDraw _draw;
        public Circle(int radius, int x, int y, IDraw draw) : base(draw)
        {
            _radius = radius;
            _x = x;
            _y = y;
            _draw = draw;
        }

        public override void Draw()
        {
            _draw.Draw(_radius, _x, _y);
        }
    }


    public void Main()
    {
        Circle redCircle = new Circle(5, 0, 0, new RedCircle());
        Circle greenCircle = new Circle(5, 0, 0, new GreenCircle());
        redCircle.Draw();
        greenCircle.Draw();
    }
}
