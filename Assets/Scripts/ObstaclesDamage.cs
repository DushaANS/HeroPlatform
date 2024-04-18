using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDamage : Entity
{
    private int lives = 3;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            lives--;
        }
        if(lives < 1)
            Die();
    }
}
