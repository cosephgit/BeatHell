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
    public const float BULLETSCALE = 0.3f;
    public const float BULLETSCALEPERDAM = 0.1f;
    private const string LAYERENEMYBULLET = "EnemyBullet";
    public const float DIFFBASE = 1f;
    public const float DIFFMULT = 0.5f;
    public const float PLAYERBULLETSPEED = 2f;
    public const float SCOREBASE = 1f;
    public const float SCOREDIFF = 1f; // score bonus per difficulty tier
    public const float DIFFSPAWNSCALEMIN = 0.5f; // the smallest enemy strength spawned as a multiple of current bar intensity
    public const float DIFFSPAWNSCALEMAX = 1.5f; // the largest enemy strength spawned as a multiple of current bar intensity
    public const int SAVESLOTS = 3;
    // save keys - must be unique
    public const string SAVEVOLUME = "Volume";
    public const string SAVEEPILEPSY = "Epilepsy";
    public const string SAVESTAGE = "Stage";
    public const string SAVEDIFFICULTY = "Difficulty";
    public const string SAVESCORE = "Score";

    public static LayerMask LayerEnemyBullet()
    {
        return LayerMask.GetMask(LAYERENEMYBULLET);
    }

    public static Vector2 Vector2Clamp(Vector2 initial, Rect bounds)
    {
        Vector2 result = initial;
        result.x = Mathf.Clamp(result.x, bounds.xMin, bounds.xMax);
        result.y = Mathf.Clamp(result.y, bounds.yMin, bounds.yMax);
        return result;
    }
}
