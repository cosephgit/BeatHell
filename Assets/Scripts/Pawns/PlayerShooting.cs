using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player shooting class to take the player attack inputs

// only the player weapon has an audio source, enemy sounds are handled differently

public class PlayerShooting : BaseShooting
{
    [SerializeField]private AudioSource shootSound;
    [SerializeField]private PlayerCollisions playerCollisions;
    [SerializeField]private int absorbBeatFracs = 4; // number of beat fractions that the absorb lasts for
    [SerializeField]private int absorbBeatFracsRecover = 12; // number of beat fractions before absorb is ready again
    [SerializeField]private float absorbRadius = 2f; // radius in which bullets are drawn
    [SerializeField]private float absorbPower = 4f; // speed at which bullets are drawn towards player
    private bool absorbBullet = false; // is bullet absorbtion active?
    private int absorbBeatFracsDone = 0; // number of beat fractions that absorb has been running for

    protected override void Shoot()
    {
        base.Shoot();
        if (!Input.GetButton("Fire1")) shooting = false; // stop shooting now that at least one shot attempt has been fired
    }

    protected override void ShotFired()
    {
        shootSound.Play();
    }

    void FixedUpdate()
    {
        if (absorbBullet)
        {
            Collider2D[] bulletNear = Physics2D.OverlapCircleAll(transform.position, absorbRadius, Global.LayerEnemyBullet());
            foreach (Collider2D bullet in bulletNear)
            {
                Vector3 pos = transform.position - bullet.transform.position;

                if (pos.magnitude > absorbPower * Time.fixedDeltaTime) // if the distance is greater than the absorb speed for this frame, scale it down
                {
                    pos = pos.normalized * absorbPower * Time.fixedDeltaTime;
                }

                pos = pos + bullet.transform.position;

                bullet.transform.position = pos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!absorbBullet)
        {
            if ((absorbBeatFracsDone == 0) && Input.GetButton("Fire2"))
            {
                absorbBullet = true; // start absorbing bullets
                playerCollisions.AbsorbState(true);
                shooting = false;
            }
            else if (Input.GetButton("Fire1")) shooting = true; // only sets shooting to true, so the player can tap shoot and will still fire on their next shoot beat
            if (shooting)
            {
                // prepare to shoot in the direction of the mouse pointer
                Vector3 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                shotFacing = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
            }
        }
    }

    protected override void BeatFractionShoot(int count)
    {
        if (absorbBullet)
        {
            absorbBeatFracsDone++;
            if (absorbBeatFracsDone == absorbBeatFracs)
            {
                absorbBullet = false;
                playerCollisions.AbsorbState(false);
                // but don't reset the counter yet
            }
        }
        else
        {
            if (absorbBeatFracsDone > 0)
            {
                absorbBeatFracsDone++;
                if (absorbBeatFracsDone >= absorbBeatFracs + absorbBeatFracsRecover)
                {
                    absorbBeatFracsDone = 0;
                }
            }
            base.BeatFractionShoot(count);
        }
    }
}
