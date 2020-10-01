using System.Collections;
using UnityEngine;

public class MonoObject : MonoBehaviour
{
    private static MonoObject _instance;

    public static MonoObject Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject t = new GameObject("[MonoObject]", typeof(MonoObject));
                _instance = t.GetComponent<MonoObject>();
                DontDestroyOnLoad(t);
            }
            return _instance;
        }
    }

    private MonoObject() { }
}
