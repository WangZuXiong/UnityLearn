using UnityEngine;

public class StrategyPattern : MonoBehaviour
{
    public interface IStrategy
    {
        int DoOperation(int num1, int num2);
    }

    public class OperationAdd : IStrategy
    {
        public int DoOperation(int num1, int num2)
        {
            return num1 + num2;
        }
    }

    public class OperationSubtract : IStrategy
    {
        public int DoOperation(int num1, int num2)
        {
            return num1 + num2;
        }
    }

    public class OperationMultiply : IStrategy
    {
        public int DoOperation(int num1, int num2)
        {
            return num1 * num2;
        }
    }

    public class Context
    {
        private IStrategy _strategy;

        public Context(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public int ExecuteStrategy(int num1, int num2)
        {
            return _strategy.DoOperation(num1, num2);
        }
    }

    public void Main()
    {
        Context context = new Context(new OperationAdd());
        Debug.Log("1 + 1 = " + context.ExecuteStrategy(1, 1));

        Context context2 = new Context(new OperationSubtract());
        Debug.Log("1 - 1 = " + context2.ExecuteStrategy(1, 1));

        Context context3 = new Context(new OperationMultiply());
        Debug.Log("1 * 1 = " + context3.ExecuteStrategy(1, 1));
    }
}
