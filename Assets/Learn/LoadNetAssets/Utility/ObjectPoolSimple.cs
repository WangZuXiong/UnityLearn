using System;
using System.Collections.Generic;

public class ObjectPoolSimple<T> where T : class, new()
{
    private readonly Stack<T> m_objectStack;
    private readonly Action<T> m_resetAction;
    private readonly Action<T> m_onetimeInitAction;

    public ObjectPoolSimple(int initialBufferSize, Action<T> ResetAction = null, Action<T> OnetimeInitAction = null)
    {
        m_objectStack = new Stack<T>(initialBufferSize);
        m_resetAction = ResetAction;
        m_onetimeInitAction = OnetimeInitAction;
    }

    public T Get()
    {
        if (m_objectStack.Count > 0)
        {
            T t = m_objectStack.Pop();
            m_resetAction?.Invoke(t);
            return t;
        }
        else
        {
            T t = new T();
            m_onetimeInitAction?.Invoke(t);
            return t;
        }
    }

    public void Release(T obj)
    {
        m_objectStack.Push(obj);
    }

    public void Clear()
    {
        m_objectStack.Clear();
    }
}