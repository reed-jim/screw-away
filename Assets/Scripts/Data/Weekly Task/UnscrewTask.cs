using UnityEngine;

public class UnscrewTask : BaseWeeklyTask
{
    public override void Init()
    {
        BasicScrew.screwLoosenedEvent += OnScrewLoosened;
    }

    private void OnScrewLoosened()
    {
        MakeProgress(1);
    }
}
