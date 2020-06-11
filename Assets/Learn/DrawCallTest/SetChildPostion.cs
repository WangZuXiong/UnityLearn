using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SetChildPostion : MonoBehaviour
{
    [SerializeField]
    [Range(-110, 110)]
    private float _offset;

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            float x, y;
            x = y = i * _offset;
            transform.GetChild(i).localPosition = new Vector3(x, y);
            transform.GetChild(i).name = i.ToString() + "_" + transform.GetChild(i).GetComponent<Image>().sprite.name;
        }
    }
}
