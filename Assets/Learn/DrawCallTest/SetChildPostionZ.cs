using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SetChildPostionZ : MonoBehaviour
{
    [SerializeField]
    [Range(-10, 10)]
    private float _offset;

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            float z = i * _offset;
            transform.GetChild(i).localPosition = new Vector3(transform.GetChild(i).localPosition.x, transform.GetChild(i).localPosition.y, z);
        }
    }
}
