
using System;
using UnityEngine;

public class AttackState_BBT_FSM : BaseState_BBT_FSM
{
    private AITank smartTank;

    public AttackState_BBT_FSM(AITank smartTank)
    {
        this.smartTank = smartTank;
    }
    public override void AIOnCollisionEnter(Collision collision)
    {
    }

    public override void AITankStart()
    {
    }

    public override void AITankUpdate()
    {
        Debug.Log("!");
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
        return null;
    }
}
