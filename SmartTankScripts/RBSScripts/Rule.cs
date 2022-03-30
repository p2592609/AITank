using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;    
public class Rule
{
    public string atecedentA;
    public string atecedentB;
    public bool consequentState;
    public Predicate compare;
    public enum Predicate {  And, Or, nAnd, nB}

    public Rule(string atecedentA, string atecedentB, bool consequentState, Predicate compare)
    {
        this.atecedentA = atecedentA;
        this.atecedentB = atecedentB;
        this.consequentState = consequentState;
        this.compare = compare;
    }

    public bool CheckRule(Dictionary<string, bool> stats)
    {
        bool atecedentABool = stats[atecedentA];
        bool atecedentBBool = stats[atecedentB];
        
        switch (compare)
        {
            case Predicate.And:
                if(atecedentABool && atecedentBBool)
                {
                    consequentState = true;
                    return consequentState;
                }
                else
                {
                    consequentState = false;
                    return consequentState;
                }

            case Predicate.Or:

                if (atecedentABool || atecedentBBool)
                {
                    consequentState = true;
                    return consequentState;
                }
                else
                {
                    consequentState = false;
                    return consequentState;
                }


            case Predicate.nAnd:

                if (!atecedentABool && !atecedentBBool)
                {
                    consequentState = true;
                    return consequentState;
                }
                else
                {
                    consequentState = false;
                    return consequentState;
                }

            case Predicate.nB:

                if (atecedentABool && !atecedentBBool)
                {
                    consequentState = true;
                    return consequentState;
                }
                else
                {
                    consequentState = false;
                    return consequentState;
                }

            default:

                return false;

        }
    }
    
}
