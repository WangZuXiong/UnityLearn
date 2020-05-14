using UnityEngine;

public static class GameObjectUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    public static void Hide(this GameObject gameObject)
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    public static void Show(this GameObject gameObject)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }
}
