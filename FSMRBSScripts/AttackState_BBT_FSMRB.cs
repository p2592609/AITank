using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_BBT_FSMRB : BaseState_BBT_FSMRB
{
    private SmartTank_BBT_FSMRB tank;
    float time = 0;
    public AttackState_BBT_FSMRB(SmartTank_BBT_FSMRB tank)
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
                return typeof(FleeHealthState_BBT_FSMRB);
            }

            if (tank.stats["lowAmmo"] == true)
            {
                return typeof(FleeAmmoState_BBT_FSMRB);
            }

            if (tank.stats["targetReached"] == true)
            {
                return typeof(AttackState_BBT_FSMRB);
            }
            else
            {
                return typeof(SearchState_BBT_FSMRB);

            }
        }

        return null;
    }
}


