using System;
using System.Linq;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class ScrewBoxManager : MonoBehaviour
{
    [SerializeField] private ScrewBox[] screwBoxs;
    [SerializeField] private ScrewBoxSlot[] screwPorts;

    [SerializeField] private int maxScrewBox;

    #region EVENT
    public static event Action<string, GameFaction, ScrewBoxSlot> looseScrewEvent;
    #endregion

    void Awake()
    {
        BaseScrew.selectScrewEvent += OnScrewSelected;
        ScrewManager.spawnScrewBoxEvent += SpawnScrewBox;
        ScrewManager.spawnAdsScrewBoxesEvent += SpawnAdsScrewBoxes;
        ScrewBox.screwBoxCompletedEvent += OnScrewBoxCompleted;

        screwBoxs = new ScrewBox[maxScrewBox];
    }

    void OnDestroy()
    {
        BaseScrew.selectScrewEvent -= OnScrewSelected;
        ScrewManager.spawnScrewBoxEvent -= SpawnScrewBox;
        ScrewManager.spawnAdsScrewBoxesEvent -= SpawnAdsScrewBoxes;
        ScrewBox.screwBoxCompletedEvent -= OnScrewBoxCompleted;
    }

    private ScrewBoxSlot CheckAvailableScrewBoxes(GameFaction selectedFaction)
    {
        foreach (var screwBox in screwBoxs)
        {
            if (screwBox == null)
            {
                continue;
            }

            if (screwBox.IsLocked)
            {
                continue;
            }

            if (selectedFaction == screwBox.Faction)
            {
                foreach (var screwBoxSlot in screwBox.ScrewBoxSlots)
                {
                    if (!screwBoxSlot.IsFilled)
                    {
                        return screwBoxSlot;
                    }
                }
            }
        }

        foreach (var screwPort in screwPorts)
        {
            if (!screwPort.IsFilled)
            {
                return screwPort;
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

    private void OnScrewBoxCompleted(ScrewBox screwBox)
    {
        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == screwBox)
            {
                screwBoxs[i] = null;

                break;
            }
        }
    }

    private void SpawnScrewBox(GameFaction faction)
    {
        ScrewBox screwBox = ObjectPoolingEverything.GetFromPool<ScrewBox>(GameConstants.SCREW_BOX);

        screwBox.Faction = faction;

        screwBox.transform.position = new Vector3(-10, 10, screwBox.transform.position.z);

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

        for (int i = 0; i < screwBox.ScrewBoxSlots.Length; i++)
        {
            ScrewBoxSlot screwBoxSlot = screwBox.ScrewBoxSlots[i];

            if (screwBoxSlot.IsFilled)
            {
                screwBoxSlot.Screw.gameObject.SetActive(false);

                screwBoxSlot.Screw = null;

                screwBoxSlot.IsFilled = false;
            }
        }

        Tween.LocalPositionX(screwBox.transform, (-(maxScrewBox - 1) / 2f + index) * 2.5f, duration: 0.5f).OnComplete(() =>
        {
            MoveFromScrewPortToScrewBox();
        });
    }

    private void SpawnAdsScrewBoxes()
    {
        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == null)
            {
                ScrewBox screwBox = ObjectPoolingEverything.GetFromPool<ScrewBox>(GameConstants.SCREW_BOX);

                screwBox.transform.position = new Vector3(10, 10, screwBox.transform.position.z);

                screwBox.Lock();

                int index = i;

                Tween.LocalPositionX(screwBox.transform, (-(maxScrewBox - 1) / 2f + index) * 2.5f, duration: 0.5f);

                screwBoxs[i] = screwBox;
            }
        }
    }

    private void MoveFromScrewPortToScrewBox()
    {
        for (int i = 0; i < screwBoxs.Length; i++)
        {
            if (screwBoxs[i] == null)
            {
                continue;
            }

            for (int j = 0; j < screwPorts.Length; j++)
            {
                if (screwPorts[j].IsFilled && screwPorts[j].Screw.Faction == screwBoxs[i].Faction)
                {
                    for (int k = 0; k < screwBoxs[i].ScrewBoxSlots.Length; k++)
                    {
                        if (!screwBoxs[i].ScrewBoxSlots[k].IsFilled)
                        {
                            int screwPortIndex = j;
                            int screwBoxIndex = i;
                            int screwBoxSlotIndex = k;

                            screwBoxs[screwBoxIndex].ScrewBoxSlots[screwBoxSlotIndex].Fill(screwPorts[screwPortIndex].Screw);

                            screwPorts[screwPortIndex].Screw.transform.SetParent(screwBoxs[screwBoxIndex].ScrewBoxSlots[screwBoxSlotIndex].transform);

                            Tween.Position(screwPorts[screwPortIndex].Screw.transform,
                                screwBoxs[screwBoxIndex].ScrewBoxSlots[screwBoxSlotIndex].transform.position + new Vector3(0, 0, -0.3f), duration: 0.5f)
                            .OnComplete(() =>
                            {
                                screwPorts[screwPortIndex].IsFilled = false;

                                screwBoxs[screwBoxIndex].ScrewBoxSlots[screwBoxSlotIndex].CompleteFill();
                            });

                            break;
                        }
                    }
                }
            }
        }
    }
}
