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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PopupSystem.Instance.GetPopup<DemoWindowController>("DemoWindow");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            PopupSystem.Instance.GetPopup<DemoWindowController1>("DemoWindow_1");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PopupSystem.Instance.CloseAllPopup();
        }
    }
}
