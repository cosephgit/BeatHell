using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// layer index references, used fo assigning bullets to layers
public enum Layer
{
    PlayerBullet = 10,
    EnemyBullet = 11,
    PlayerPawn = 12,
    EnemyPawn = 13
}

public static class Global
{
    public const float BULLETSCALE = 0.2f;
    public const float BULLETSCALEPERDAM = 0.05f;
    private const string LAYERENEMYBULLET = "EnemyBullet";
    // save keys - must be unique
    public const string SAVEVOLUME = "Volume";
    public const string SAVEEPILEPSY = "Epilepsy";

    public static LayerMask LayerEnemyBullet()
    {
        return LayerMask.GetMask(LAYERENEMYBULLET);
    }
}
