using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class BaseScrew : MonoBehaviour, IScrew
{
    [SerializeField] protected int screwId;

    [SerializeField] protected ScrewServiceLocator screwServiceLocator;

    [Header("CUSTOMIZE")]
    [SerializeField] protected float scaleOnScrewBox;

    #region PRIVATE FIELD
    protected List<Tween> _tweens = new List<Tween>();
    protected int _numberBlockingObjects;
    protected Vector3 _initialScale;
    protected bool _isInteractable = true;
    protected bool _isDone;
    protected ScrewData _screwData;
    #endregion

    public int ScrewId
    {
        get => screwId;
        set => screwId = value;
    }

    public ScrewData ScrewData
    {
        get => _screwData;
    }

    public GameFaction Faction
    {
        get => screwServiceLocator.screwFaction.Faction;
        set => screwServiceLocator.screwFaction.Faction = value;
    }

    public int NumberBlockingObjects
    {
        get => _numberBlockingObjects;
    }

    public bool IsDone
    {
        get => _isDone;
        set => _isDone = value;
    }

    #region EVENT
    public static event Action<int, GameFaction> selectScrewEvent;
    public static event Action<BaseScrew> addScrewToListEvent;
    #endregion

    protected virtual void Awake()
    {
        ScrewBoxManager.looseScrewEvent += Loose;
        RegisterMoreEvent();

        screwId = gameObject.GetInstanceID();

        AddScrewToList();

        _initialScale = transform.localScale;

        MoreLogicInAwake();
    }

    void OnDestroy()
    {
        ScrewBoxManager.looseScrewEvent -= Loose;
        UnregisterMoreEvent();

        CommonUtil.StopAllTweens(_tweens);
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

    public virtual void Loose(int screwId, GameFaction faction, ScrewBoxSlot screwBoxSlot)
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

    public bool IsValidToLoose()
    {
        CountBlockingObjects();

        if (_numberBlockingObjects > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ForceUnscrew()
    {
        _isDone = true;

        gameObject.SetActive(false);
    }

    public virtual int CountBlockingObjects()
    {
        return 0;
    }
}
