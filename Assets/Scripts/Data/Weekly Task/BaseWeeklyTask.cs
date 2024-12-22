using System;
using UnityEngine;

[Serializable]
public abstract class BaseWeeklyTask
{
    public abstract float RequirementValue
    {
        get; set;
    }

    public abstract float CurrentValue
    {
        get; set;
    }

    public float Progress
    {
        get => CurrentValue / RequirementValue;
    }

    public abstract void Init();

    // public void SetRequirement(float value)
    // {
    //     _requirementValue = value;
    // }

    public void MakeProgress(float addedValue)
    {
        CurrentValue += addedValue;
    }
}
