using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the basic shooting class for pawns

/*
ok so what does this need?
frames between shots (EVERYTHING IN FRAMES!)
*/

public class BaseShooting : MonoBehaviour
{
    [SerializeField]private ShotDetails shotDetails;
    private int beatFracsSinceShot;
    private float[] shotAngles;
    protected bool shooting = false;
    protected float shotFacing = 0; // the angle which the shots should be cented around

    void Awake()
    {
        // set up the firing angles for the desired number of shots
        if (shotDetails.shotCount > 0)
        {
            shotAngles = new float[shotDetails.shotCount];
            if (shotDetails.shotCount > 1)
            {
                float minAngle = -shotDetails.cone / 2f;
                float angleStep = shotDetails.cone / (shotDetails.shotCount - 1);
                for (int i = 0; i < shotDetails.shotCount; i++)
                {
                    shotAngles[i] = minAngle + (angleStep * i);
                }
            }
            else
            {
                shotAngles[0] = 0f;
            }
        }
    }

    private void Start()
    {
        beatFracsSinceShot = shotDetails.beatFracsOffset;
        BeatManager.onBeatFrac += BeatFractionShoot;
    }

    // the basic shooting function
    protected virtual void Shoot()
    {
        if (shotDetails.shotCount > 0)
        {
            for (int i = 0; i < shotDetails.shotCount; i++)
            {
                float fireAngle = shotFacing + transform.eulerAngles.z + shotAngles[i];
                Quaternion bulletRotation = Quaternion.Euler(0, 0, fireAngle);
                Bullet bulletFired = BulletLibrary.instance.GetBullet();

                bulletFired.Shoot(transform.position, bulletRotation, shotDetails.shotDamage, shotDetails.shotColor, shotDetails.shotSpeed, shotDetails.shotLife, shotDetails.layer);
            }
        }
    }

    // called every beat fraction
    private void BeatFractionShoot(int count)
    {
        if (beatFracsSinceShot == 0)
        {
            if (shooting)
            {
                Shoot();
                beatFracsSinceShot++;
            }
        }
        else
        {
            beatFracsSinceShot++;
        }
        if (beatFracsSinceShot == shotDetails.beatFracsPerShot)
        {
            beatFracsSinceShot = 0;
        }
    }
}
