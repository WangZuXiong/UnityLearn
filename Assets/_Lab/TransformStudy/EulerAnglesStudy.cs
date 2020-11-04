using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EulerAnglesStudy : MonoBehaviour
{
    //public float _scale = 5.0f;

    //private float _temp;

    //void Start()
    //{
    //    // Print the rotation around the global X Axis
    //    print(transform.eulerAngles.x);
    //    // Print the rotation around the global Y Axis
    //    print(transform.eulerAngles.y);
    //    // Print the rotation around the global Z Axis
    //    print(transform.eulerAngles.z);
    //}

    //private void FixedUpdate()
    //{
    //    transform.eulerAngles = -new Vector3(0, _temp * _scale, 0);
    //}

    //void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        var mouseX = Input.GetAxis("Mouse X");
    //        _temp += mouseX;
    //        Debug.Log(mouseX);
    //    }


    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        transform.DetachChildren();
    //    }

    //    //transform.RotateAround(,)
    //}


    public float xAngle, yAngle, zAngle;
    public Material selfMat, worldMat;

    private GameObject cube1, cube2;

    void Awake()
    {
        cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube1.transform.position = new Vector3(0.75f, 0.0f, 0.0f);
        cube1.GetComponent<Renderer>().material = selfMat;
        cube1.name = "Self";

        cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = new Vector3(-0.75f, 0.0f, 0.0f);
        cube2.GetComponent<Renderer>().material = worldMat;
        cube2.name = "World";
    }

    void Update()
    {
        cube1.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
        cube2.transform.Rotate(xAngle, yAngle, zAngle, Space.World);
    }
}
