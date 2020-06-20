using UnityEngine;

/// <summary>
/// 解释器模式
/// </summary>
public class InterpreterPattern : MonoBehaviour
{
    public interface IExpression
    {
        bool Interpret(string context);
    }

    public class TerminalExpression : IExpression
    {
        private string _data;

        public TerminalExpression(string data)
        {
            _data = data;
        }

        public bool Interpret(string context)
        {
            return context.Contains(_data);
        }
    }

    public class OrExpression : IExpression
    {
        private IExpression _expression1;
        private IExpression _expression2;

        public OrExpression(IExpression expression1, IExpression expression2)
        {
            _expression1 = expression1;
            _expression2 = expression2;
        }

        public bool Interpret(string context)
        {
            return _expression1.Interpret(context) || _expression2.Interpret(context);
        }
    }

    public class AndExpression : IExpression
    {
        private IExpression _expression1;
        private IExpression _expression2;

        public AndExpression(IExpression expression1, IExpression expression2)
        {
            _expression1 = expression1;
            _expression2 = expression2;
        }

        public bool Interpret(string context)
        {
            return _expression1.Interpret(context) && _expression2.Interpret(context);
        }
    }


    public IExpression GetMaleExpression()
    {
        IExpression robert = new TerminalExpression("Robert");
        IExpression john = new TerminalExpression("John");
        return new OrExpression(robert, john);
    }

    public IExpression GetMarriedWomanExpression()
    {
        IExpression julie = new TerminalExpression("Julie");
        IExpression married = new TerminalExpression("Married");
        return new AndExpression(julie, married);
    }

    public void Main()
    {
        IExpression isMale = GetMaleExpression();
        IExpression isMarriedWoman = GetMarriedWomanExpression();

        Debug.Log("John is male? " + isMale.Interpret("John"));
        Debug.Log("Juile is a married women? " + isMarriedWoman.Interpret("Married Julie"));
    }
}
