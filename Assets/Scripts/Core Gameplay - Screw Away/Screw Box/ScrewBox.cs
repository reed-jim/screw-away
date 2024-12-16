using UnityEngine;
using static GameEnum;

public class ScrewBox : MonoBehaviour
{
    [SerializeField] private ScrewBoxSlot[] screwBoxSlots;

    [SerializeField] private ScrewBoxServiceLocator screwBoxServiceLocator;

    public GameFaction Faction
    {
        get => screwBoxServiceLocator.screwBoxFaction.Faction;
    }

    public ScrewBoxSlot[] ScrewBoxSlots
    {
        get => screwBoxSlots;
    }
}
