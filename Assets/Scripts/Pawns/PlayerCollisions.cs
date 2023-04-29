using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player collider needs special handling as each time the player is hit by a bullet, they absorb the bullet

public class PlayerCollisions : BaseCollisions
{
    // need a reference to the magazine to refill it
    [SerializeField]private PlayerMagazine magazine;

    void Start()
    {
        UIManager.instance.UpdateHealth((float)health / (float)healthMax);
    }

    protected override void BulletHit(Bullet bullet)
    {
        // add the key bullet data to the player's magazine so the player can fire it
        if (bullet.shotStored)
            magazine.AddShot(bullet.shotStored);

        health -= bullet.damage;

        UIManager.instance.UpdateHealth((float)health / (float)healthMax);

        Debug.Log(gameObject + " took " + bullet.damage + " damage - health left: " + health);
        if (health <= 0) Death();
    }

    protected override void Death()
    {
        EffectTimed pop = Instantiate(PrefabProvider.instance.particlePop, transform.position, PrefabProvider.instance.particlePop.transform.rotation);
        pop.Trigger(sprite.color, transform.localScale);
        GameManager.instance.PlayerDeath();
        Destroy(gameObject);
    }
}
