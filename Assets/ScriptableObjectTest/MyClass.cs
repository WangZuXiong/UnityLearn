using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class MyClass : ScriptableObject
{
    private MyClass() { }

    [SerializeField]
    public bool BoolValue;

    [SerializeField]
    public int IntValue;

    public string StrValue;

    //public bool BoolTest
    //{
    //    get
    //    {
    //        return _boolTest;
    //    }
    //}

    //public void SetBool(bool value)
    //{
    //    _boolTest = value;
    //    EditorUtility.SetDirty(Instance);
    //}

    //private static MyClass _instance;

    //public static MyClass Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = Resources.Load<MyClass>("MyClass");
    //        }
    //        if (_instance == null)
    //        {
    //            _instance = CreateInstance<MyClass>();
    //            AssetDatabase.CreateAsset(_instance, "Assets/Resources/MyClass.asset");
    //        }
    //        return _instance;
    //    }
    //}
}
