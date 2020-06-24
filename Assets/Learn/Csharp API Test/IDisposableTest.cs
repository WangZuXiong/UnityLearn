using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// https://www.cnblogs.com/YNLDY/articles/2272151.html
/// </summary>
public class IDisposableTest : MonoBehaviour
{
    private void Main()
    {
        //DisposableTestFunc();

        Create();
        Debug.Log("After created");
        CallGC();

        //如上运行的结果如下：
        //After created!
        //Destructor called!
        //结论：
        //显然在出了Create函数外，myClass对象的析构函数没有被立刻调用，而是等显示调用GC.Collect才被调用。


        //对于Dispose来说，也需要显示的调用，但是对于继承了IDisposable的类型对象可以使用using这个关键字，这样对象的Dispose方法在出了using范围后会被自动调用。例如：
        using (DisposeClass t = new DisposeClass())
        {
            
        }
        //如上运行的结果如下：
        ///Dispose called
    }

    public void DisposableTestFunc()
    {
        DisposableTest disposableTest = new DisposableTest();
        //do something
        disposableTest.Dispose();


        using (DisposableTest disposableTest1 = new DisposableTest())
        {
            //do something
        }


        //FileStream : Stream : IDisposable
    }


    public class DisposableTest : IDisposable
    {
        public void Dispose()
        {
            Debug.Log("Do Dispose");
        }
    }


    public class ResourceHolder : IDisposable
    {
        //_isDispose成员变量表示对象是否已被删除，并允许确保不多次删除成员变量
        private bool _isDisposed = false;

        //显示调用Dispose的方法
        public void Dispose()
        {
            Dispose(true);
            //SuppressFinalize()方法则告诉垃圾收集器有一个类不再需要调用其析构函数了
            //因为Dispose()已经完成了所有需要的清理工作，所以析构函数不需要做任何工作。调用SuppressFinalize()就意味着垃圾收集器认为这个对象根本没有析构函数．
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {
                    //这里执行清除托管对象的操作
                }
                //这里执行清除非托管对象的操作

                _isDisposed = true;
            }
        }
        //析构函数
        ~ResourceHolder()
        {
            Dispose(false);
        }
    }


    public class DisposeClass : IDisposable
    {
        public void Close()
        {
            Debug.Log("close called");
        }

        ~DisposeClass()
        {
            Debug.Log("destructor called");
        }

        public void Dispose()
        {
            Debug.Log("dispoed called");
        }
    }

    private void Create()
    {
        DisposeClass myClass = new DisposeClass();
    }

    private void CallGC()
    {
        GC.Collect();
    }
}
