using System;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class ScrewBox : MonoBehaviour
{
    [SerializeField] private ScrewBoxSlot[] screwBoxSlots;

    [SerializeField] private ScrewBoxServiceLocator screwBoxServiceLocator;

    [SerializeField] private bool isLocked;

    #region EVENT
    public static event Action<ScrewBox> screwBoxCompletedEvent;
    public static event Action spawnNewScrewBoxEvent;
    public static event Action<ScrewBox> setFactionForScrewBoxEvent;
    #endregion

    public GameFaction Faction
    {
        get => screwBoxServiceLocator.screwBoxFaction.Faction;
        set => screwBoxServiceLocator.screwBoxFaction.Faction = value;
    }

    public ScrewBoxSlot[] ScrewBoxSlots
    {
        get => screwBoxSlots;
    }

    public bool IsLocked
    {
        get => isLocked;
        set => isLocked = value;
    }

    public ScrewBoxServiceLocator ScrewBoxServiceLocator
    {
        get => screwBoxServiceLocator;
    }

    private void Awake()
    {
        ScrewBoxSlot.screwBoxCompleteEvent += OnScrewBoxCompleted;
        ScrewBoxUI.unlockScrewBox += Unlock;
    }

    private void OnDestroy()
    {
        ScrewBoxSlot.screwBoxCompleteEvent -= OnScrewBoxCompleted;
        ScrewBoxUI.unlockScrewBox -= Unlock;
    }

    private void OnScrewBoxCompleted(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Tween.LocalPositionX(transform, 10, duration: 0.5f).OnComplete(() =>
            {
                screwBoxCompletedEvent?.Invoke(this);
                spawnNewScrewBoxEvent?.Invoke();
            });
        }
    }

    public void Lock()
    {
        screwBoxServiceLocator.screwBoxUI.Lock();

        isLocked = true;
    }

    private void Unlock(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            isLocked = false;

            setFactionForScrewBoxEvent?.Invoke(this);
        }
    }
}
