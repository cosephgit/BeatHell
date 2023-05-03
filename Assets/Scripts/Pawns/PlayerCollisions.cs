using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player collider needs special handling as each time the player is hit by a bullet, they absorb the bullet

public class PlayerCollisions : BaseCollisions
{
    // need a reference to the magazine to refill it
    [SerializeField]private PlayerMagazine magazine;
    [SerializeField]private int healthRegenBarDelay = 2; // beat fractions between each health recovery
    private int barCount = 0;
    private bool absorbActive = false;

    void Start()
    {
        UIManager.instance.UpdateHealth((float)health / (float)healthMax);
        BeatManager.onBar += HealthRegenBar;
    }

    protected override void BulletHit(Bullet bullet)
    {
        // add the key bullet data to the player's magazine so the player can fire it
        if (bullet.shotStored)
            magazine.AddShot(bullet.shotStored);

        if (!absorbActive)
        {
            health -= bullet.damage;

            UIManager.instance.UpdateHealth((float)health / (float)healthMax);

            //Debug.Log(gameObject + " took " + bullet.damage + " damage - health left: " + health);
            if (health <= 0) Death();
        }
    }

    protected override void Death()
    {
        EffectTimed pop = Instantiate(PrefabProvider.instance.particlePop, transform.position, PrefabProvider.instance.particlePop.transform.rotation);
        pop.Trigger(sprite.color, transform.localScale);
        GameManager.instance.PlayerDeath();
        Destroy(gameObject);
    }

    private void HealthRegenBar()
    {
        if (health < healthMax && health > 0)
        {
            barCount++;
            if (barCount == healthRegenBarDelay)
            {
                health++;
                UIManager.instance.UpdateHealth((float)health / (float)healthMax);
                barCount = 0;
            }
        }
        else if (barCount > 0)
            barCount = 0;
    }

    // this is called by PlayerShooting when the player changes their bullet absorb state
    public void AbsorbState(bool active)
    {
        absorbActive = active;
    }
}
