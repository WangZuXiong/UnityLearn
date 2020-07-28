using System.Collections.Generic;
using System.Text;
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

    [MenuItem("GameObject/UI/Image", true)]
    static void CreatNoRaycastTargetImage()
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

    [MenuItem("GameObject/Copy Path", false, 21)]
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

    [MenuItem("GameObject/Gen Code", false, 22)]
    static void GenCode()
    {
        var temp = Selection.activeTransform;
        if (temp == null)
        {
            return;
        }

        var btns = temp.GetComponentsInChildren<Button>(true);

        List<string> vs = new List<string>();
        for (int i = 0; i < btns.Length; i++)
        {
            var path = GetTransformFullPath(temp, btns[i].transform);
            var str = string.Format("Button {0} = transform.Find(\"{1}\").GetComponent<Button>();", btns[i].transform.name, path);
            vs.Add(str);
        }
        var result = string.Join("\n", vs.ToArray());

        TextEditor textEditor = new TextEditor
        {
            text = result
        };
        textEditor.OnFocus();
        textEditor.Copy();
    }


    private static string GetTransformFullPath(Transform parent, Transform target)
    {
        List<string> vs = new List<string>();


        while (target.parent != null && !target.parent.Equals(parent))
        {
            vs.Add(target.name);
            target = target.parent;
        }
        vs.Reverse();

        return string.Join("/", vs.ToArray());
    }
}

