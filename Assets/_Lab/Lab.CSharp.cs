using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

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

    private void As_Test()
    {

        Animal dog = new Dog();

        Debug.Log(dog is Animal);//True

        Dog dog1 = new Dog();

        Animal animal = dog1 as Animal;//多余的转换
        Debug.Log(animal == null);//False

        Animal animal1 = dog1 as Animal;//多余的转换
        Debug.Log(animal == null);//False

        Dog dog2 = animal as Dog;//向下转型
        Debug.Log(dog2 == null);//False
    }

    void GetHashCodeTest()
    {
        var str1 = "wzx";
        var str2 = "www";

        Debug.Log("wzx hash code:" + str1.GetHashCode());
        Debug.Log("www hash code:" + str2.GetHashCode());


        StudentClass student1 = new StudentClass();
        student1.Name = "1";

        StudentClass student2 = new StudentClass();
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

    void StackTest()
    {
        //栈是先进后出
        Stack<string> strStack = new Stack<string>();
        //元素入栈用push,就是向栈中添加元素
        strStack.Push("A");
        strStack.Push("B");
        strStack.Push("C");

        //获取stack 中的元素个数
        int num = strStack.Count;
        //Pop出栈，返回栈顶的元素 并且删除
        string str1 = strStack.Pop();
        //Peek返回栈顶的元素 不删除
        string str2 = strStack.Peek();

        strStack.Clear();
    }

    void QueueTest()
    {
        //队列是先进先出
        Queue<string> strQueue = new Queue<string>();
        strQueue.Enqueue("A");
        strQueue.Enqueue("B");
        strQueue.Enqueue("C");

        int count = strQueue.Count;
        //返回队列里面第一个元素  并且删除这个元素
        string str1 = strQueue.Dequeue();
        //返回队列里面第一个元素
        string str2 = strQueue.Peek();
    }

    /// <summary>
    /// 朗母达表达式排序
    /// </summary>
    private void LongmudaSort()
    {
        //var list = new List<Student>
        //    {
        //        new Student (10,160),
        //        new Student (20,170),
        //        new Student (20,150)
        //    };

        //for (int i = 0; i < list.Count; i++)
        //{
        //    Debug.Log(list[i].ToString());
        //}

        //Debug.LogError("==============");

        //按照年龄排序
        //如果年龄一致的话则按照身高排序

        //list.Sort((item1, item2) =>
        //{
        //    if (item1.Age == item2.Age)
        //    {
        //        return item1.Height.CompareTo(item2.Height);
        //    }
        //    return item1.Age.CompareTo(item2.Age);
        //});

        ////权重大的排前面
        //list.Sort((item1, item2) => item1.Age.CompareTo(item2.Age) * 2 + item1.Height.CompareTo(item2.Height));


        ////list.AddRange(new List<Student>());
        //for (int i = 0; i < list.Count; i++)
        //{
        //    Debug.Log(list[i].ToString());
        //}



        List<Tuple<int, int>> tmp = new List<Tuple<int, int>>()
        {
            new Tuple<int,int>(2,1),
            new Tuple<int,int>(53,1),
            new Tuple<int,int>(12,1),
            new Tuple<int,int>(22,3),
            new Tuple<int,int>(1,2),
        };

        //先按照Item2升序排列 Item2相同的情况下在按照Item1降序排列
        //tmp.Sort((x, y) =>
        //(x.Item1.CompareTo(y.Item1) * -1
        //+ x.Item2.CompareTo(y.Item2) * 2));

        //for (int i = 0; i < tmp.Count; i++)
        //{
        //    Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        //}
        //53,1
        //12,1
        //2,1
        //1,2
        //22,3


        //先按照Item2降序排列 Item2相同的情况下在按照Item1降序排列
        // tmp.Sort((x, y) =>
        //-(x.Item1.CompareTo(y.Item1)
        //+ x.Item2.CompareTo(y.Item2) * 2));
        // for (int i = 0; i < tmp.Count; i++)
        // {
        //     Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        // }
        //22,3
        //1,2
        //53,1
        //12,1
        //2,1


        //先按照Item2升序排列 Item2相同的情况下在按照Item1升序排列
        // tmp.Sort((x, y) =>
        //(x.Item1.CompareTo(y.Item1) * 1
        //+ x.Item2.CompareTo(y.Item2) * 2));
        // for (int i = 0; i < tmp.Count; i++)
        // {
        //     Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        // }
        //2,1
        //12,1
        //53,1
        //1,2
        //22,3



        //先按照Item2升序排列 Item2相同的情况下在按照Item1降序排列
        // tmp.Sort((x, y) =>
        //(y.Item1.CompareTo(x.Item1) * 1
        //+ x.Item2.CompareTo(y.Item2) * 2));
        // for (int i = 0; i < tmp.Count; i++)
        // {
        //     Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        // }
        //53,1
        //12,1
        //2,1
        //1,2
        //22,3

        //先按照Item2降序排列 Item2相同的情况下在按照Item1降序排列
        tmp.Sort((x, y) =>
       (y.Item1.CompareTo(x.Item1) * 1
       + y.Item2.CompareTo(x.Item2) * 2));
        for (int i = 0; i < tmp.Count; i++)
        {
            Debug.LogError(tmp[i].Item1 + "," + tmp[i].Item2);
        }
        //22,3
        //1,2
        //53,1
        //12,1
        //2,1
    }

    /// <summary>
    /// 向右操作运算符
    /// 向左操作运算符
    /// </summary>
    private void OperatorTest()
    {
        int m = 8;
        Debug.LogError(m >> 1);//十进制转化为二进制 8 = 1000 ，1向右边移动1位，0100，即4
        Debug.LogError(m >> 2);//十进制转化为二进制 8 = 1000 ，1向右边移动2位，0010，即2
        Debug.LogError(m >> 3);//十进制转化为二进制 8 = 1000 ，1向右边移动3位，0001，即1

        int n = 2;
        Debug.LogError(n << 1);//十进制转化为二进制 2 = 10 ，1向左边移动1位，100，即4
        Debug.LogError(n << 2);//十进制转化为二进制 2 = 10 ，1向左边移动2位，1000，即8
        Debug.LogError(n << 3);//十进制转化为二进制 2 = 10 ，1向左边移动3位，10000，即16
    }

    /// <summary>
    /// 反射程序集，调用其中的静态函数
    /// </summary>
    public void LoadAssemblyInvokeStaticMethod()
    {
        string assemblyPath = @"E:\wangzuxiong\Unity Project\UnityLearn\Library\ScriptAssemblies\Assembly-CSharp.dll";
        string className = "Lab";
        string methodName = "TestStaticMethod";
        Assembly assembly = Assembly.LoadFile(assemblyPath);
        Type type = assembly.GetType(className);
        MethodInfo methodInfo = type.GetMethod(methodName);
        methodInfo.Invoke(null, new object[] { 1 });
    }

    public static void TestStaticMethod(int x)
    {
        Debug.Log(x);
    }

    /// <summary>
    /// 反射程序集，调用其中的非静态函数
    /// </summary>
    public void LoadAssemblyInvokeMethod()
    {
        string assemblyPath = @"E:\wangzuxiong\Unity Project\UnityLearn\Library\ScriptAssemblies\Assembly-CSharp.dll";
        string className = "Lab";
        string methodName = "TestStaticMethod";
        Assembly assembly = Assembly.LoadFile(assemblyPath);
        Type type = assembly.GetType(className);
        var instance = Activator.CreateInstance(type);
        MethodInfo methodInfo = type.GetMethod(methodName);
        methodInfo.Invoke(instance, new object[] { 1 });
    }


    public void MyInvoke(object obj, string methodName, params object[] vs)
    {
        obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase).Invoke(obj, vs);
    }

    public void TestMethod(int x)
    {
        Debug.Log(x);
    }


    public void GetCustomType()
    {
        Type type = Type.GetType("");
        transform.GetComponent(type);
        gameObject.AddComponent(type);

        transform.GetComponent(typeof(Button));
    }


    public struct StudentStruct
    {
        public int Age;
        public int Height;
        public string Name;
    }


    public class StudentClass
    {
        public int Age;
        public int Height;
        public string Name;


        //public override bool Equals(object obj)
        //{
        //    return Name == ((Student)obj).Name;
        //}

        //public override int GetHashCode()
        //{
        //    return Name.GetHashCode();
        //}

        //public Student(int age, int height)
        //{
        //    Age = age;
        //    Height = height;
        //}

        //public override string ToString()
        //{
        //    return ("Age:" + Age + " Height:" + Height);
        //}

        public void Say(string str)
        {
            Debug.Log("==>" + str);
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


    class Animal
    {
        public void call() { Debug.LogError("无声的叫唤"); }
    }

    class Dog : Animal
    {
        // new的作用是隐藏父类的同名方法
        public new void call() { Debug.LogError("叫声：汪～汪～汪～"); }
        public void smell() { Debug.LogError("嗅觉相当不错！"); }
    }


}



