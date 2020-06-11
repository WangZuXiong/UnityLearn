using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LabInEditMode : MonoBehaviour
{
    public Transform Target;

    public float Speed = 1;
    void Update()
    {
        //Target.Translate(Vector3.right * Time.deltaTime * 10);

        //transform.LookAt(Target);



        if (Input.GetMouseButton(0))
        {
            Target.Translate(Vector3.right* Speed);
            Speed += 1;
        }
    }
}
