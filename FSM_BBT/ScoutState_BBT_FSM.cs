
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoutState_BBT_FSM : BaseState_BBT_FSM
{
    private AITank smartTank;

    public ScoutState_BBT_FSM(AITank smartTank)
    {
        this.smartTank = smartTank;
    }
    public override void AIOnCollisionEnter(Collision collision)
    {
    }

    public override void AITankStart()
    {
        Debug.Log("enter scout state");
        GenerateRandomPoint();
        FollowPathToRandomPoint(1f);
        t = 0; 
    }

    public override void AITankUpdate()
    {
        Debug.Log("!!");

        //move to random point
        if (consumablesFound.Count > 0)
        {
            //path to consumable if need
        }
        else if (targetTanksFound.First().Key != null)
        {
            //attack if found
        }
        else if (basesFound.Count > 0)
        {
            //attack if near
        }
        else
        {
            targetTanksFound = null;
            consumablesFound = null;
            basesFound = null;
            FollowPathToRandomPoint(1f);
            t += Time.deltaTime;
            if (t>10)
            {
                GenerateRandomPoint();
                Debug.Log("!!");
                t = 0;
            }
        }
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
        Debug.Log("state_update");
        AITankUpdate();
        return null;
        //return typeof(AttackState_BBT_FSM);
    }
}
