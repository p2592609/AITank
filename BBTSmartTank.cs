using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BBTSmartTank : AITank
{
    public Dictionary<GameObject, float> targetTanksFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> consumablesFound = new Dictionary<GameObject, float>();
    public Dictionary<GameObject, float> basesFound = new Dictionary<GameObject, float>();

    public GameObject targetTankPosition;
    public GameObject consumablePosition;
    public GameObject basePosition;

    float t;

    public Dictionary<string, bool> stats = new Dictionary<string, bool>();
    public BBTRules rules = new BBTRules();
    public bool lowHealth;
    public bool lowAmmo;
    public bool lowFuel;

    public bool isAttacking = false;
    public bool isSearching = false;
    public bool fleeAmmo = false;
    public bool fleeHealth = false;
    public bool fleeFuel = false;
    public bool isChasing = false;


    /*******************************************************************************************************      
    WARNING, do not include void Start(), use AITankStart() instead if you want to use Start method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankStart()
    {
        stats.Add("lowHealth", lowHealth);
        stats.Add("lowAmmo", lowAmmo);
        stats.Add("lowFuel", lowFuel);
        stats.Add("targetSpotted", false);
        stats.Add("targetReached", false);
        stats.Add("fleeState", false);
        stats.Add("chaseState", false);
        stats.Add("searchState", false);
        stats.Add("attackState", false);

  

        rules.AddRule(new Rule("attackState", "lowHealth", fleeHealth, Rule.Predicate.And));
        rules.AddRule(new Rule("attackState", "lowAmmo", fleeAmmo, Rule.Predicate.And));
        rules.AddRule(new Rule("attackState", "lowfuel", fleeFuel, Rule.Predicate.And));
        rules.AddRule(new Rule("searchState", "targetSpotted", isChasing, Rule.Predicate.And));
        rules.AddRule(new Rule("searchState", "targetSpotted", isSearching, Rule.Predicate.nAnd));
        rules.AddRule(new Rule("targetSpotted", "targetReached", isAttacking, Rule.Predicate.And));

        Application.targetFrameRate = 60;
    }

    /*******************************************************************************************************       
    WARNING, do not include void Update(), use AITankUpdate() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AITankUpdate()
    {

        

        targetTanksFound = GetAllTargetTanksFound;
        consumablesFound = GetAllConsumablesFound;
        basesFound = GetAllBasesFound;

        //foreach (var item in rules.GetRules)
        //{
        //    if (item.CheckRule(stats) != null)
        //    {
        //        item.consequentState = item.CheckRule(stats);

        //    }

        //}
        

        if (isAttacking)
        {
            stats["attackState"] = true;
            Attack();
        }
        else
        {
            stats["attackState"] = false;
        }

        if(fleeAmmo)
        {
            FleeGetAmmo();
        }
        

        if (fleeHealth)
        {
            FleeGetHealth();
        }

        if (fleeFuel)
        {
            FleeGetFuel();
        }

        if (isChasing)
        {
           
            Chase();
            stats["chaseState"] = true;
        }
        else
        {
            stats["chaseState"] = false;
        }

        if(isSearching)
        {
            stats["searchState"] = true;
            Search();
            
        }
        else
        {
            stats["searchState"] = false;
        }
        

        
        if(GetHealthLevel < 50)
        {
            lowHealth = true;
        }

        if (GetAmmoLevel < 5)
        {
            lowAmmo = true;
        }

        if(GetFuelLevel <= 30f)
        {
            lowFuel = true;
        }

        CheckTargetSpotted();
        CheckTargetReached();

        List<Rule> RuleList = rules.GetRules;
        if (RuleList[0].CheckRule(stats) != null)
        {
            fleeHealth = RuleList[0].CheckRule(stats);
        }
        if (RuleList[1].CheckRule(stats) != null)
        {
            fleeAmmo = RuleList[1].CheckRule(stats);
        }
        if (RuleList[2].CheckRule(stats) != null)
        {
            isChasing = RuleList[2].CheckRule(stats);
        }
        if (RuleList[3].CheckRule(stats) != null)
        {
            isSearching = RuleList[3].CheckRule(stats);
        }
        if (RuleList[4].CheckRule(stats) != null)
        {
            isAttacking = RuleList[4].CheckRule(stats);
        }
        if (RuleList[5].CheckRule(stats) != null)
        {
            fleeFuel = RuleList[5].CheckRule(stats);
        }
    }

    /*******************************************************************************************************       
    WARNING, do not include void OnCollisionEnter(), use AIOnCollisionEnter() instead if you want to use Update method from Monobehaviour.
    *******************************************************************************************************/
    public override void AIOnCollisionEnter(Collision collision)
    {
    }

    public void FleeGetHealth()
    {
        
        if(consumablesFound.Count > 0)
        {
            for(int i = 0; i < consumablesFound.Count() - 1; i++)
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
    }

    public void Chase()
    {
        if (targetTankPosition != null)
        {

            // chase while states are all OK and target isnt too far to chase
            while (!lowFuel && !lowHealth && !lowAmmo && Vector3.Distance(transform.position, targetTankPosition.transform.position) < 40f);
            {
                // if high fuel levels, increase speed to take advantage of resource abundance
                if (GetFuelLevel >= 80f)
                {
                    FollowPathToPoint(targetTankPosition, 1.5f);
                }
                // otherwise chase at base speed
                else if (!lowFuel)
                {
                    FollowPathToPoint(targetTankPosition, 1f);
                }
                else
                {
                    FleeGetFuel();
                }
            }

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

    public void FleeGetFuel()
    {
        if (consumablesFound.Count > 0)
        {
            for (int i = 0; i < consumablesFound.Count() - 1; i++)
            {
                if (consumablesFound.Keys.ElementAt(i).tag == "Fuel")
                {

                    consumablePosition = consumablesFound.Keys.ElementAt(i);
                }

            }
            FollowPathToPoint(consumablePosition, 1.5f);
        }
        else
        {

            consumablePosition = null;

            FollowPathToRandomPoint(0.5f);
        }
    }
}
