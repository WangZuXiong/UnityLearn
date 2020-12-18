using UnityEngine;

public class SimpleCoroutineManager : MonoBehaviour
{

    private static SimpleCoroutineManager _insatnce;

    public static SimpleCoroutineManager Instance
    {
        get
        {
            if (_insatnce == null)
            {
                GameObject gameObject = new GameObject("[MonoObject]");
                _insatnce = gameObject.AddComponent<SimpleCoroutineManager>();
                DontDestroyOnLoad(gameObject);
            }

            return _insatnce;
        }
    }


    private SimpleCoroutineManager() { }
}
