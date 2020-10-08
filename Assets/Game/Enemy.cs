using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditorInternal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _maxBlood = 100;
    [SerializeField]
    private int _currentBlood;
    [SerializeField]
    private float _speed = 10;
    private Transform _target;

    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private float _duration = 1;
    [SerializeField]
    private float _temp;
    private void Start()
    {
        _currentBlood = _maxBlood;
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    private void Update()
    {
        if (_temp <= 0)
        {
            _spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }
        else
        {
            _temp -= Time.deltaTime;
        }
    }

    internal void TakeDamage(int damage)
    {
        HurtShader();
        _currentBlood -= damage;
        if (_currentBlood <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void HurtShader()
    {
        _spriteRenderer.material.SetFloat("_FlashAmount", 1);
        _temp = _duration;
    }
}
