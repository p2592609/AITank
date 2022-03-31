using System;
using UnityEngine;

public class AttackState_BBT_FSM : BaseState_BBT_FSM
{
    private SmartTank_BBT_FSM smartTank;
    private float time;
    public AttackState_BBT_FSM(SmartTank_BBT_FSM smartTank)
    {
        this.smartTank = smartTank;
    }
   
    public override Type StateEnter()
    {
        time = 0;
        return null;
    }

    public override Type StateExit()
    {
        time = 0;
        return null;
    }

    public override Type StateUpdate()
    {
        smartTank.Attack();
        time += Time.deltaTime;

        if (time > 1f)
        {
            if (smartTank.lowHealth == true)
            {
                return typeof(FleeState_BBT_FSM);
            }

           

            if (smartTank.CheckTargetReached())
            {
                return typeof(AttackState_BBT_FSM);
            }
            else
            {
                time = 0;
                return typeof(ScoutState_BBT_FSM);

            }
        }


        return null;
    }
}
