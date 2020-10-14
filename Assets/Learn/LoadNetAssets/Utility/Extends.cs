using UnityEngine;


public static class Extends
{
    public static T AttachUniqueComponent<T>(GameObject go) where T : Component
    {
        if (null == go) return null;
        T t = go.GetComponent<T>();
        if (null == t)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }
}
