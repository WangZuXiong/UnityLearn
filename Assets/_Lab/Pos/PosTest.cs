using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosTest : MonoBehaviour
{
    public ConnectingLine lines;

    public Transform target1;
    public Transform target2;

    // Start is called before the first frame update
    void Start()
    {


        Debug.Log(target1.position);
        Debug.Log(target2.position);

        //先把布局组件disable
        //先把布局组件disable
        //先把布局组件disable


        //GetComponentsInChildren<LayoutGroup>();

        lines.SetLine(target1, target2, Vector3.zero, 1 / 0.9f);

        //lines.SetLine(target1.position, target2.position, new Vector2(159, 50));

    }
}
