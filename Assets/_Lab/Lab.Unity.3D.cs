using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Lab : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _bullet;

    /// <summary>
    /// 在 unity程序中,实现一个3d空间里子弹以30度角抛射以10为速度抛物线运动击中目标角色
    /// </summary>  
    void Shoot()
    {
        _bullet.transform.Rotate(Vector3.left, 30f);
        _bullet.velocity = new Vector3(0, 0, 10);
    }

    /// <summary>
    /// 在unity程序中，实现一个3D空间里子弹以30度角抛射以10f的速度的抛物线运动击中目标角色
    /// </summary>
    void BulletBehaviour()
    {

        //子弹先转向在添加力
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.localEulerAngles = new Vector3(-30, 0, 0);
        //cube.AddComponent<Rigidbody>().AddForce(cube.transform.forward * _force);

        //子弹直接添加对于角度的力
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 force = new Vector3(0, Mathf.Tan(Mathf.Deg2Rad * 30), 1).normalized;
        cube.AddComponent<Rigidbody>().AddForce(force * _force);
    }
}
