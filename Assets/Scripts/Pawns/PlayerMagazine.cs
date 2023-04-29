using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player's magazine starts with a supply of basic bullets, but after that the player can only fire bullets that they are hit by or absorb!

public class PlayerMagazine : BaseMagazine
{
    [SerializeField]private int shotsStart = 20;
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
        UIManager.instance.magazine.SetShots(shots.Count);
    }
}
