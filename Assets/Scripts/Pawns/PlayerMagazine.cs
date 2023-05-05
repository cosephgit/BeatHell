using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player's magazine starts with a supply of basic bullets, but after that the player can only fire bullets that they are hit by or absorb!

public class PlayerMagazine : BaseMagazine
{
    [SerializeField]private Color shotExplosionColor = Color.yellow;
    [SerializeField]private Layer shotExplosionLayer = Layer.PlayerBullet;
    [Header("Shot explosion alert")]
    [SerializeField]private AudioSource shotExplosionSound;
    private List<Shot> shots;

    private void Start()
    {
        shots = new List<Shot>();
        for (int i = 0; i < Global.PLAYERBULLETSTART; i++)
        {
            shots.Add(defaultShotPrefab);
            UIManager.instance.magazine.AddShot(defaultShotPrefab);
        }
        //UIManager.instance.magazine.SetShots(shots.Count);
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

    public override bool Empty()
    {
        return (shots.Count == 0);
    }

    public override void AddShot(Shot shotAdd)
    {
        shots.Add(shotAdd);
        UIManager.instance.magazine.AddShot(shotAdd);

        if (shots.Count >= Global.PLAYERBULLETEXPLODE)
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
            shotExplosionSound.Play();
        }
        //UIManager.instance.magazine.SetShots(shots.Count);
    }
}
