using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine_BBT_FSM : MonoBehaviour
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

    void Update()
    {
        if (CurrentState == null)
        {
            CurrentState = states.Values.First();
            CurrentState.StateEnter();
        }
        else
        {
            var nextState = CurrentState.StateUpdate();
            if (nextState != null && nextState != CurrentState.GetType())
            {
                SwitchToState(nextState);
            }
        }
    }

    void SwitchToState(Type nextState)
    {
        Debug.Log("switch");
        CurrentState.StateExit();
        CurrentState = states[nextState];
        CurrentState.StateEnter();
    }




    
}
