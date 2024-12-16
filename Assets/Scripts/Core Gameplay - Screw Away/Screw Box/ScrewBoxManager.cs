using System;
using System.Linq;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class ScrewBoxManager : MonoBehaviour
{
    [SerializeField] private ScrewBox[] screwBoxs;

    [SerializeField] private int maxScrewBox;

    #region EVENT
    public static event Action<string, GameFaction, ScrewBoxSlot> looseScrewEvent;
    #endregion

    void Awake()
    {
        BaseScrew.selectScrewEvent += OnScrewSelected;
        ScrewManager.spawnScrewBoxEvent += SpawnScrewBox;

        screwBoxs = new ScrewBox[maxScrewBox];
    }

    void OnDestroy()
    {
        BaseScrew.selectScrewEvent -= OnScrewSelected;
        ScrewManager.spawnScrewBoxEvent -= SpawnScrewBox;
    }

    private ScrewBoxSlot CheckAvailableScrewBoxes(GameFaction selectedFaction)
    {
        foreach (var screwBox in screwBoxs)
        {
            if (selectedFaction == screwBox.Faction)
            {
                foreach (var screwBoxSlot in screwBox.ScrewBoxSlots)
                {
                    if (!screwBoxSlot.IsFilled)
                    {
                        screwBoxSlot.IsFilled = true;

                        return screwBoxSlot;
                    }
                }
            }
        }

        return null;
    }

    private void OnScrewSelected(string screwId, GameFaction selectedFaction)
    {
        ScrewBoxSlot screwBoxSlot = CheckAvailableScrewBoxes(selectedFaction);

        if (screwBoxSlot != null)
        {
            looseScrewEvent?.Invoke(screwId, selectedFaction, screwBoxSlot);
        }
    }

    private void SpawnScrewBox(GameFaction faction)
    {
        ScrewBox screwBox = ObjectPoolingEverything.GetFromPool<ScrewBox>(GameConstants.SCREW_BOX);

        screwBox.Faction = faction;

        screwBox.transform.position = new Vector3(-10, 9, screwBox.transform.position.z);

        int index = 0;

        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == null)
            {
                screwBoxs[i] = screwBox;

                index = i;

                break;
            }
        }

        Tween.LocalPositionX(screwBox.transform, -4 + 3 * index, duration: 0.5f);
    }
}
