using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseState_BBT_FSM 
{
    public abstract Type StateUpdate();
    public abstract Type StateEnter();
    public abstract Type StateExit();

 
}
