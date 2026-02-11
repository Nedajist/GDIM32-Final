using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody2D;
    [SerializeField] float _moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D.velocity = (Vector2.left * _moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -5)
        {
            Destroy(gameObject);
        }
    }
}
