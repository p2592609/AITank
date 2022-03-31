
using System;
using UnityEngine;

public class FleeState_BBT_FSM : BaseState_BBT_FSM
{
    private SmartTank_BBT_FSM smartTank;

    public FleeState_BBT_FSM(SmartTank_BBT_FSM smartTank)
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
        smartTank.Flee();
        if(!smartTank.lowHealth )
        {
            return typeof(ScoutState_BBT_FSM);
        }

        return null;
    }
}
