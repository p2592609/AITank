using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_BBT_FSMRBBT : BaseState_BBT_FSMRBBT
{
    private SmartTank_BBT_FSMRBBT tank;
    float time = 0;
    public AttackState_BBT_FSMRBBT(SmartTank_BBT_FSMRBBT tank)
    {
        this.tank = tank;
    }

    public override Type StateEnter()
    {
        tank.stats["attackState"] = true;
        return null;
    }

    public override Type StateExit()
    {
        tank.stats["attackState"] = false;
        time = 0;
        return null;
    }

    public override Type StateUpdate()
    {
        tank.Attack();
        time += Time.deltaTime;

        if (time > 1f)
        {
            if (tank.stats["lowHealth"] == true)
            {
                return typeof(FleeState_BBT_FSMRBBT);
            }

            if (tank.stats["lowAmmo"] == true)
            {
                return typeof(FleeState_BBT_FSMRBBT);
            }

            if (tank.stats["targetReached"] == true)
            {
                return typeof(AttackState_BBT_FSMRBBT);
            }
            else
            {
                return typeof(SearchState_BBT_FSMRBBT);

            }
        }

        return null;
    }
}
