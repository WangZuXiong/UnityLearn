#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UGUIAdvance : MonoBehaviour
{

    [MenuItem("GameObject/UI/Image_NoRaycastTarget")]
    static void CreatImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Image", typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.layer = Selection.activeTransform.gameObject.layer;
            }
        }
    }

    [MenuItem("GameObject/UI/Text_NoRaycastTarget")]
    static void CreatText()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Text", typeof(Text));
                Text text = go.GetComponent<Text>();
                if (text != null)
                {
                    text.raycastTarget = false;
                }

                go.transform.SetParent(Selection.activeTransform);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.layer = Selection.activeTransform.gameObject.layer;
            }
        }
    }


    //[MenuItem("Assets/FindNodePath &q", false, 21)]
    [MenuItem("GameObject/FindNodePath &q", false, 1)]
    static void OutputNodePath2()
    {
        Transform selectedTransform = Selection.activeTransform;
        string prefabRoot = selectedTransform.GetNodePath();
        prefabRoot = prefabRoot.Replace(".", "/");
        int index = prefabRoot.IndexOf("/");
        prefabRoot = prefabRoot.Remove(0, index + 1);
        // 复制粘贴到剪贴板
        GUIUtility.systemCopyBuffer = prefabRoot;
    }
}
#endif
