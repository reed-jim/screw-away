using System;
using UnityEngine;
using static GameEnum;

public class ScrewBoxManager : MonoBehaviour
{
    [SerializeField] private ScrewBox[] screwBoxs;

    #region EVENT
    public static event Action<string, GameFaction, Vector3> looseScrewEvent;
    #endregion

    void Awake()
    {
        BaseScrew.selectScrewEvent += OnScrewSelected;
    }

    void OnDestroy()
    {
        BaseScrew.selectScrewEvent -= OnScrewSelected;
    }

    private ScrewBox CheckAvailableScrewBoxes(GameFaction selectedFaction)
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

                        return screwBox;
                    }
                }
            }
        }

        return null;
    }

    private void OnScrewSelected(string screwId, GameFaction selectedFaction)
    {
        ScrewBox screwBox = CheckAvailableScrewBoxes(selectedFaction);

        if (screwBox != null)
        {
            looseScrewEvent?.Invoke(screwId, selectedFaction, screwBox.transform.position + new Vector3(0, 0, -1));
        }
    }
}
