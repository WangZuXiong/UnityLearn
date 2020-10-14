using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField]
    private int _damage = 10;
    [SerializeField]
    private int _offset = 1;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    public void Hide()
    {
        _spriteRenderer.color = Color.clear;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.transform.GetComponent<Enemy>().TakeDamage(_damage);

            Vector3 temp = (other.transform.position - transform.position).normalized * _offset;
            other.transform.position += temp;
        }
    }
}
