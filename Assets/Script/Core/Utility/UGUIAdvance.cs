#if UNITY_EDITOR
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

}
#endif
