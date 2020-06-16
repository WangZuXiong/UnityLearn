using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ScriptableTool : Editor
{
    [MenuItem("Tools/Scriptable Test/Set True")]
    private static void Test1()
    {
        var myClass = Resources.Load<MyClass>("MyClass");
        myClass.BoolValue = true;
        myClass.IntValue = 1;
        myClass.StrValue = "true";

        Debug.LogError(myClass.BoolValue.ToString());
        Debug.LogError(myClass.IntValue.ToString());
        Debug.LogError(myClass.StrValue);

    }

    [MenuItem("Tools/Scriptable Test/Set False")]
    private static void Test0()
    {
        var myClass = Resources.Load<MyClass>("MyClass");
        myClass.BoolValue = false;
        myClass.IntValue = 0;
        myClass.StrValue = "false";

        Debug.LogError(myClass.BoolValue.ToString());
        Debug.LogError(myClass.IntValue.ToString());
        Debug.LogError(myClass.StrValue);
    }
}
