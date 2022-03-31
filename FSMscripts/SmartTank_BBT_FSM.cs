using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartTank_BBT_FSM : AITank
{
    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    float t;


    public bool lowHealth;
    public bool lowAmmo;

   


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



        targetTanksFound = GetAllTargetTanksFound;
        consumablesFound = GetAllConsumablesFound;
        basesFound = GetAllBasesFound;




        if (GetHealthLevel < 50)
        {
            lowHealth = true;
        }

        if (GetAmmoLevel < 5)
        {
            lowAmmo = true;
        }



        CheckTargetSpotted();
        CheckTargetReached();
    }

    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision)
    {
    }

    private void InitializeStateMachine()
    {
        Dictionary<Type, BaseState_BBT_FSM> states = new Dictionary<Type, BaseState_BBT_FSM>();

        //states.Add(typeof(FleeAmmoState_BBT_FSMRBBT), new FleeAmmoState_BBT_FSMRBBT(this));
        states.Add(typeof(FleeState_BBT_FSM), new FleeState_BBT_FSM(this));
        states.Add(typeof(ScoutState_BBT_FSM), new ScoutState_BBT_FSM(this));
        states.Add(typeof(ChaseState_BBT_FSM), new ChaseState_BBT_FSM(this));
        states.Add(typeof(AttackState_BBT_FSM), new AttackState_BBT_FSM(this));

        GetComponent<StateMachine_BBT_FSM>().setStates(states);
    }


    public void Flee()
    {
        if (consumablesFound.Count > 0)
        {
            consumablePosition = consumablesFound.First().Key;
            FollowPathToPoint(consumablePosition, 1f);
            t += Time.deltaTime;
            if (t > 10)
            {
                GenerateRandomPoint();
                t = 0;
            }
        }
        else if (CheckTargetSpotted())
        {
            GenerateRandomPoint();
            FollowPathToRandomPoint(1f);
        }
        else
        {

            consumablePosition = null;
            basePosition = null;
            FollowPathToRandomPoint(1f);
        }
        
    }

    public bool CheckTargetSpotted()
    {
        if (targetTanksFound.Count > 0 && targetTanksFound.First().Key != null)
        {
            targetTankPosition = targetTanksFound.First().Key;
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool CheckTargetReached()
    {
        if (CheckTargetSpotted())
        {
            if (Vector3.Distance(transform.position, targetTankPosition.transform.position) < 25f)
            {
                targetTankPosition = targetTanksFound.First().Key;
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }

    public void Attack()
    {
        FireAtPoint(targetTankPosition);

        
    }

    public void Chase()
    {
        if (targetTankPosition != null)
        {



            FollowPathToPoint(targetTankPosition, 1f);

        }
        
    }

    public void Search()
    {
        if (consumablesFound.Count > 0)
        {
            consumablePosition = consumablesFound.First().Key;
            FollowPathToPoint(consumablePosition, 1f);
            t += Time.deltaTime;
            if (t > 10)
            {
                GenerateRandomPoint();
                t = 0;
            }
        }
        else if (basesFound.Count > 0)
        {
            //if base if found
            basePosition = basesFound.First().Key;
            if (basePosition != null)
            {
                //go close to it and fire
                if (Vector3.Distance(transform.position, basePosition.transform.position) < 25f)
                {
                    FireAtPoint(basePosition);
                }
                else
                {
                    FollowPathToPoint(basePosition, 1f);
                }
            }
        }
        else
        {

            consumablePosition = null;
            basePosition = null;
            FollowPathToRandomPoint(1f);
        }
       
    }

    bool GetLowHealth()
    {
        return lowHealth;
    }

    bool GetLowAmmo()
    {
        return lowAmmo;
    }
}

