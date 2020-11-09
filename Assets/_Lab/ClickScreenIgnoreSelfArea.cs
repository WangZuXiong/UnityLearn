using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 点击了屏幕除去自身以外的位置
/// </summary>
public class ClickScreenIgnoreSelfArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //    //会受到canvas rander model 的影响

            //    //canvas rander model = screen space - overlay [only]
            //    //Vector3[] fourCornersArray = new Vector3[4];
            //    //RectTransform rectTransform = GetComponent<RectTransform>();
            //    //rectTransform.GetWorldCorners(fourCornersArray);

            //    //var pos = Input.mousePosition;
            //    //bool isInRect =
            //    //    pos.x >= fourCornersArray[0].x &&
            //    //    pos.x <= fourCornersArray[2].x &&
            //    //    pos.y >= fourCornersArray[0].y &&
            //    //    pos.y <= fourCornersArray[2].y;
            //    //Debug.LogError(isInRect);








            //canvas rander model = screen space - camera || canvas rander model = world space
            Vector3[] fourCornersArray = new Vector3[4];
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.GetWorldCorners(fourCornersArray);

            bool isInRect = false;
            Plane plane = new Plane(fourCornersArray[0], fourCornersArray[1], fourCornersArray[2]);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float enter))
            {
                var pos = ray.GetPoint(enter);

                //1     2
                //  pos
                //0     3
                isInRect = Vector3.Cross(pos - fourCornersArray[0], fourCornersArray[1] - fourCornersArray[0]).z > 0
                        && Vector3.Cross(pos - fourCornersArray[1], fourCornersArray[2] - fourCornersArray[1]).z > 0
                        && Vector3.Cross(pos - fourCornersArray[2], fourCornersArray[3] - fourCornersArray[2]).z > 0
                        && Vector3.Cross(pos - fourCornersArray[3], fourCornersArray[0] - fourCornersArray[3]).z > 0;

                Debug.DrawLine(pos, fourCornersArray[1], Color.red);
                Debug.DrawLine(fourCornersArray[2], fourCornersArray[1], Color.red);


            }



            Debug.LogError(isInRect);







            //    ////会受到transform 的缩放 的影响
            //    //RectTransform r = GetComponent<RectTransform>();
            //    //var x = 0.5f * Screen.width + r.anchoredPosition.x - r.pivot.x * r.rect.width;
            //    //var y = 0.5f * Screen.height + r.anchoredPosition.y - r.pivot.y * r.rect.height;
            //    //Rect rect = new Rect(x, y, r.rect.width, r.rect.height);
            //    //Debug.LogError(rect.Contains(Input.mousePosition));


            //    //canvas rander model = screen space - overlay [only]
            //    RectTransform r = GetComponent<RectTransform>();
            //    Vector3[] fourCornersArray = new Vector3[4];
            //    r.GetWorldCorners(fourCornersArray);

            //    //1 2
            //    //0 3
            //    var width = Mathf.Abs(fourCornersArray[3].x - fourCornersArray[0].x);
            //    var height = Mathf.Abs(fourCornersArray[1].y - fourCornersArray[0].y);

            //    var x = 0.5f * Screen.width + r.anchoredPosition.x - r.pivot.x * width;
            //    var y = 0.5f * Screen.height + r.anchoredPosition.y - r.pivot.y * height; ;




            //    Rect rect = new Rect(x, y, width, height);
            //    Debug.LogError(rect.Contains(Input.mousePosition));
        }
    }


    private void DrawRectTransform(RectTransform rectTransform)
    {
        Vector3[] fourCornersArray = new Vector3[4];
        rectTransform.GetWorldCorners(fourCornersArray);
        Gizmos.color = Color.blue;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(fourCornersArray[i], fourCornersArray[(i + 1) % 4]);
        }
    }
}
