using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CreatThread : MonoBehaviour
{
    Thread _thread0;
    Thread _thread1;

    ThreadStart _threadStart;
    ParameterizedThreadStart _parameterizedThreadStart;
    void Start()
    {
        _threadStart = new ThreadStart(Method);
        _thread0 = new Thread(_threadStart);
        _thread0.Name = "Thread 0";
        //_thread0.Name = "My Thread 0";//InvalidOperationException: Thread.Name can only be set once.
        _thread0.Priority = System.Threading.ThreadPriority.Highest;
        _thread0.Start();


        Thread.Sleep(1000);
        _parameterizedThreadStart = new ParameterizedThreadStart(ParameterizedMethod);
        _thread1 = new Thread(_parameterizedThreadStart);
        _thread1.Name = "Thread 1";
        _thread1.Start("parameter");
    }

    void Method()
    {
        Debug.Log("Thread 0 Name:" + _thread0.Name);
        Debug.Log("Thread 0 Priority:" + _thread0.Priority.ToString());
        Debug.Log("Thread 0 ThreadState:" + _thread0.ThreadState.ToString());



        //GameObject gameObject = new GameObject();//UnityException: Internal_CreateGameObject can only be called from the main thread.
    }

    void ParameterizedMethod(object obj)
    {
        Debug.Log("Thread 1 Name:" + _thread1.Name);
        Debug.Log("Thread 1 parameter:" + obj.ToString());
    }


    private void OnDestroy()
    {
        //终止线程。
        _thread0.Abort();
        _thread1.Abort();
    }
}
