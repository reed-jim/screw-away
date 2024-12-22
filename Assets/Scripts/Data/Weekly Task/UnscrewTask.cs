using System;
using UnityEngine;

public class UnscrewTask : BaseWeeklyTask
{
    public override float RequirementValue
    {
        get; set;
    }

    public override float CurrentValue
    {
        get; set;
    }

    public override void Init()
    {
        BasicScrew.screwLoosenedEvent += OnScrewLoosened;
    }

    private void OnScrewLoosened()
    {
        MakeProgress(1);
    }
}
