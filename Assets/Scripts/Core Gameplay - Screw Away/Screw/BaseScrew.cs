using System;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BaseScrew : MonoBehaviour, IScrew
{
    [SerializeField] protected string screwId;

    [SerializeField] protected ScrewServiceLocator screwServiceLocator;

    [Header("CUSTOMIZE")]
    [SerializeField] protected float scaleOnScrewBox;

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

    protected virtual void Awake()
    {
        ScrewBoxManager.looseScrewEvent += Loose;
        RegisterMoreEvent();

        AddScrewToList();

        _initialScale = transform.localScale;

        MoreLogicInAwake();
    }

    void OnDestroy()
    {
        ScrewBoxManager.looseScrewEvent -= Loose;
        UnregisterMoreEvent();
    }

    protected virtual void MoreLogicInAwake()
    {

    }

    protected virtual void RegisterMoreEvent()
    {

    }

    protected virtual void UnregisterMoreEvent()
    {

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
