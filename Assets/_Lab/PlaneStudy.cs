using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneStudy : MonoBehaviour
{
    public float fieldLength;
    public float fieldWidth;
    public Plane goalLine1;
    public Plane goalLine2;
    public Plane leftSideLine;
    public Plane rightSideLine;

    void Start()
    {

    }


    private void Update()
    {
        // Set up goal lines of a playing field.
        goalLine1.SetNormalAndPosition(Vector3.forward, Vector3.forward * fieldLength / 2);
        goalLine1.SetNormalAndPosition(-Vector3.forward, -Vector3.forward * fieldLength / 2);
        // Set up side lines.
        leftSideLine.SetNormalAndPosition(-Vector3.right, -Vector3.right * fieldWidth / 2);
        leftSideLine.SetNormalAndPosition(Vector3.right, Vector3.right * fieldWidth / 2);



        DrawPlane(Vector3.forward * fieldLength / 2, fieldLength, 10);
        DrawPlane(-Vector3.forward * fieldLength / 2, fieldLength, 10);

 

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (goalLine1.Raycast(ray, out float enter1))
        {
            var targetPoint = ray.GetPoint(enter1);
            Debug.DrawLine(Vector3.zero, targetPoint, Color.red);
        }


        if (goalLine2.Raycast(ray, out float enter2))
        {
            var targetPoint = ray.GetPoint(enter2);
            Debug.DrawLine(Vector3.zero, targetPoint, Color.yellow);
        }
    }



    private void DrawPlane(Vector3 centralPoint, float width, float height)
    {
        //1 2
        //4 3
        Vector3 point1 = new Vector3(centralPoint.x - (width / 2), centralPoint.x + (height / 2), centralPoint.z);
        Vector3 point2 = new Vector3(centralPoint.x + (width / 2), centralPoint.x + (height / 2), centralPoint.z);
        Vector3 point3 = new Vector3(centralPoint.x + (width / 2), centralPoint.x - (height / 2), centralPoint.z);
        Vector3 point4 = new Vector3(centralPoint.x - (width / 2), centralPoint.x - (height / 2), centralPoint.z);

        Debug.DrawLine(point1, point2);
        Debug.DrawLine(point2, point3);
        Debug.DrawLine(point3, point4);
        Debug.DrawLine(point4, point1);

        Debug.DrawLine(point1, point3);
        Debug.DrawLine(point2, point4);
    }
}
