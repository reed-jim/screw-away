using UnityEngine;
using static GameEnum;

public class ScrewBoxFaction : MonoBehaviour
{
    [SerializeField] private GameFaction faction;

    [SerializeField] private ScrewBoxServiceLocator screwBoxServiceLocator;

    public GameFaction Faction
    {
        get => faction;
        set => SetFaction(value);
    }

    void Awake()
    {
        screwBoxServiceLocator.screwBoxMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
    }

    private void SetFaction(GameFaction faction)
    {
        this.faction = faction;

        screwBoxServiceLocator.screwBoxMaterialPropertyBlock.SetColor(FactionUtility.GetColorForFaction(faction));
    }
}
