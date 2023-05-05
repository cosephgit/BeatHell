using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player shooting class to take the player attack inputs

// only the player weapon has an audio source, enemy sounds are handled differently

public class PlayerShooting : BaseShooting
{
    [SerializeField]private AudioSource shootSound;
    [SerializeField]private PlayerCollisions playerCollisions;
    [Header("Bullet magnet")]
    [SerializeField]private SpriteRenderer absorbArea;
    [SerializeField]private SpriteRenderer absorbReady;
    [SerializeField]private Color absorbStart = Color.yellow;
    [SerializeField]private Color absorbEnd = Color.clear;
    [SerializeField]private AudioSource absorbSound;
    [SerializeField]private int absorbBeatFracs = 4; // number of beat fractions that the absorb lasts for
    [SerializeField]private int absorbBeatFracsRecover = 12; // number of beat fractions before absorb is ready again
    [SerializeField]private float absorbRadius = 2f; // radius in which bullets are drawn
    [SerializeField]private float absorbPower = 4f; // speed at which bullets are drawn towards player
    private bool absorbBullet = false; // is bullet absorbtion active?
    private int absorbBeatFracsDone = 0; // number of beat fractions that absorb has been running for

    protected override void Awake()
    {
        base.Awake();
        absorbReady.enabled = true;
        absorbReady.color = absorbStart;
        InitWeapon(0);
    }
    protected override void Shoot()
    {
        base.Shoot();
        if (!Input.GetButton("Fire1")) shooting = false; // stop shooting now that at least one shot attempt has been fired
    }

    protected override void ShotFired()
    {
        shootSound.Play();
        if (magazine.Empty() && GameManager.instance.stage[GameManager.instance.slotActive] == 0)
            UIMousePointer.instance.ShowHintAbsorb();
    }

    void FixedUpdate()
    {
        if (absorbBullet && (absorbBeatFracsDone > 0))
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
        // make sure we're not in the menu and not in bullet absorb mode
        if (!StageManager.instance.menuOpen && !absorbBullet)
        {
            if ((absorbBeatFracsDone == 0) && Input.GetButton("Fire2"))
            {
                absorbBullet = true; // start absorbing bullets
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
            if (absorbBeatFracsDone == 0)
            {
                playerCollisions.AbsorbState(true);
                absorbSound.Play();
                absorbArea.enabled = true;
                absorbReady.enabled = false;
            }
            if (absorbBeatFracsDone == absorbBeatFracs)
            {
                absorbArea.enabled = false;
                absorbBullet = false;
                playerCollisions.AbsorbState(false);
                // but don't reset the counter yet
            }
            else
            {
                absorbArea.color = Color.Lerp(absorbStart, absorbEnd, ((float)absorbBeatFracsDone / (float)absorbBeatFracs) - 0.5f);
            }
            absorbBeatFracsDone++;
        }
        else
        {
            if (absorbBeatFracsDone > 0)
            {
                if (absorbBeatFracsDone >= absorbBeatFracs + absorbBeatFracsRecover)
                {
                    absorbReady.enabled = true;
                    absorbReady.color = absorbStart;
                    absorbBeatFracsDone = 0;
                }
                else
                    absorbBeatFracsDone++;
            }
            base.BeatFractionShoot(count);
        }
    }

    protected override bool IsPlayer()
    {
        return true;
    }
}
