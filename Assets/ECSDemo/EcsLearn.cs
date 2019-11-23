using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常规理解的OOP编程
/// </summary>
public class EcsLearn1
{
    public class Dog
    {
        public void WagTail()
        {
            Debug.Log("dog wag tail");
        }
    }

    public class Pig
    {
        public void WagTail()
        {
            Debug.Log("pig wag tail");
        }
    }

    public void Main(Dog[] dogs, Pig[] pigs)
    {
        for (int i = 0; i < dogs.Length; i++)
        {
            dogs[i].WagTail();
        }

        for (int i = 0; i < pigs.Length; i++)
        {
            pigs[i].WagTail();
        }
    }
}

/// <summary>
/// 优化之后的OOP
/// </summary>
public class EcsLearn2
{
    public interface IWagTail
    {
        void WagTail();
    }

    public class Dog : IWagTail
    {
        public void WagTail()
        {
            Debug.Log("dog wag tail");
        }
    }

    public class Pig : IWagTail
    {
        public void WagTail()
        {
            Debug.Log("pig wag tail");
        }
    }

    public void Main(IWagTail[] dogsAndPigs)
    {
        for (int i = 0; i < dogsAndPigs.Length; i++)
        {
            dogsAndPigs[i].WagTail();
        }
    }
}

/// <summary>
/// OOD编程
/// </summary>
public class EcsLearn3
{
    public void Main()
    {
        List<Entity> entities = CreateEntity();

        List<Tail> tails = new List<Tail>();

        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i].HaveCompontent<Tail>())
            {
                tails.Add(entities[i].GetCompontent<Tail>());
            }
        }

        WagTailSystem system = new WagTailSystem();
        system.WagTail(tails);
    }

    /// <summary>
    /// E
    /// </summary>
    public class Entity
    {
        public void AddCompontent<T>() where T : ICompontent
        {
            //to do
            //实现entity绑定组件
        }

        public bool HaveCompontent<T>() where T : ICompontent
        {
            //to do
            //返回entity是否含有某个组件
            return true;
        }

        public T GetCompontent<T>() where T : ICompontent
        {
            T t = default(T);
            //to do
            //返回entity的某个组件
            return t;
        }
    }

    /// <summary>
    /// C
    /// </summary>
    public class ICompontent
    {
        //就是一个空的基类，这里为什么要使用class，因为C#语言特性，struct不能继承。interface只关注方法，而我们需要的Component其实是一个数据的集合，所以这里作为演示代码就不写那么复杂的设计，理解概念就好。
    }

    /// <summary>
    /// S
    /// </summary>
    public class WagTailSystem
    {
        public void WagTail(List<Tail> tails)
        {
            Debug.Log("wag tail");
        }
    }

    public class Dog : ICompontent
    {

    }

    public class Pig : ICompontent
    {

    }

    public class Tail : ICompontent
    {

    }

    public List<Entity> CreateEntity()
    {
        List<Entity> entities = new List<Entity>();
        Entity entity;
        for (int i = 0; i < 100; i++)
        {
            entity = new Entity();
            //添加tail组件
            entity.AddCompontent<Tail>();
            if (i < 50)
            {
                //添加dog组件
                entity.AddCompontent<Dog>();
            }
            else
            {
                entity.AddCompontent<Pig>();
            }
            entities.Add(entity);
        }
        return entities;
    }
}

