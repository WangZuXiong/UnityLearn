using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _axisH, _axisV;
    [Range(1, 20)]
    [SerializeField]
    private float _speed = 10;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = new Vector2(_axisH, _axisV) * _speed;
        Flip();
    }

    private void Update()
    {
        _axisH = Input.GetAxis("Horizontal");
        _axisV = Input.GetAxis("Vertical");
    }

    private void Flip()
    {
        if (_axisH > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (_axisH < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
