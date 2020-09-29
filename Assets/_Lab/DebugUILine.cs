using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUILine : MonoBehaviour
{
    static readonly Vector3[] fourCorners = new Vector3[4];

    private void OnDrawGizmos()
    {
        foreach (MaskableGraphic g in GetMaskableGraphics())
        {
            RectTransform rectTransform = g.transform as RectTransform;
            rectTransform.GetWorldCorners(fourCorners);
            Gizmos.color = Color.blue;
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
            }
        }
    }

    private MaskableGraphic[] GetMaskableGraphics()
    {
        //Debug.LogError(1);
        return GameObject.FindObjectsOfType<MaskableGraphic>();
    }
}
