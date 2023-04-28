using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the base class for all pawns which have collisions
// it handles any collisions with pawns and bullets

public class BaseCollisions : MonoBehaviour
{
    [SerializeField]private Collider2D colliderPhysics;
    [SerializeField]private int healthMax = 1;
    private int health;
    private bool alive;

    private void Awake()
    {
        health = healthMax;
        alive = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            Debug.Log("trigger!");
            if (bullet)
            {
                health -= bullet.damage;
                Debug.Log(gameObject + " took " + bullet.damage + " damage - health left: " + health);
                if (health <= 0) Death();
            }
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
