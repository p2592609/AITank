using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules_BBT_RB
{
    public void AddRule(Rule_BBT_RB rule)
    {
        GetRules.Add(rule);
    }

    public List<Rule_BBT_RB> GetRules { get; } = new List<Rule_BBT_RB>();
}

