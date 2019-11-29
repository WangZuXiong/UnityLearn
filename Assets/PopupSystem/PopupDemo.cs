using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PopupSystem.Instance.Pop<DemoWindowController>("Window");
        }
        else if (Input.GetMouseButtonDown(2))
        {
            PopupSystem.Instance.CloseAllPop();
        }
    }
}
