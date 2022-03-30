using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine_BBT_FSM : AITank
{
    private Dictionary<Type, BaseState_BBT_FSM> states;
    private BaseState_BBT_FSM currentState;

    public BaseState_BBT_FSM CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }

    public void setStates(Dictionary<Type, BaseState_BBT_FSM> states)
    {
        this.states = states;
    }

    void SwitchToState(Type nextState)
    {
        Debug.Log("switch");
        CurrentState.StateExit();
        CurrentState = states[nextState];
        CurrentState.StateEnter();
    }

    public override void AIOnCollisionEnter(Collision collision)
    {
    }

    public override void AITankStart()
    {
        Debug.Log("state machine start");
        InitializeStateMachine();
        SwitchToState(typeof(ScoutState_BBT_FSM));
    }

    public override void AITankUpdate()
    {
        Debug.Log(states.Values.First());
        if (CurrentState == null)
        {
            CurrentState = states.Values.First();
            SwitchToState(typeof(ScoutState_BBT_FSM));
            Debug.Log("State is null");
        }
        else
        {
            var nextState = CurrentState.StateUpdate();

            // only switch state if next state is not null or same as current state
            if (nextState != null && nextState == CurrentState.GetType())
            {
                SwitchToState(nextState);
            }
        }



    }


    private void InitializeStateMachine()
    {
        Debug.Log("initialise state machine");

        Dictionary<Type, BaseState_BBT_FSM> states = new Dictionary<Type, BaseState_BBT_FSM>();

        states.Add(typeof(ScoutState_BBT_FSM), value: new ScoutState_BBT_FSM(this));
        states.Add(typeof(AttackState_BBT_FSM), value: new AttackState_BBT_FSM(this));
        states.Add(typeof(FleeState_BBT_FSM), value: new FleeState_BBT_FSM(this));

        GetComponent<StateMachine_BBT_FSM>().setStates(states);
    }
}
