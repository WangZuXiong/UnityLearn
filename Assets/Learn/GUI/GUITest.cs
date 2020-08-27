using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GUITest : MonoBehaviour
{
    private float _t;
    private void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, 300, 300));

        GUI.Box(new Rect(50, 50, 300, 300), "Group in here");

        //固定布局
        if (GUI.Button(new Rect(50, 70, 100, 30), "GUI Button"))
        {
            Debug.Log("固定布局");
        }
        //自动布局
        if (GUILayout.Button("GUILayout Button"))
        {
            Debug.Log("自动布局");
        }
        _t = GUILayout.HorizontalSlider(_t, 0, 5);

        GUILayout.Box(_t.ToString());
        GUILayout.Box("Box");

        //GUILayout.Toggle(,)

        GUI.EndGroup();
    }
}
