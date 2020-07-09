using UnityEngine;

public class CalculateDemo : MonoBehaviour
{
    public abstract class Calculate
    {
        public int Num1;
        public int Num2;

        public abstract int Compute();
    }

    public class Addition : Calculate
    {
        public override int Compute()
        {
            return Num1 + Num2;
        }
    }

    public class Subtraction : Calculate
    {
        public override int Compute()
        {
            return Num1 - Num2;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="operation">运算符</param>
    /// <returns></returns>
    public Calculate GetCalculate(string operation)
    {
        Calculate calculate = null;
        switch (operation)
        {
            case "+":
                calculate = new Addition();
                break;
            case "-":
                calculate = new Subtraction();
                break;
        }
        return calculate;
    }

    public void Main1(int num1, int num2, string operation)
    {
        Calculate calculate = GetCalculate(operation);
        calculate.Num1 = num1;
        calculate.Num2 = num2;
        var result = calculate.Compute();
    }
}
