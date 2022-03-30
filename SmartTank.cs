using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartTank : AITank
{
    
    /*******************************************************************************************************      
    WARNING, do not include void Start(), use AITankStart() instead if you want to use Start method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankStart()
    {
        InitializeStateMachine();
    }

    /*******************************************************************************************************       
    WARNING, do not include void Update(), use AITankUpdate() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankUpdate()
    {

    }

    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision)
    {
        //This method is used for detecting collisions (unlikley you will need this).
    }

    private void InitializeStateMachine()
    {
        Debug.Log("initialise state machine");

        Dictionary<Type, BaseState_BBT_FSM> states = new Dictionary<Type, BaseState_BBT_FSM>();

        states.Add(typeof(ScoutState_BBT_FSM), new ScoutState_BBT_FSM(this));
        states.Add(typeof(AttackState_BBT_FSM), new AttackState_BBT_FSM(this));
        states.Add(typeof(FleeState_BBT_FSM), new FleeState_BBT_FSM(this));

        GetComponent<StateMachine_BBT_FSM>().setStates(states);
    }
}
