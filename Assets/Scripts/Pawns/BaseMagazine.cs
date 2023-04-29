using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is the magazine for a pawn's weapon
// each magazine has a type of Shot that it contains
// the magazine is contained by a ShotDetails
// the magazine MUST BE INSTANTIATED IN THE OWNING PAWN

public class BaseMagazine : MonoBehaviour
{
    [SerializeField]protected Shot defaultShotPrefab;

    // the basic magazine just has one unlimited bullet type that it returns every time it's checked
    // for child classes for magazines with limited ammo, this method should remove the returned shot from the magazine
    public virtual Shot GetShot()
    {
        return defaultShotPrefab;
    }

    public virtual void AddShot(Shot shotAdd)
    {
        // base class doesn't use this
    }
}
