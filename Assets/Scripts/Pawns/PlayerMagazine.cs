using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player's magazine starts with a supply of basic bullets, but after that the player can only fire bullets that they are hit by or absorb!

public class PlayerMagazine : BaseMagazine
{
    [SerializeField]private int shotsStart = 20;
    [SerializeField]private int shotCapacity = 50;
    [SerializeField]private Color shotExplosionColor = Color.yellow;
    [SerializeField]private Layer shotExplosionLayer = Layer.PlayerBullet;
    private List<Shot> shots;

    void Awake()
    {
        // populate the shot list with some generic shots to start
        shots = new List<Shot>();
        for (int i = 0; i < shotsStart; i++)
        {
            shots.Add(defaultShotPrefab);
        }
    }

    void Start()
    {
        UIManager.instance.magazine.SetShots(shots.Count);
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
        if (shots.Count >= shotCapacity)
        {
            float angle = transform.eulerAngles.z;
            float angleStep = 360 / shots.Count;
            // MASSIVE BULLET EXPLOSION!!!
            for (int i = shots.Count - 1; i >= 0; i--)
            {
                Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
                Bullet bulletFired = BulletLibrary.instance.GetBullet();

                bulletFired.Shoot(transform.position, bulletRotation, shots[i], shotExplosionColor, shotExplosionLayer);

                shots.RemoveAt(i);
                angle += angleStep;
            }
        }
        UIManager.instance.magazine.SetShots(shots.Count);
    }
}
