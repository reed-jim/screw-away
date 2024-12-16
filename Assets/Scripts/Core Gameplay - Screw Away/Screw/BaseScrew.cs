using System;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BaseScrew : MonoBehaviour, IScrew
{
    [SerializeField] private string screwId;

    [SerializeField] private ScrewServiceLocator screwServiceLocator;

    public string ScrewId
    {
        get => screwId;
        set => screwId = value;
    }

    public GameFaction Faction
    {
        get => screwServiceLocator.screwFaction.Faction;
    }

    #region EVENT
    public static event Action<string, GameFaction> selectScrewEvent;
    #endregion

    void Awake()
    {
        ScrewBoxManager.looseScrewEvent += Loose;
    }

    void OnDestroy()
    {
        ScrewBoxManager.looseScrewEvent -= Loose;
    }

    public void Select()
    {
        selectScrewEvent?.Invoke(screwId, Faction);
    }

    public void Loose(string screwId, GameFaction faction, Vector3 screwBoxPosition)
    {
        if (screwId == this.screwId)
        {
            Tween.Rotation(transform, Quaternion.Euler(new Vector3(-30, 180, 0)), duration: 1f);
            Tween.Position(transform, screwBoxPosition, duration: 1f);

            // gameObject.SetActive(false);
        }
    }
}
