using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMatrix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //local position → world position
        Vector3 worldPos = transform.parent.localToWorldMatrix.MultiplyPoint(transform.localPosition);
        Debug.Log(worldPos);


        //world position → camera position 相对于相机的坐标
        //从世界坐标转换到对于父节点下的local position
        Vector3 cameraPos = Camera.main.transform.worldToLocalMatrix.MultiplyPoint(worldPos);
        Debug.Log(cameraPos);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
