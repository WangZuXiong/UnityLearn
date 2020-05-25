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
}
