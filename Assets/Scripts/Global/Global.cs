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
    // bullet values
    public const float BULLETSCALE = 0.3f;
    public const float BULLETSCALEPERDAM = 0.1f;
    public const string LAYERENEMYBULLET = "EnemyBullet";
    public const string LAYERPLAYERBULLET = "PlayerBullet";
    // difficulty settings
    public const float DIFFBASE = 1f;
    public const float DIFFMULT = 0.5f;
    public const float PLAYERBULLETSPEED = 2f;
    public const int PLAYERBULLETSTART = 20;
    public const int PLAYERBULLETWARNING = 30;
    public const int PLAYERBULLETEXPLODE = 50;
    // score settings
    public const float SCOREBASE = 1f;
    public const float SCOREDIFF = 1f; // score bonus per difficulty tier
    public const float DIFFSPAWNSCALEMIN = 0.5f; // the smallest enemy strength spawned as a multiple of current bar intensity
    public const float DIFFSPAWNSCALEMAX = 1.5f; // the largest enemy strength spawned as a multiple of current bar intensity
    public const int SAVESLOTS = 3;
    public const int SCORESLOTS = 10; // the number of high score positions
    // save keys - must be unique
    public const string SAVEVOLUME = "Volume";
    public const string SAVEEPILEPSY = "Epilepsy";
    public const string SAVESTAGE = "Stage";
    public const string SAVEDIFFICULTY = "Difficulty";
    public const string SAVESCORE = "Score";
    public const string SAVEHISCORE = "HiScore";
    public const string DIFFNAME0 = "CASUAL (125BPM)";
    public const string DIFFNAME1 = "INTENSE (150BPM)";
    public const string DIFFNAME2 = "HYPER (190BPM)";

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

    public static string ScoreName(int i)
    {
        if (i == 0) return DIFFNAME0;
        else if (i == 1) return DIFFNAME1;
        else return DIFFNAME2;
    }
}
