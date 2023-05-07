using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the base class for all pawns which have collisions
// it handles any collisions with pawns and bullets
// note that this class may destroy the gameObject

public class BaseCollisions : MonoBehaviour
{
    [SerializeField]protected SpriteRenderer sprite;
    [SerializeField]private Collider2D colliderPhysics;
    [SerializeField]protected int healthMax = 1;
    [SerializeField]private int scoreKill = 10;
    public int health { get; protected set; }
    protected bool alive;
    protected bool invulnerable;

    private void Awake()
    {
        health = healthMax;
        alive = true;
        invulnerable = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet)
            {
                BulletHit(bullet);
            }
        }
    }

    protected virtual void BulletHit(Bullet bullet)
    {
        TakeDamage(bullet.damage);
    }

    public virtual void TakeDamage(int amount)
    {
        if (alive && !invulnerable)
        {
            health -= amount;
            //Debug.Log(gameObject + " took " + bullet.damage + " damage - health left: " + health);
            if (health <= 0) Death();
        }
    }

    protected virtual void Death()
    {
        EffectTimed pop = Instantiate(PrefabProvider.instance.particlePop, transform.position, PrefabProvider.instance.particlePop.transform.rotation);
        pop.Trigger(sprite.color, transform.localScale);
        StageManager.instance.AddScore(scoreKill);
        Destroy(gameObject);
    }

    public void SetVulnerable(bool vulnerable, Color spriteColor)
    {
        colliderPhysics.enabled = vulnerable;
        invulnerable = !vulnerable;
        sprite.color = spriteColor;
    }
}
