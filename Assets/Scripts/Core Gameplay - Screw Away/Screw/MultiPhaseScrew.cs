using System;
using UnityEngine;

public class MultiPhaseScrew : BasicScrew
{
    [SerializeField] private int phase;

    [SerializeField] private MeshRenderer screwRenderer;

    #region PRIVATE FIELD
    private bool _isLocked;
    #endregion

    public static event Action<MultiPhaseScrew> manageScrewEvent;

    public int Phase
    {
        get => phase;
        set => phase = value;
    }

    protected override void MoreLogicInAwake()
    {
        if (phase != 0)
        {
            screwServiceLocator.screwMaterialPropertyBlock.SetColor(Color.black);

            _isLocked = true;
        }
    }

    protected override void RegisterMoreEvent()
    {
        MultiPhaseLevelManager.switchPhaseEvent += OnPhaseSwitched;
    }

    protected override void UnregisterMoreEvent()
    {
        MultiPhaseLevelManager.switchPhaseEvent -= OnPhaseSwitched;
    }

    private void Start()
    {
        manageScrewEvent?.Invoke(this);
    }

    public override void Loose(string screwId, GameEnum.GameFaction faction, ScrewBoxSlot screwBoxSlot)
    {
        if (_isLocked)
        {
            return;
        }

        base.Loose(screwId, faction, screwBoxSlot);
    }

    private void OnPhaseSwitched(int phase)
    {
        if (phase == this.phase)
        {
            screwServiceLocator.screwFaction.SetColorByFaction();

            _isLocked = false;
        }
    }
}
