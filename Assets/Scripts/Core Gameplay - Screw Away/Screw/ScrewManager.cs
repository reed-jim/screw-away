using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;

public class ScrewManager : MonoBehaviour
{
    [SerializeField] private List<BaseScrew> _screws;

    public static event Action<GameFaction> spawnScrewBoxEvent;

    void Awake()
    {
        BaseScrew.addScrewToListEvent += AddScrew;
        ScrewBox.spawnNewScrewBoxEvent += SpawnNewScrewBox;

        _screws = new List<BaseScrew>();

        SpawnScrewBox();
    }

    void OnDestroy()
    {
        BaseScrew.addScrewToListEvent -= AddScrew;
        ScrewBox.spawnNewScrewBoxEvent -= SpawnNewScrewBox;
    }

    private void AddScrew(BaseScrew screw)
    {
        _screws.Add(screw);
    }

    private async void SpawnScrewBox()
    {
        await Task.Delay(1000);

        GameFaction? lastFaction = null;

        for (int i = 0; i < _screws.Count; i++)
        {
            GameFaction faction = _screws[i].Faction;

            if (lastFaction == null)
            {
                spawnScrewBoxEvent?.Invoke(faction);

                lastFaction = faction;
            }
            else
            {
                if (faction != lastFaction)
                {
                    spawnScrewBoxEvent?.Invoke(faction);
                }
            }
        }
    }

    private void SpawnNewScrewBox()
    {
        for (int i = 0; i < _screws.Count; i++)
        {
            GameFaction faction = _screws[i].Faction;

            spawnScrewBoxEvent?.Invoke(faction);

            break;
        }
    }
}
