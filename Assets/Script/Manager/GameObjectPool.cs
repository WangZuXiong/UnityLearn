using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Component
{
    private Stack<T> _stack;
    private GameObject _original;
    private Transform _parent;
    private int _index = 0;


    private Dictionary<int, GameObject> _gameObjectDict = new Dictionary<int, GameObject>();

    private T Create()
    {
        var gameObject = GameObject.Instantiate(_original, _parent);
        gameObject.name = string.Format("{0}_{1}", _original.name, _index++.ToString());
        T t = gameObject.transform.GetComponent<T>();
        if (t == null)
        {
            t = gameObject.AddComponent<T>();
        }
        _gameObjectDict.Add(t.GetInstanceID(), gameObject);
        return t;
    }


    public GameObjectPool(GameObject original, int capacity)
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
        foreach (var item in _gameObjectDict)
        {
            GameObject.Destroy(item.Value);
        }
        GameObject.Destroy(_parent.gameObject);
        _gameObjectDict.Clear();
        _stack.Clear();
        _original = null;
    }

    public void ReleaseGameObject(T t)
    {
        if (_gameObjectDict.TryGetValue(t.GetInstanceID(), out GameObject go))
        {
            go.transform.position = Vector3.zero;
            go.transform.SetParent(_parent);
            _stack.Push(t);
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