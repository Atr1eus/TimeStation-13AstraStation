using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueConditional
{
    [SerializeField]
    public enum VariableType
    {
        INT,
        STRING,
        BOOL,
        FLOAT
    };

    [SerializeField]
    public enum NumberCondition
    {
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        EqualTo,
    }

    public enum StringCondition
    {
        EqualTo,
        NotEqualTo,
        EqualToIgnoreCasing,
        NotEqualToIgnoreCasing
    }

    public enum BoolCondition
    {
        FALSE,
        TRUE
    }

    public VariableType type;
    public string variable;
    public NumberCondition numberCondition;
    public StringCondition stringCondition;
    public BoolCondition boolCondition;
    public float floatTarget;
    public string stringTarget;
    public bool boolTarget;
    public int intTarget;

}


