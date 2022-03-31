using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState_BBT_FSM : BaseState_BBT_FSM
{
    private SmartTank_BBT_FSM tank;

    public ChaseState_BBT_FSM(SmartTank_BBT_FSM tank)
    {
        this.tank = tank;
    }

    public override Type StateEnter()
    {
        
        return null;
    }

    public override Type StateExit()
    {
        
        return null;
    }

    public override Type StateUpdate()
    {
        tank.Chase();
        if (tank.CheckTargetReached())
        {
            return typeof(AttackState_BBT_FSM);
        }
        return null;
    }
}