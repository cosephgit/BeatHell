using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player's magazine starts with a supply of basic bullets, but after that the player can only fire bullets that they are hit by or absorb!

public class PlayerMagazine : BaseMagazine
{
    [SerializeField]private int shotsStart = 20;
    [SerializeField]private int shotWarning = 30; // how many shots before the player gets a warning indicator
    [SerializeField]private int shotCapacity = 50; // how many shots before the magazine explodes
    [SerializeField]private Color shotExplosionColor = Color.yellow;
    [SerializeField]private Layer shotExplosionLayer = Layer.PlayerBullet;
    [Header("Shot explosion alert")]
    [SerializeField]private AudioSource shotExposionWarningSound;
    [SerializeField]private AudioClip shotExplosionSound;
    [SerializeField]private int shotExposionWarningBeatFracs = 4; // number of beat fracs between warning flashes
    private List<Shot> shots;
    private int shotExplosionCount;


    void Awake()
    {
        // populate the shot list with some generic shots to start
        shots = new List<Shot>();
        shotExplosionCount = 0;
    }

    void Start()
    {
        for (int i = 0; i < shotsStart; i++)
        {
            shots.Add(defaultShotPrefab);
            UIManager.instance.magazine.AddShot(defaultShotPrefab);
        }
        //UIManager.instance.magazine.SetShots(shots.Count);
        BeatManager.onBeatFrac += WarningFlashBeatFrac;
    }

    public override Shot GetShot()
    {
        if (shots.Count > 0)
        {
            // take the shot at the start of the list
            Shot shotReturn = shots[0];
            shots.RemoveAt(0);
            UIManager.instance.magazine.Shoot();
            return shotReturn;
        }
        return null;
    }

    public override void AddShot(Shot shotAdd)
    {
        shots.Add(shotAdd);
        UIManager.instance.magazine.AddShot(shotAdd);

        if (shots.Count >= shotCapacity)
        {
            float angle = transform.eulerAngles.z;
            float angleStep = 360 / shots.Count;
            // MASSIVE BULLET EXPLOSION!!!
            for (int i = shots.Count - 1; i >= 0; i--)
            {
                Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
                Bullet bulletFired = BulletLibrary.instance.GetBullet();

                bulletFired.Shoot(transform.position, bulletRotation, shots[i], shotExplosionColor, shotExplosionLayer, true);
                //Debug.Log("<color=orange>WTF</color> bullet spawned at " + transform.position + " on frame " + Time.frameCount);

                shots.RemoveAt(i);

                angle += angleStep;
            }

            UIManager.instance.magazine.ClearShots();
            shotExposionWarningSound.PlayOneShot(shotExplosionSound);
        }
        //UIManager.instance.magazine.SetShots(shots.Count);
    }

    void OnDestroy()
    {
        BeatManager.onBeatFrac -= WarningFlashBeatFrac;
    }

    private void WarningFlashBeatFrac(int count)
    {
        if (shots.Count > shotWarning)
        {
            shotExplosionCount++;
            if (shotExplosionCount == shotExposionWarningBeatFracs)
            {
                shotExplosionCount = 0;
                shotExposionWarningSound.Play();
                UIManager.instance.magazine.MagazineAlert();
            }
        }
        else shotExplosionCount = 0;
    }
}
