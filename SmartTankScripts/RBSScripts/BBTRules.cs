using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBTRules
{
    public void AddRule(Rule rule)
    {
        GetRules.Add(rule);
    }

    public List<Rule> GetRules { get; } = new List<Rule>();
}

