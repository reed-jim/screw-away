using UnityEngine;
using static GameEnum;

public class ScrewBoxFaction : MonoBehaviour
{
    [SerializeField] private GameFaction faction;

    [SerializeField] private ScrewBoxServiceLocator screwBoxServiceLocator;

    public GameFaction Faction
    {
        get => faction;
    }

    void Awake()
    {
        screwBoxServiceLocator.screwBoxMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
    }
}
