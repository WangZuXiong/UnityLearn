using System;
using System.Collections.Generic;

public static class EventDispatcher 
{
    private static readonly Dictionary<string, Delegate> _delegateDict = new Dictionary<string, Delegate>();

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="actionName">事件名称</param>
    /// <param name="action">事件</param>
    public static void AddListener(string actionName, Action action)
    {
        if (action == null)
        {
            throw new Exception("事件为空：" + actionName);
        }

        if (_delegateDict.ContainsKey(actionName))
        {
            Delegate temp = _delegateDict[actionName];

            if (!temp.GetType().Equals(action.GetType()))
            {
                throw new Exception("参数类型不对：" + temp.GetType() + "_" + action.GetType());
            }

            Delegate delegates = Delegate.Combine(temp, action);

            _delegateDict[actionName] = delegates;
        }
        else
        {
            _delegateDict.Add(actionName, action);
        }
    }

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="actionName"></param>
    /// <param name="action"></param>
    public static void AddListener<T>(string actionName, Action<T> action)
    {
        if (action == null)
        {
            throw new Exception("事件为空：" + actionName);
        }

        if (_delegateDict.ContainsKey(actionName))
        {
            Delegate temp = _delegateDict[actionName];

            if (!temp.GetType().Equals(action.GetType()))
            {
                throw new Exception("参数类型不对：" + temp.GetType() + "_" + action.GetType());
            }

            Delegate delegates = Delegate.Combine(temp, action);
            _delegateDict[actionName] = delegates;
        }
        else
        {
            _delegateDict.Add(actionName, action);
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="actionName">事件名称</param>
    public static void TriggerListener(string actionName)
    {
        if (!_delegateDict.ContainsKey(actionName))
        {
            throw new Exception("不存在该事件：" + actionName);
        }

        Delegate[] delegates = _delegateDict[actionName].GetInvocationList();

        for (int i = 0; i < delegates.Length; i++)
        {
            Action action = delegates[i] as Action;
            action();
        }
    }

    public static void TriggerListener<T>(string actionName, T t)
    {
        if (!_delegateDict.ContainsKey(actionName))
        {
            throw new Exception("不存在该事件：" + actionName);
        }

        Delegate[] delegates = _delegateDict[actionName].GetInvocationList();

        for (int i = 0; i < delegates.Length; i++)
        {
            if (!(delegates[i] is Action<T> action))
            {
                throw new Exception("参数类型不对应：" + actionName + "_" + delegates[i].GetType() + "_" + t.GetType());
            }
            action(t);
        }
    }

    /// <summary>
    /// 移除actionName对应的所有事件
    /// </summary>
    /// <param name="actionName"></param>
    public static void RemoveListtener(string actionName)
    {
        if (!_delegateDict.ContainsKey(actionName))
        {
            throw new Exception("不存在该事件：" + actionName);
        }
        Delegate[] delegates = _delegateDict[actionName].GetInvocationList();

        for (int i = 0; i < delegates.Length; i++)
        {
            delegates[i] = null;
        }

        Delegate.RemoveAll(_delegateDict[actionName], _delegateDict[actionName]);
        _delegateDict.Remove(actionName);
    }

    /// <summary>
    /// 移除actionName对应的所有事件中的某一个
    /// </summary>
    /// <param name="actionName"></param>
    /// <param name="action"></param>
    public static void RemoveListtener(string actionName, Action action)
    {
        if (!_delegateDict.ContainsKey(actionName))
        {
            throw new Exception("不存在该事件：" + actionName);
        }
        _delegateDict[actionName] = Delegate.Remove(_delegateDict[actionName], action);

        if (_delegateDict[actionName] == null)
        {
            _delegateDict.Remove(actionName);
        }
    }

    public static void RemoveListtener<T>(string actionName, Action<T> action)
    {
        if (!_delegateDict.ContainsKey(actionName))
        {
            throw new Exception("不存在该事件：" + actionName);
        }
        if (!_delegateDict[actionName].GetType().Equals(action.GetType()))
        {
            throw new Exception("参数类型不对应：" + actionName + "_" + _delegateDict[actionName].GetType() + "_" + action.GetType());
        }

        _delegateDict[actionName] = (Action<T>)Delegate.Remove(_delegateDict[actionName], action);

        if (_delegateDict[actionName] == null)
        {
            _delegateDict.Remove(actionName);
        }
    }

    /// <summary>
    /// 清理
    /// </summary>
    public static void Clear()
    {
        var enumerator = _delegateDict.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Delegate temp = enumerator.Current.Value;
            temp = Delegate.RemoveAll(temp, temp);
            temp = null;
        }
        _delegateDict.Clear();
    }
}