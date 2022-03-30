using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartTank_BBT_FSMRB : AITank
{
    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    float t;

    public Dictionary<string, bool> stats = new Dictionary<string, bool>();
    public Rules_BBT_FSMRB rules = new Rules_BBT_FSMRB();
    public bool lowHealth;
    public bool lowAmmo;

    public bool isAttacking = false;
    public bool isSearching = false;
    public bool fleeAmmo = false;
    public bool fleeHealth = false;
    public bool isChasing = false;


    /*******************************************************************************************************      
    WARNING, do not include void Start(), use AITankStart() instead if you want to use Start method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankStart()
    {
        stats.Add("lowHealth", lowHealth);
        stats.Add("lowAmmo", lowAmmo);
        stats.Add("targetSpotted", false);
        stats.Add("targetReached", false);
        stats.Add("fleeState", false);
        stats.Add("chaseState", false);
        stats.Add("searchState", false);
        stats.Add("attackState", false);



        rules.AddRule(new Rule_BBT_FSMRB("attackState", "lowHealth", typeof(FleeHealthState_BBT_FSMRB), Rule_BBT_FSMRB.Predicate.And));
        rules.AddRule(new Rule_BBT_FSMRB("attackState", "lowAmmo", typeof(FleeAmmoState_BBT_FSMRB), Rule_BBT_FSMRB.Predicate.And));
        rules.AddRule(new Rule_BBT_FSMRB("searchState", "targetSpotted", typeof(ChaseState_BBT_FSMRB), Rule_BBT_FSMRB.Predicate.And));
        rules.AddRule(new Rule_BBT_FSMRB("searchState", "targetSpotted", typeof(SearchState_BBT_FSMRB), Rule_BBT_FSMRB.Predicate.nAnd));
        rules.AddRule(new Rule_BBT_FSMRB("chaseState", "targetReached", typeof(AttackState_BBT_FSMRB), Rule_BBT_FSMRB.Predicate.And));


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

        

        
    }

    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision)
    {
    }

    private void InitializeStateMachine()
    {
        Dictionary<Type, BaseState_BBT_FSMRB> states = new Dictionary<Type, BaseState_BBT_FSMRB>();

        states.Add(typeof(FleeAmmoState_BBT_FSMRB), new FleeAmmoState_BBT_FSMRB(this));
        states.Add(typeof(FleeHealthState_BBT_FSMRB), new FleeHealthState_BBT_FSMRB(this));
        states.Add(typeof(SearchState_BBT_FSMRB), new SearchState_BBT_FSMRB(this));
        states.Add(typeof(ChaseState_BBT_FSMRB), new ChaseState_BBT_FSMRB(this));
        states.Add(typeof(AttackState_BBT_FSMRB), new AttackState_BBT_FSMRB(this));

        GetComponent<StateMachine_BBT_FSMRB>().SetStates(states);
    }

    public void FleeGetHealth()
    {

        if (consumablesFound.Count > 0)
        {
            for (int i = 0; i < consumablesFound.Count() - 1; i++)
            {
                if (consumablesFound.Keys.ElementAt(i).tag == "Health")
                {

                    consumablePosition = consumablesFound.Keys.ElementAt(i);
                }

            }
            FollowPathToPoint(consumablePosition, 1f);
        }
        else
        {

            consumablePosition = null;

            FollowPathToRandomPoint(1f);
        }

        CheckTargetSpotted();
        CheckTargetReached();
    }

    public void FleeGetAmmo()
    {

        if (consumablesFound.Count > 0)
        {
            for (int i = 0; i < consumablesFound.Count() - 1; i++)
            {
                if (consumablesFound.Keys.ElementAt(i).tag == "Ammo")
                {

                    consumablePosition = consumablesFound.Keys.ElementAt(i);
                }

            }
            FollowPathToPoint(consumablePosition, 1f);
        }
        else
        {

            consumablePosition = null;

            FollowPathToRandomPoint(1f);
        }

        CheckTargetSpotted();
        CheckTargetReached();
    }

    public void CheckTargetSpotted()
    {
        if (targetTanksFound.Count > 0 && targetTanksFound.First().Key != null)
        {
            targetTankPosition = targetTanksFound.First().Key;
            stats["targetSpotted"] = true;
        }
        else
        {
            stats["targetSpotted"] = false;
        }

    }

    public void CheckTargetReached()
    {
        if (stats["targetSpotted"] == true)
        {
            if (Vector3.Distance(transform.position, targetTankPosition.transform.position) < 25f)
            {
                stats["targetReached"] = true;
            }
            else
            {
                stats["targetReached"] = false;
            }
        }
    }

    public void Attack()
    {
        FireAtPoint(targetTankPosition);

        CheckTargetSpotted();
        CheckTargetReached();
    }

    public void Chase()
    {
        if (targetTankPosition != null)
        {



            FollowPathToPoint(targetTankPosition, 1f);

        }
        CheckTargetSpotted();
        CheckTargetReached();
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
        CheckTargetSpotted();
        CheckTargetReached();
    }

}
