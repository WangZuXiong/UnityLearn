using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

#if !UNITY_EDITOR
public class ConditionTest : MonoBehaviour
{
    private int index = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetText(index++.ToString());
        }
    }
    

    [Conditional("DEBUG_ENABLE")]

    private void SetText(string str)
    {
        transform.Find("Text").GetComponent<Text>().text = str;
    }

}
#endif