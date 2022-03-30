using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules_BBT_FSMRB
{
    public void AddRule(Rule_BBT_FSMRB rule)
    {
        GetRules.Add(rule);
    }

    public List<Rule_BBT_FSMRB> GetRules { get; } = new List<Rule_BBT_FSMRB>();
}

