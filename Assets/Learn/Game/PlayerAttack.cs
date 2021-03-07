using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Camera _camera;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _camera = Camera.main;
        _animator = transform.Find("Slash").GetComponent<Animator>();
        _spriteRenderer = _animator.transform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //鼠标点击为主与player之间的方向向量
            Vector2 diff = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            //水平位置与向量之间的夹角
            float rad = Mathf.Atan2(diff.y, diff.x);
            //弧度转为角度  transform.rotation中的是角度单位
            float deg = rad * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, deg);


            Attack();
        }
    }

    private void Attack()
    {
        _spriteRenderer.color = Color.white;
        _animator.Play("Slash");
    }
}
