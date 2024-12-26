using System;
using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using UnityEngine;
using static GameEnum;

public class ScrewBox : MonoBehaviour
{
    [SerializeField] private ScrewBoxSlot[] screwBoxSlots;

    [SerializeField] private ScrewBoxServiceLocator screwBoxServiceLocator;

    [SerializeField] private bool isLocked;

    #region EVENT
    public static event Action<ScrewBox> screwBoxCompletedEvent;
    public static event Action spawnNewScrewBoxEvent;
    public static event Action<ScrewBox> setFactionForScrewBoxEvent;
    public static event Action screwBoxUnlockedEvent;
    #endregion

    #region PRIVATE FIELD
    private List<Tween> _tweens;
    #endregion

    public GameFaction Faction
    {
        get => screwBoxServiceLocator.screwBoxFaction.Faction;
        set => screwBoxServiceLocator.screwBoxFaction.Faction = value;
    }

    public ScrewBoxSlot[] ScrewBoxSlots
    {
        get => screwBoxSlots;
    }

    public bool IsLocked
    {
        get => isLocked;
        set => isLocked = value;
    }

    public ScrewBoxServiceLocator ScrewBoxServiceLocator
    {
        get => screwBoxServiceLocator;
    }

    private void Awake()
    {
        ScrewBoxSlot.screwBoxCompleteEvent += OnScrewBoxCompleted;
        ScrewBoxUI.unlockScrewBox += Unlock;

        _tweens = new List<Tween>();
    }

    private void OnDestroy()
    {
        ScrewBoxSlot.screwBoxCompleteEvent -= OnScrewBoxCompleted;
        ScrewBoxUI.unlockScrewBox -= Unlock;

        CommonUtil.StopAllTweens(_tweens);
    }

    private void OnScrewBoxCompleted(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            AudioSource screwBoxDoneSound = ObjectPoolingEverything.GetFromPool<AudioSource>(GameConstants.SCREW_BOX_DONE_SOUND);

            screwBoxDoneSound.Play();

            float initialPositionY = transform.position.y;

            Tween.LocalPositionY(transform, transform.position.y + 8, duration: 0.5f).OnComplete(() =>
            {
                screwBoxCompletedEvent?.Invoke(this);
                spawnNewScrewBoxEvent?.Invoke();

                transform.position = transform.position.ChangeY(initialPositionY);

                ObjectPoolingEverything.ReturnToPool(GameConstants.SCREW_BOX, gameObject);
            });
        }
    }

    public void Lock()
    {
        screwBoxServiceLocator.screwBoxUI.Lock();

        isLocked = true;
    }

    public void Unlock()
    {
        // CLEAR SCREWS
        for (int i = 0; i < screwBoxSlots.Length; i++)
        {
            if (screwBoxSlots[i].IsFilled)
            {
                screwBoxSlots[i].IsFilled = false;

                Destroy(screwBoxSlots[i].Screw.gameObject);

                screwBoxSlots[i].Screw = null;
            }
        }

        isLocked = false;

        setFactionForScrewBoxEvent?.Invoke(this);

        screwBoxUnlockedEvent?.Invoke();
    }

    private void Unlock(int instanceId)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Unlock();
        }
    }
}
