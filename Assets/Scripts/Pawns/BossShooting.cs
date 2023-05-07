using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooting : BaseShooting
{
    protected override void Start()
    {
        shooting = true;
        InitWeapon(0);
        base.Start();
    }
}
