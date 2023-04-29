using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used as a structure for shot data, so it can be standardised and transferred

[System.Serializable]
public class ShotDetails : MonoBehaviour
{
    [field: Header("Fire rate and timing")]
    [field: SerializeField]public int beatFracsOffset { get; private set; } = 0; // the innitial beat fractions offset, so shots can be set up to be asynchronous
    [field: SerializeField]public int beatFracsPerShot { get; private set; } = 1; // the number of beat fractions between shots
    [field: Header("Shot pattern")]
    [field: SerializeField]public int shotCount { get; private set; } = 1; // number of projectiles to fire each shot
    [field: SerializeField]public float cone { get; private set; } = 0; // cone which shots are spread over, centred around the forward direction
    [field: Header("Projectile type")]
    [field: SerializeField]public int shotDamage { get; private set; } = 1;
    [field: SerializeField]public Color shotColor { get; private set; } = Color.yellow;
    [field: SerializeField]public float shotSpeed { get; private set; } = 10f;
    [field: SerializeField]public float shotLife { get; private set; } = 1f;
    [field: SerializeField]public Layer layer { get; private set; }
}
