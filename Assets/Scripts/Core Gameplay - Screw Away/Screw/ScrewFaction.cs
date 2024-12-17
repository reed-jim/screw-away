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
        faction = (GameFaction)Random.Range(0, 4);

        screwServiceLocator.screwMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
    }
}
