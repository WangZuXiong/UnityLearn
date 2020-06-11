using UnityEngine;

public class Move : MonoBehaviour
{
    public float Speed = 1;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
    }
}
