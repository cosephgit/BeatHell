using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    [Header("Note boss movement is ABSOLUTE not RELATIVE like normal enemies")]
    [SerializeField]private BaseStrategy[] strategySteps; // each stage of boss strategy
    [SerializeField]private BaseCollisions bossCollision; // the actual destructible part
    [SerializeField]private EnemyShooting coreTurret; // a turret that is central to the main body and not indepednently destructible
    [SerializeField]private SpriteRenderer coreTurretSprite; // the core turret isn't destructible, but we want to match the color
    [SerializeField]private float coreTurretSpin = 360f; // spin per beat frac
    [SerializeField]private Color colorInvuln = Color.blue;
    [SerializeField]private Color colorVuln = Color.magenta;
    [SerializeField]private BaseCollisions[] subComponents; // these sub components must be destroyed before this component becomes destructible
    [SerializeField]private EnemyShooting[] subTurrets; // the shooting component of the sub component - MUST SYNC UP WITH ABOVE
    private float coreTurretSpinFrame;

    protected override void Awake()
    {
        base.Awake();
        coreTurret.SetWeapon(0);
        coreTurretSprite.color = colorInvuln;
        for (int i = 0; i < subTurrets.Length; i++)
        {
            subTurrets[i].SetWeapon(0);
        }
        bossCollision.SetVulnerable(false, colorInvuln);
    }

    protected override void Start()
    {
        base.Start();
        SetStrategy(strategySteps[0]);
        coreTurretSpinFrame = coreTurretSpin / BeatManager.beatFracFrames;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (strategy)
        {
            bool turretActive = strategy.Shooting(moveStep);

            // core turret operates on the INVERSE of the main gun
            coreTurret.SetShooting(!turretActive);
            if (!turretActive)
                coreTurret.transform.Rotate(0, 0, coreTurretSpinFrame);

            // regular turrets operate as normal
            for (int i = 0; i < subTurrets.Length; i++)
            {
                subTurrets[i].SetShooting(turretActive);
            }
        }
    }

    protected override bool MoveAbsolute()
    {
        return true;
    }
}
