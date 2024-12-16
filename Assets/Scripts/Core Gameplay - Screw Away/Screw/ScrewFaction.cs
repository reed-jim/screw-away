using UnityEngine;
using static GameEnum;

public class ScrewFaction : MonoBehaviour
{
    [SerializeField] private ScrewServiceLocator screwServiceLocator;

    [SerializeField] private GameFaction faction;

    public GameFaction Faction
    {
        get => faction;
    }

    void Awake()
    {
        screwServiceLocator.screwMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
    }
}
