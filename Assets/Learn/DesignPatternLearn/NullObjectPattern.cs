using System.Linq;
using UnityEngine;
/// <summary>
/// 空对象模式
/// </summary>
public class NullObjectPattern
{
    public abstract class AbstractCustomer
    {
        protected string Name;
        public abstract bool IsNil();

        public abstract string GetName();
    }

    public class RealCustomer : AbstractCustomer
    {
        public RealCustomer(string name)
        {
            Name = name;
        }

        public override string GetName()
        {
            return Name;
        }

        public override bool IsNil()
        {
            return false;
        }
    }

    public class NullCustomer : AbstractCustomer
    {
        public override string GetName()
        {
            return "Not Available in Customer Database";
        }

        public override bool IsNil()
        {
            return true;
        }
    }

    public class CustomerFactory
    {
        public static string[] Names = { "Rob", "Joe", "Julie" };

        public static AbstractCustomer GetCustomer(string name)
        {
            if (Names.Contains(name))
            {
                return new RealCustomer(name);
            }
            return new NullCustomer();
        }
    }

    public void Main()
    {
        AbstractCustomer customer1 = CustomerFactory.GetCustomer("Rob");
        AbstractCustomer customer2 = CustomerFactory.GetCustomer("Joe");
        AbstractCustomer customer3 = CustomerFactory.GetCustomer("Julie");
        AbstractCustomer customer4 = CustomerFactory.GetCustomer("Laura");

        Debug.Log(customer1.GetName());
        Debug.Log(customer2.GetName());
        Debug.Log(customer3.GetName());
        Debug.Log(customer4.GetName());
    }
}
