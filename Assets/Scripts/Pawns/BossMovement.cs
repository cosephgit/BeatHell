using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    [Header("Note boss movement is ABSOLUTE not RELATIVE like normal enemies")]
    [SerializeField]private BaseStrategy[] strategySteps; // each stage of boss strategy

    protected override void Start()
    {
        base.Start();
        strategy = strategySteps[0];
    }

    protected override bool MoveAbsolute()
    {
        return true;
    }
}
