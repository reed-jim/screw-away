using System;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class ScrewBox : MonoBehaviour
{
    [SerializeField] private ScrewBoxSlot[] screwBoxSlots;

    [SerializeField] private ScrewBoxServiceLocator screwBoxServiceLocator;

    #region EVENT
    public static event Action spawnNewScrewBoxEvent;
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

    private void Awake()
    {
        ScrewBoxSlot.screwBoxCompleteEvent += OnScrewBoxCompleted;
    }

    private void OnDestroy()
    {
        ScrewBoxSlot.screwBoxCompleteEvent -= OnScrewBoxCompleted;
    }

    private void OnScrewBoxCompleted(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Tween.LocalPositionX(transform, 10, duration: 0.5f).OnComplete(() =>
            {
                spawnNewScrewBoxEvent?.Invoke();
            });
        }
    }
}
