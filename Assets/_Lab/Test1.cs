using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Test1 : MonoBehaviour
{
    public Transform t1;
    public Transform t2;


    private void Start()
    {
        var p1 = Camera.main.WorldToScreenPoint(t1.position);
        var p2 = Camera.main.WorldToScreenPoint(t2.position);

        Debug.Log(p1);
        Debug.Log(p2);

    }

    private void Update()
    {

    }
}
