using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules_BBT_FSMRBBT
{
    public void AddRule(Rule_BBT_FSMRBBT rule)
    {
        GetRules.Add(rule);
    }

    public List<Rule_BBT_FSMRBBT> GetRules { get; } = new List<Rule_BBT_FSMRBBT>();
}
