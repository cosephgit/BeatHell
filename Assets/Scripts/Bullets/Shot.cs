using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class contains the actual parameters of each launched bullet
// each ShotDetails contains at least one of these

public class Shot : MonoBehaviour
{
    [field: SerializeField]public int shotDamage { get; private set; } = 1;
    [field: SerializeField]public float shotSpeed { get; private set; } = 10f;
    [field: SerializeField]public float shotLife { get; private set; } = 1f;
}
