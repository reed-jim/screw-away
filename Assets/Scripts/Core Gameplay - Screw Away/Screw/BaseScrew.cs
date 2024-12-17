using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BaseScrew : MonoBehaviour, IScrew
{
    [SerializeField] protected string screwId;

    [SerializeField] protected ScrewServiceLocator screwServiceLocator;

    protected int _numberBlockingObjects;
    protected Vector3 _initialScale;
    protected bool _isDone;

    public string ScrewId
    {
        get => screwId;
        set => screwId = value;
    }

    public GameFaction Faction
    {
        get => screwServiceLocator.screwFaction.Faction;
    }

    public int NumberBlockingObjects
    {
        get => _numberBlockingObjects;
    }

    public bool IsDone
    {
        get => _isDone;
    }

    #region EVENT
    public static event Action<string, GameFaction> selectScrewEvent;
    public static event Action<BaseScrew> addScrewToListEvent;
    #endregion

    void Awake()
    {
        ScrewBoxManager.looseScrewEvent += Loose;

        AddScrewToList();

        _initialScale = transform.localScale;
    }

    void OnDestroy()
    {
        ScrewBoxManager.looseScrewEvent -= Loose;
    }

    public void Select()
    {
        selectScrewEvent?.Invoke(screwId, Faction);
    }

    public virtual void Loose(string screwId, GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {
        if (screwId == this.screwId)
        {
            Tween.Rotation(transform, Quaternion.Euler(new Vector3(-30, 180, 0)), duration: 1f);
            Tween.Position(transform, screwBoxSlot.transform.position + new Vector3(0, 0, -0.3f), duration: 1f);
        }
    }

    private async void AddScrewToList()
    {
        await Task.Delay(200);

        addScrewToListEvent?.Invoke(this);
    }

    public virtual int CountBlockingObjects()
    {
        return 0;
    }
}
