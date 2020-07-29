using UnityEngine;

public class MonoObject : MonoBehaviour
{

    private static MonoObject _insatnce;

    public static MonoObject Instance
    {
        get
        {
            if (_insatnce == null)
            {
                GameObject gameObject = new GameObject("[MonoObject]");
                _insatnce = gameObject.AddComponent<MonoObject>();
                DontDestroyOnLoad(gameObject);
            }

            return _insatnce;
        }
    }


    private MonoObject() { }
}
