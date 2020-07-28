using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    Vector3 target;
    public float speed = 100;

    public Vector3 startPos;
    public Vector3 v;
    //花费时间
    float t = 0;
    //加速度
    float a = 5;
    private void Start()
    {
        target = new Vector3(0, 0, 1000);
        startPos = this.transform.position;
        v = (target - startPos).normalized;

        vector3 = target - transform.position;
    }
    //1.
    //2.A加速移动到B 时间t


    Vector3 vector3;

    private void Update()
    {
        float distance = Vector3.Distance(this.transform.position, target);

        //if (distance > 1)
        //{
        //    transform.position = startPos + v * speed * Time.deltaTime * a * (t++);
        //}

        //if (distance > 1)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, target, 1);
        //}

        if (distance > 1)
        {
            speed += (Time.deltaTime * 5);
            transform.position = transform.position + vector3.normalized * speed * Time.deltaTime;
        }
    }

    //补全下面代码的 Update函数,让这个 Gameobject以 speed的速度向 target移动,当与 target的距离小于等于1的时候停止
}
