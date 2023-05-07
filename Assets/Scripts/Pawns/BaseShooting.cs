using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the basic shooting class for pawns
// this class must be resilient to being destroyed by another class

public class BaseShooting : MonoBehaviour
{
    [Header("shotDetails is just a prefab data reference")]
    [SerializeField]private ShotDetails[] shotDetails;
    [Header("magazine must be reference to a real object")]
    [SerializeField]protected BaseMagazine magazine;
    [SerializeField]private Color shotColor = Color.yellow;
    [SerializeField]private Layer shotLayer = Layer.EnemyBullet;
    [SerializeField]private string shotSortLayer = Global.LAYERENEMYBULLET;
    private ShotDetails shotSelected;
    private int beatFracsSinceShot;
    private float[] shotAngles;
    protected bool shooting = false;
    protected float shotFacing = 0; // the angle which the shots should be cented around

    protected virtual void Awake()
    {
    }

    protected virtual void InitWeapon(float weaponKey)
    {
        shotSelected = shotDetails[Mathf.FloorToInt(weaponKey * (float)shotDetails.Length)];
        // set up the firing angles for the desired number of shots
        if (shotSelected.shotCount > 0)
        {
            shotAngles = new float[shotSelected.shotCount];
            if (shotSelected.shotCount > 1)
            {
                float minAngle = -shotSelected.cone / 2f;
                float angleStep = shotSelected.cone / (shotSelected.shotCount - 1);
                for (int i = 0; i < shotSelected.shotCount; i++)
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

    protected virtual void Start()
    {
        beatFracsSinceShot = shotSelected.beatFracsOffset;
        BeatManager.onBeatFrac += BeatFractionShoot;
    }

    // the basic shooting function
    protected virtual void Shoot()
    {
        if (shotSelected.shotCount > 0)
        {
            for (int i = 0; i < shotSelected.shotCount; i++)
            {
                float fireAngle = shotFacing + transform.eulerAngles.z + shotAngles[i];
                Quaternion bulletRotation = Quaternion.Euler(0, 0, fireAngle);
                Bullet bulletFired = BulletLibrary.instance.GetBullet();
                Shot shot = magazine.GetShot();

                if (shot)
                {
                    bulletFired.Shoot(transform.position, bulletRotation, shot, shotColor, shotLayer, shotSortLayer, IsPlayer());
                    ShotFired();
                }
                else
                    ShotFailed();
            }
        }
    }

    // called when a shot is successfully fired
    protected virtual void ShotFired()
    {

    }

    // called when a shot fails to fire (no ammo)
    protected virtual void ShotFailed()
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
            else if (beatFracsSinceShot > 0)
            {
                beatFracsSinceShot++;
            }

            if (beatFracsSinceShot == shotSelected.beatFracsPerShot)
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
