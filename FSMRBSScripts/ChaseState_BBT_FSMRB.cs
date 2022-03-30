using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState_BBT_FSMRB : BaseState_BBT_FSMRB
{
    private SmartTank_BBT_FSMRB tank;

    public ChaseState_BBT_FSMRB(SmartTank_BBT_FSMRB tank)
    {
        this.tank = tank;
    }

    public override Type StateEnter()
    {
        tank.stats["chaseState"] = true;
        return null;
    }

    public override Type StateExit()
    {
        tank.stats["chaseState"] = false;
        return null;
    }

    public override Type StateUpdate()
    {
        tank.Chase();
        foreach (var item in tank.rules.GetRules)
        {
            if (item.CheckRule(tank.stats) != null)
            {
                return item.CheckRule(tank.stats);

            }

        }
        return null;
    }
}
