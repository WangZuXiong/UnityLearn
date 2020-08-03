using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Component
{
    private Stack<T> _stack;
    private T _original;
    private Transform _parent;
    private int _index = 0;
    private List<T> _compontents = new List<T>();

    private T Create()
    {
        T t = GameObject.Instantiate(_original, _parent);
        t.gameObject.name = string.Format("{0}_{1}", _original.name, _index++.ToString());
        _compontents.Add(t);
        return t;
    }


    public GameObjectPool(T original, int capacity)
    {
        if (original == null)
        {
            throw new System.Exception(typeof(T).ToString() + " == null ");
        }
        _stack = new Stack<T>();
        _original = original;
        _parent = new GameObject(string.Format("[{0} Pool]", _original.name)).transform;

        for (int i = 0; i < capacity; i++)
        {
            _stack.Push(Create());
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _compontents.Count; i++)
        {
            GameObject.Destroy(_compontents[i].gameObject);
        }
        GameObject.Destroy(_parent.gameObject);
        _compontents.Clear();
        _stack.Clear();
        _original = null;
    }

    public void Release(T t)
    {
        t.transform.position = Vector3.zero;
        t.transform.SetParent(_parent);
        _stack.Push(t);

    }

    public void ReleaseAll()
    {
        for (int i = 0; i < _compontents.Count; i++)
        {
            Release(_compontents[i]);
        }
    }

    public T GetGameObject()
    {
        if (_stack.Count <= 0)
        {
            _stack.Push(Create());
        }

        return _stack.Pop();
    }
}