using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Object
{
    private Stack<T> _stack;
    private T _original;
    private Transform _parent;
    private int _index = 0;


    private Dictionary<int, GameObject> _gameObjectDict = new Dictionary<int, GameObject>();

    private T Create()
    {
        var temp = GameObject.Instantiate(_original, _parent);
        temp.name = string.Format("{0}_{1}", _original.name, _index++.ToString());
        return temp;
    }


    public GameObjectPool(T original, int capacity)
    {
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
        for (int i = 0; i < _stack.Count; i++)
        {
            //GameObject go = _stack.Pop() as GameObject;
            GameObject.Destroy(_stack.Pop());
        }
        GameObject.Destroy(_parent.gameObject);
        _stack.Clear();
        _original = null;
    }

    public void ReleaseGameObject(T t)
    {
        GameObject go = t as GameObject;
        go.transform.SetParent(_parent);
        _stack.Push(t);
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