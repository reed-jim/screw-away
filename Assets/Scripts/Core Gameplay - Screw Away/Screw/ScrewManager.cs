using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;

public class ScrewManager : MonoBehaviour
{
    [SerializeField] private List<BaseScrew> _screws;

    public static event Action<GameFaction> spawnScrewBoxEvent;

    private int _totalScrew;

    [SerializeField] private LevelDifficultyConfiguration levelDifficultyConfiguration;

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

        _totalScrew++;
    }

    private Dictionary<GameFaction, int> GetRemainingScrewByFaction()
    {
        Dictionary<GameFaction, int> remainingScrewByFaction = new Dictionary<GameFaction, int>();

        for (int i = 0; i < _screws.Count; i++)
        {
            if (_screws[i].IsDone)
            {
                continue;
            }

            if (!remainingScrewByFaction.ContainsKey(_screws[i].Faction))
            {
                remainingScrewByFaction.Add(_screws[i].Faction, 1);
            }
            else
            {
                remainingScrewByFaction[_screws[i].Faction]++;
            }
        }

        return remainingScrewByFaction;
    }

    private async void SpawnScrewBox()
    {
        await Task.Delay(1000);

        Debug.Log("TOTAL: " + _totalScrew);

        GameFaction? lastFaction = null;

        for (int i = 0; i < _screws.Count; i++)
        {
            _screws[i].CountBlockingObjects();
        }

        for (int i = 0; i < _screws.Count; i++)
        {
            if (_screws[i].NumberBlockingObjects == 0)
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

                        break;
                    }
                }
            }
        }
    }

    private async void SpawnNewScrewBox()
    {
        float progress;
        int doneScrew = 0;

        int maxNumberBlockingObject = 0;

        for (int i = 0; i < _screws.Count; i++)
        {
            _screws[i].CountBlockingObjects();

            if (_screws[i].NumberBlockingObjects > maxNumberBlockingObject)
            {
                maxNumberBlockingObject = _screws[i].NumberBlockingObjects;
            }

            if (_screws[i].IsDone)
            {
                doneScrew++;
            }
        }

        progress = (float)doneScrew / _totalScrew;

        LevelDifficulty levelDifficulty = LevelDifficulty.Easy;

        foreach (var phase in levelDifficultyConfiguration.LevelPhases)
        {
            if (progress >= phase.StartProgress && progress <= phase.EndProgress)
            {
                levelDifficulty = phase.LevelDifficulty;

                break;
            }
        }

        Debug.Log($"Current Progress: " + progress);
        Debug.Log($"Current Progress: " + levelDifficulty);

        Dictionary<GameFaction, int> remainingScrewByFaction = GetRemainingScrewByFaction();

        foreach (var faction in remainingScrewByFaction.Keys)
        {
            Debug.Log($"Faction {faction} - {remainingScrewByFaction[faction]}");
        }

        bool isFound = false;

        for (int i = 0; i < _screws.Count; i++)
        {
            if (!_screws[i].IsDone && remainingScrewByFaction[_screws[i].Faction] >= 3)
            {
                if (levelDifficulty == LevelDifficulty.Easy)
                {
                    if (_screws[i].NumberBlockingObjects == 0)
                    {
                        Spawn(_screws[i].Faction);

                        isFound = true;

                        break;
                    }
                }
                else if (levelDifficulty == LevelDifficulty.Normal)
                {
                    if (_screws[i].NumberBlockingObjects == 1)
                    {
                        Spawn(_screws[i].Faction);

                        isFound = true;

                        break;
                    }
                }
                else if (levelDifficulty == LevelDifficulty.Hard)
                {
                    if (_screws[i].NumberBlockingObjects == 2)
                    {
                        Spawn(_screws[i].Faction);

                        isFound = true;

                        break;
                    }
                }
            }
        }

        Debug.Log("FOUND " + isFound);

        void Spawn(GameFaction faction)
        {
            spawnScrewBoxEvent?.Invoke(faction);
        }
    }
}
