using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine_BBT_FSMRBBT : MonoBehaviour
{
    private Dictionary<Type, BaseState_BBT_FSMRBBT> states;
    public BaseState_BBT_FSMRBBT currentState;
    public BaseState_BBT_FSMRBBT CurrentState
    {
        get
        {
            return currentState;
        }
        private set
        {
            currentState = value;
        }
    }

    public void SetStates(Dictionary<Type, BaseState_BBT_FSMRBBT> states)
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
        CurrentState.StateExit();
        CurrentState = states[nextState];
        CurrentState.StateEnter();
    }
}
