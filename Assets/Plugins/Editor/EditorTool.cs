using EditorFramework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorTool : MonoBehaviour
{

    [MenuItem("GameObject/UI/Custom Image")]
    static void CreatCustomImage()
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

    [MenuItem("GameObject/UI/Custom Text")]
    static void CreateCustomText()
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

    [MenuItem("GameObject/Copy Transform Path", false, 21)]
    static void CopyTransformPath()
    {
        Transform selectedTransform = Selection.activeTransform;
        string prefabRoot = selectedTransform.GetNodePath();
        int index = prefabRoot.IndexOf("/");
        prefabRoot = prefabRoot.Remove(0, index + 1);
        // 复制粘贴到剪贴板
        GUIUtility.systemCopyBuffer = prefabRoot;
    }

    [MenuItem("Tools/Find Target Prefabs")]
    static void FindTargetPrefabs()
    {
        var path = Application.dataPath + "/Resources";
        var outputPaths = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
        FileUtility.WriteTextToLocal1(Application.dataPath, "PrefabsPaths.txt", string.Join("\n", outputPaths.ToArray()));
    }



}

