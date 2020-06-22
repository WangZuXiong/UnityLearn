using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Lab : MonoBehaviour
{
    void ipp_ppi_Test()
    {
        //i++ 先传值，再自增
        //++i 先自增，再传值
        //在这里的例子中：都是自增，没有用到传值  所以结果是一样的
        List<int> vs = new List<int>
        {
            1,
            2,
            3
        };

        for (int i = 0; i < vs.Count; i++)
        {
            Debug.LogError(vs[i]);//1 2 3
        }

        for (int i = 0; i < vs.Count; ++i)
        {
            Debug.LogError(vs[i]);//1 2 3
        }

        Debug.LogError("========================");
        for (int i = 0; i < vs.Count;)
        {
            Debug.LogError(vs[i]);//1 2 3
            i++;
        }

        int index1 = 0;
        if (index1 < vs.Count)
        {
            Debug.LogError(vs[index1]);
            index1++;//index = 1
        }
        if (index1 < vs.Count)
        {
            Debug.LogError(vs[index1]);
            index1++;//index = 2
        }
        if (index1 < vs.Count)
        {
            Debug.LogError(vs[index1]);
            index1++;//index = 3
        }





        for (int i = 0; i < vs.Count;)
        {
            Debug.LogError(vs[i]);//1 2 3
            ++i;
        }

        int index2 = 0;
        if (index2 < vs.Count)
        {
            Debug.LogError(vs[index2]);
            ++index2;//index = 1
        }
        if (index2 < vs.Count)
        {
            Debug.LogError(vs[index2]);
            ++index2;//index = 2
        }
        if (index2 < vs.Count)
        {
            Debug.LogError(vs[index2]);
            ++index2;//index = 3
        }
    }

    void StringSort()
    {

        List<string> vs = new List<string>();
        vs.Add("A/B/C");
        vs.Add("A");
        vs.Add("A/B");

        vs.Add("F/B");

        vs.Sort();

        Debug.LogError(string.Join(",", vs));
    }


    void GetHashCodeTest()
    {
        var str1 = "wzx";
        var str2 = "www";

        Debug.Log("wzx hash code:" + str1.GetHashCode());
        Debug.Log("www hash code:" + str2.GetHashCode());


        Student student1 = new Student();
        student1.Name = "1";

        Student student2 = new Student();
        student2.Name = "1";

        Debug.Log("student1== student2   " + (student1 == student2));
        Debug.Log("student1.Equals(student2)   " + (student1.Equals(student2)));
        Debug.Log("student1.GetHashCode()==student2.GetHashCode()  " + (student1.GetHashCode() == student2.GetHashCode()));
    }


    void ReturnTest()
    {
        Debug.Log("1");
        Return();
        Debug.Log("2");
        Debug.Log("3");
    }

    void Return()
    {
        return;
    }

    public class Student
    {
        public string Name;


        public override bool Equals(object obj)
        {
            return Name == ((Student)obj).Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }


    /// <summary>
    /// 字典比较的方案
    /// </summary>
    public void DictComparerTest()
    {
        //用枚举用作Dict的Key时，会有装箱的的操作
        Dictionary<TestEnum, int> keyValuePairs = new Dictionary<TestEnum, int>();
        

        //比较合理的方式 不造成装箱的操作
        Dictionary<TestEnum, int> keyValuePairs2 = new Dictionary<TestEnum, int>(new DictEqualityComparer());
    }

    /// <summary>
    /// Key比较器
    /// </summary>
    public class DictEqualityComparer : IEqualityComparer<TestEnum>
    {
        public bool Equals(TestEnum x, TestEnum y)
        {
            return x == y;
        }

        public int GetHashCode(TestEnum obj)
        {
            return (int)obj;
        }
    }

    public enum TestEnum
    {
        X = 1,
        Y = 2,
        Z = 3
    }
}


public abstract class TestA : MonoBehaviour
{
    /// <summary>
    /// 抽象
    /// </summary>
    public abstract void Func1();

    internal abstract void Func3();

    protected abstract void Func2();


    public virtual void Func4()
    {

    }

}
public class TestB : TestA
{
    public override void Func1()
    {
        throw new System.NotImplementedException();
    }

    protected override void Func2()
    {
        throw new System.NotImplementedException();
    }

    internal override void Func3()
    {
        throw new System.NotImplementedException();
    }

    public override void Func4()
    {
        base.Func4();
    }
}
