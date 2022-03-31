using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState_BBT_FSMRBBT : BaseState_BBT_FSMRBBT
{
    private SmartTank_BBT_FSMRBBT tank;

    public FleeState_BBT_FSMRBBT(SmartTank_BBT_FSMRBBT tank)
    {
        this.tank = tank;
    }

    public override Type StateEnter()
    {
        tank.stats["fleeState"] = true;
        return null;
    }

    public override Type StateExit()
    {
        tank.stats["fleeState"] = false;
        return null;
    }

    public override Type StateUpdate()
    {
        tank.Flee();
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
