using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Loom : MonoBehaviour
{
    private static Loom _instance;

    public static Loom Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameObject = new GameObject("[Loom]");
                _instance = gameObject.AddComponent<Loom>();
                DontDestroyOnLoad(gameObject);
            }
            return _instance;
        }
    }
    public struct NoDelayedQueueItem
    {
        public Action<object> action;
        public object param;
    }
    public struct DelayedQueueItem
    {
        public float time;
        public Action<object> action;
        public object param;
    }

    private static readonly int maxThreads = 8;
    private static int numThreads;

    private readonly List<NoDelayedQueueItem> _actions = new List<NoDelayedQueueItem>();
    private readonly List<NoDelayedQueueItem> _currentActions = new List<NoDelayedQueueItem>();

    private readonly List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
    private readonly List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();


    private void Update()
    {
        if (_actions.Count > 0)
        {
            lock (_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            for (int i = 0; i < _currentActions.Count; i++)
            {
                _currentActions[i].action(_currentActions[i].param);
            }
        }

        if (_delayed.Count > 0)
        {
            lock (_delayed)
            {
                _currentDelayed.Clear();

                var temp = _delayed.FindAll(d => d.time <= Time.time);
                if (temp != null)
                {
                    _currentDelayed.AddRange(temp);
                }
                for (int i = 0; i < _currentDelayed.Count; i++)
                {
                    _delayed.Remove(_currentDelayed[i]);
                }
            }

            for (int i = 0; i < _currentDelayed.Count; i++)
            {
                _currentDelayed[i].action(_currentDelayed[i].param);
            }
        }
    }

    public void QueueOnMainThread(Action<object> taction, object tparam)
    {
        QueueOnMainThread(taction, tparam, 0f);
    }

    public void QueueOnMainThread(Action<object> taction, object tparam, float time)
    {
        if (time != 0)
        {
            lock (Instance._delayed)
            {
                Instance._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = taction, param = tparam });
            }
        }
        else
        {
            lock (Instance._actions)
            {
                Instance._actions.Add(new NoDelayedQueueItem { action = taction, param = tparam });
            }
        }
    }

    public Thread RunAsync(Action action)
    {
        while (numThreads >= maxThreads)
        {
            Thread.Sleep(100);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, action);

        //ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(RunAction);
        //Thread thread = new Thread(parameterizedThreadStart);
        //thread.Name = "Thread 1";
        //thread.Start(action);

        return null;
    }

    private void RunAction(object action)
    {
        try
        {
            Action temp = (Action)action;
            temp?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }
    }
}