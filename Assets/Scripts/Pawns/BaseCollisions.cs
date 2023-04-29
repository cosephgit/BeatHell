using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the base class for all pawns which have collisions
// it handles any collisions with pawns and bullets
// note that this class may destroy the gameObject

public class BaseCollisions : MonoBehaviour
{
    [SerializeField]private SpriteRenderer sprite;
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
        EffectTimed pop = Instantiate(PrefabProvider.instance.particlePop, transform.position, PrefabProvider.instance.particlePop.transform.rotation);
        pop.Trigger(sprite.color, transform.localScale);
        Destroy(gameObject);
    }
}
