using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// 单例模式
/// </summary>
public class SingletonPattern : MonoBehaviour
{
    public class SingleObject
    {
        //创建 SingleObject 的一个对象
        private static SingleObject _instance = new SingleObject();
        //让构造函数为 private，这样该类就不会被实例化
        private SingleObject()
        {

        }
        //获取唯一可用的对象
        public SingleObject GetInstance()
        {
            return _instance;
        }

        public void ShowMessage()
        {
            Debug.Log("Hello World!");
        }
    }

    /// <summary>
    /// 1、懒汉式，线程不安全
    /// 这种方式是最基本的实现方式，这种实现最大的问题就是不支持多线程。因为没有加锁 synchronized，所以严格意义上它并不算单例模式。这种方式 lazy loading 很明显，不要求线程安全，在多线程不能正常工作。
    /// </summary>
    public class Singleton
    {
        private static Singleton _instance;
        private Singleton() { }

        public static Singleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Singleton();
            }
            return _instance;
        }
    }
    /// <summary>
    /// 懒汉式，线程安全
    /// 这种方式具备很好的 lazy loading，能够在多线程中很好的工作，但是，效率很低，99% 情况下不需要同步。
    /// 优点：第一次调用才初始化，避免内存浪费。
    /// 缺点：必须加锁 Synchronized 才能保证单例，但加锁会影响效率。
    /// GetSingleton() 的性能对应用程序不是很关键（该方法使用不太频繁）。
    /// </summary>
    public class Singleton2
    {
        private static Singleton2 _instance;
        private Singleton2() { }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Singleton2 GetSingleton()
        {
            if (_instance == null)
            {
                _instance = new Singleton2();
            }
            return _instance;
        }
    }
    /// <summary>
    /// 饿汉式
    /// 这种方式比较常用，但容易产生垃圾对象。
    /// 优点：没有加锁，执行效率会提高。
    /// 缺点：类加载时就初始化，浪费内存。
    /// 它基于 classloader 机制避免了多线程的同步问题，不过，instance 在类装载时就实例化，
    /// 虽然导致类装载的原因有很多种，在单例模式中大多数都是调用 getInstance 方法， 
    /// 但是也不能确定有其他的方式（或者其他的静态方法）导致类装载，这时候初始化 instance 显然没有达到 lazy loading 的效果。
    /// </summary>
    public class Singleton3
    {
        public static Singleton3 _instance = new Singleton3();
        private Singleton3() { }

        public static Singleton3 GetInstance()
        {
            return _instance;
        }
    }



    //双检锁/双重校验锁（DCL，即 double-checked locking）
    //....


    //登记式/静态内部类
    //....
}
