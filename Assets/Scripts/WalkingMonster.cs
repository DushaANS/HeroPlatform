using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WalkingMonster : Entity
{
    private float speed = 3.5f;
    private Vector3 dir;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lives = 5;
        dir = transform.right;
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f);
        if (colliders.Length > 0) dir *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
