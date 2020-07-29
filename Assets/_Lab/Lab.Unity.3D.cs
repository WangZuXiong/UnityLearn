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

    /// <summary>
    /// 角度和弧度之间的关系
    /// </summary>
    void DegAndRad()
    {

        //角度：degree
        //弧度：radian

        //角度——>弧度   Mathf.Deg2Rad = (float)Math.PI / 180f

        //角度30转化为弧度     Mathf.Deg2Rad * 30 即 (30 * Math.PI) / 180 = 0.52

        //弧度——>角度   Mathf.Rad2Deg = 57.29578f = 180 / Math.PI

        //弧度0.52转化为角度       Mathf.Rad2Deg * 0.52 即 (0.52 * 180) / Math.PI = 29.8
    }


    void Move()
    {
        if (Vector3.Distance(transform.position, target) > 1)
        {
            // 1
            //transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);

            // 2
            //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            //3
            //Vector3 translation = (target - transform.position).normalized;
            //transform.Translate(translation * speed * Time.deltaTime);

            //4
            //transform.GetComponent<Rigidbody>().MovePosition(target);

            //5
            //Vector3 translation = (target - transform.position).normalized;
            //transform.GetComponent<Rigidbody>().velocity = translation * speed * Time.deltaTime;

            //匀变速直线运动
            _t += Time.deltaTime;
            transform.position = transform.position + new Vector3(_speed * _t + 0.5f * _a * Mathf.Pow(_t, 2), 0, 0);
        }
    }


    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _a;
    [SerializeField]
    private float _t;
}
