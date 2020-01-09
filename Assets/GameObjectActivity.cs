using UnityEngine;

public class GameObjectActivity : MonoBehaviour
{
    private void  OnDisable()
    {
        gameObject.SetActive(false);
    }
}
