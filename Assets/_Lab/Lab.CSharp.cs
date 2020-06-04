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
