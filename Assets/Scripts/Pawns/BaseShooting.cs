using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the basic shooting class for pawns
// this class must be resilient to being destroyed by another class

public class BaseShooting : MonoBehaviour
{
    [Header("shotDetails is just a prefab data reference")]
    [SerializeField]private ShotDetails shotDetails;
    [Header("magazine must be reference to a real object")]
    [SerializeField]private BaseMagazine magazine;
    [SerializeField]private Color shotColor = Color.yellow;
    [SerializeField]private Layer shotLayer = Layer.EnemyBullet;
    private int beatFracsSinceShot;
    private float[] shotAngles;
    protected bool shooting = false;
    protected float shotFacing = 0; // the angle which the shots should be cented around

    protected virtual void Awake()
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
                Shot shot = magazine.GetShot();

                if (shot)
                {
                    bulletFired.Shoot(transform.position, bulletRotation, shot, shotColor, shotLayer, IsPlayer());
                    ShotFired();
                }
            }
        }
    }

    // called when a shot is successfully fired
    protected virtual void ShotFired()
    {

    }

    private void OnDestroy()
    {
        BeatManager.onBeatFrac -= BeatFractionShoot;
    }

    // called every beat fraction
    protected virtual void BeatFractionShoot(int count)
    {
        if (isActiveAndEnabled)
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

    protected virtual bool IsPlayer()
    {
        return false;
    }
}
