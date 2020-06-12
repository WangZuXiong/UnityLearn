using UnityEngine;
/// <summary>
/// 访问者模式
/// </summary>
public class VisitorPattern : MonoBehaviour
{
    //定义一个表示元素的接口。
    public interface IComputerPart
    {
        void Accept(IComputerPartVisitor computerPartVisitor);
    }

    public interface IComputerPartVisitor
    {
        void Visit(Keyboard keyboard);
        void Visit(Monitor monitor);
        void Visit(Mouse mouse);
        void Visit(Computer computer);
    }

    //创建扩展了上述类的实体类。
    public class Keyboard : IComputerPart
    {
        public void Accept(IComputerPartVisitor computerPartVisitor)
        {
            computerPartVisitor.Visit(this);
        }
    }

    public class Monitor : IComputerPart
    {
        public void Accept(IComputerPartVisitor computerPartVisitor)
        {
            computerPartVisitor.Visit(this);
        }
    }

    public class Mouse : IComputerPart
    {
        public void Accept(IComputerPartVisitor computerPartVisitor)
        {
            computerPartVisitor.Visit(this);
        }
    }

    public class Computer : IComputerPart
    {
        private IComputerPart[] _computerParts;

        public Computer()
        {
            _computerParts = new IComputerPart[] { new Mouse(), new Keyboard(), new Monitor() };
        }

        public void Accept(IComputerPartVisitor computerPartVisitor)
        {
            for (int i = 0; i < _computerParts.Length; i++)
            {
                _computerParts[i].Accept(computerPartVisitor);
            }
            computerPartVisitor.Visit(this);
        }
    }

    public class ComputerPartDisplayVisitor : IComputerPartVisitor
    {
        public void Visit(Keyboard keyboard)
        {
            Debug.Log("Visit keyboard");
        }

        public void Visit(Monitor monitor)
        {
            Debug.Log("Visit monitor");
        }

        public void Visit(Mouse mouse)
        {
            Debug.Log("Visit mouse");
        }

        public void Visit(Computer computer)
        {
            Debug.Log("Visit computer");
        }
    }

    public void Main()
    {
        IComputerPart part = new Computer();
        part.Accept(new ComputerPartDisplayVisitor());
    }
}
