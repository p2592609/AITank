
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoutState_BBT_FSM : BaseState_BBT_FSM
{
    private SmartTank_BBT_FSM smartTank;

    public ScoutState_BBT_FSM(SmartTank_BBT_FSM smartTank)
    {
        this.smartTank = smartTank;
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
        smartTank.Search();
        if (smartTank.CheckTargetSpotted())
        {
            return typeof(ChaseState_BBT_FSM);
        }
        else
        {
            smartTank.Search();
            return null;
        }

        //return typeof(AttackState_BBT_FSM);
    }
}