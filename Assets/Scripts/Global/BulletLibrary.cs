using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stores all the bullets in the level and enables/disables them as required

public class BulletLibrary : MonoBehaviour
{
    public static BulletLibrary instance;
    [SerializeField]private Bullet bulletPrefab;
    [SerializeField]private int bulletCount = 1000; // how many bullets should exist
    private List<Bullet> bullets;

    void Awake()
    {
        if (instance)
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
            instance = this;

        bullets = new List<Bullet>();
        // set up the bullet library
        for (int i = 0; i < bulletCount; i++)
        {
            Bullet bulletNew = Instantiate(bulletPrefab, transform.position, transform.rotation);
            ReturnBullet(bulletNew);
        }
    }

    // check out a bullet from the bullet library
    public Bullet GetBullet()
    {
        if (bullets.Count > 0)
        {
            Bullet pickBullet = bullets[0];
            bullets.Remove(pickBullet);
            return pickBullet;
        }
        else
        {
            // OOPS, spawn another prefab
            Bullet bulletNew = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Debug.LogError("BulletLibrary had to instantiate an extra bullet");
            return bulletNew;
        }
    }

    public void ReturnBullet(Bullet bulletAdd)
    {
        if (!bullets.Contains(bulletAdd))
        {
            bullets.Add(bulletAdd);
        }
        bulletAdd.Clear();
        bulletAdd.transform.parent = transform; // make sure it doesn't get destroyed with something else
        bulletAdd.gameObject.SetActive(false);
    }
}
