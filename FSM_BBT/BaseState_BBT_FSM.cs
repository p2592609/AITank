using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseState_BBT_FSM : AITank
{
    public abstract Type StateUpdate();
    public abstract Type StateEnter();
    public abstract Type StateExit();

    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    public float t;
}

