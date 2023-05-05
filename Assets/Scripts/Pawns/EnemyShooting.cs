using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : BaseShooting
{
    public void SetWeapon(float weaponKey)
    {
        InitWeapon(weaponKey);
        shooting = true;
    }
}
