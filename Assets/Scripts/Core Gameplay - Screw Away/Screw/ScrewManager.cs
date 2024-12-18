using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;

public class ScrewManager : MonoBehaviour
{
    [SerializeField] private List<BaseScrew> _screws;

    [SerializeField] private ScrewBoxManager screwBoxManager;

    [SerializeField] private LevelDifficultyConfiguration levelDifficultyConfiguration;

    #region PRIVATE FIELD
    private int _totalScrew;
    #endregion

    #region EVENT
    public static event Action<GameFaction> spawnScrewBoxEvent;
    public static event Action spawnAdsScrewBoxesEvent;
    #endregion

    void Awake()
    {
        BaseScrew.addScrewToListEvent += AddScrew;
        ScrewBox.spawnNewScrewBoxEvent += SpawnNewScrewBox;
        ScrewBox.setFactionForScrewBoxEvent += AssignFactionForNewScrewBox;

        _screws = new List<BaseScrew>();

        SpawnScrewBox();
    }

    void OnDestroy()
    {
        BaseScrew.addScrewToListEvent -= AddScrew;
        ScrewBox.spawnNewScrewBoxEvent -= SpawnNewScrewBox;
        ScrewBox.setFactionForScrewBoxEvent -= AssignFactionForNewScrewBox;
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

    private Dictionary<GameFaction, int> GetTotalBlockObjectsByFaction()
    {
        Dictionary<GameFaction, int> totalBlockObjectsByFaction = new Dictionary<GameFaction, int>();

        for (int i = 0; i < _screws.Count; i++)
        {
            if (_screws[i].IsDone)
            {
                continue;
            }

            int numberBlockObject = _screws[i].NumberBlockingObjects;

            // Compensate
            if (numberBlockObject == 0)
            {
                numberBlockObject = -2;
            }

            if (!totalBlockObjectsByFaction.ContainsKey(_screws[i].Faction))
            {
                totalBlockObjectsByFaction.Add(_screws[i].Faction, numberBlockObject);
            }
            else
            {
                totalBlockObjectsByFaction[_screws[i].Faction] += numberBlockObject;
            }
        }

        return totalBlockObjectsByFaction;
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

        spawnAdsScrewBoxesEvent?.Invoke();
    }

    private async void SpawnNewScrewBox()
    {
        // float progress;
        // int doneScrew = 0;

        // int maxNumberBlockingObject = 0;

        // for (int i = 0; i < _screws.Count; i++)
        // {
        //     if (_screws[i].IsDone)
        //     {
        //         doneScrew++;
        //     }
        //     else
        //     {
        //         _screws[i].CountBlockingObjects();

        //         if (_screws[i].NumberBlockingObjects > maxNumberBlockingObject)
        //         {
        //             maxNumberBlockingObject = _screws[i].NumberBlockingObjects;
        //         }
        //     }
        // }

        // progress = (float)doneScrew / _totalScrew;

        // LevelDifficulty levelDifficulty = LevelDifficulty.Easy;

        // foreach (var phase in levelDifficultyConfiguration.LevelPhases)
        // {
        //     if (progress >= phase.StartProgress && progress <= phase.EndProgress)
        //     {
        //         levelDifficulty = phase.LevelDifficulty;

        //         break;
        //     }
        // }

        // Debug.Log($"Current Progress: " + progress);
        // Debug.Log($"Current Progress: " + levelDifficulty);

        // Dictionary<GameFaction, int> remainingScrewByFaction = GetRemainingScrewByFaction();
        // Dictionary<GameFaction, int> totalBlockObjectsByFaction = GetTotalBlockObjectsByFaction();

        // GameFaction[] factionSortedByDifficulty = totalBlockObjectsByFaction.OrderByDescending(item => item.Value).Select(item => item.Key).ToArray();

        // foreach (var faction in remainingScrewByFaction.Keys)
        // {
        //     Debug.Log($"Faction {faction} / {remainingScrewByFaction[faction]} / {totalBlockObjectsByFaction[faction]}");
        // }

        // foreach (var faction in factionSortedByDifficulty)
        // {
        //     Debug.Log($"HARD Faction: {faction}");
        // }

        // bool isFound = false;

        // GameFaction nextFaction;

        // for (int i = 0; i < _screws.Count; i++)
        // {
        //     if (!_screws[i].IsDone && remainingScrewByFaction[_screws[i].Faction] >= 3)
        //     {
        //         if (levelDifficulty == LevelDifficulty.Easy)
        //         {
        //             Spawn(factionSortedByDifficulty[4]);

        //             isFound = true;

        //             break;

        //             // if (_screws[i].NumberBlockingObjects == 0)
        //             // {
        //             //     Spawn(_screws[i].Faction);

        //             //     isFound = true;

        //             //     break;
        //             // }
        //         }
        //         else if (levelDifficulty == LevelDifficulty.Normal)
        //         {
        //             Spawn(factionSortedByDifficulty[1]);

        //             isFound = true;

        //             break;

        //             // if (_screws[i].NumberBlockingObjects == 1)
        //             // {
        //             //     Spawn(_screws[i].Faction);

        //             //     isFound = true;

        //             //     break;
        //             // }
        //         }
        //         else if (levelDifficulty == LevelDifficulty.Hard)
        //         {
        //             Spawn(factionSortedByDifficulty[0]);

        //             isFound = true;

        //             break;

        //             // if (_screws[i].NumberBlockingObjects == 2)
        //             // {
        //             //     Spawn(_screws[i].Faction);

        //             //     isFound = true;

        //             //     break;
        //             // }
        //         }
        //     }
        // }

        // Debug.Log("FOUND " + isFound);

        GameFaction faction = GetFactionForNewScrewBox();

        Spawn(faction);

        void Spawn(GameFaction faction)
        {
            spawnScrewBoxEvent?.Invoke(faction);
        }
    }

    private void AssignFactionForNewScrewBox(ScrewBox screwBox)
    {
        GameFaction faction = GetFactionForNewScrewBox();

        screwBox.Faction = faction;
    }

    private GameFaction GetFactionForNewScrewBox()
    {
        float progress;
        int doneScrew = 0;

        int maxNumberBlockingObject = 0;

        for (int i = 0; i < _screws.Count; i++)
        {
            if (_screws[i].IsDone)
            {
                doneScrew++;
            }
            else
            {
                _screws[i].CountBlockingObjects();

                if (_screws[i].NumberBlockingObjects > maxNumberBlockingObject)
                {
                    maxNumberBlockingObject = _screws[i].NumberBlockingObjects;
                }
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

        Dictionary<GameFaction, int> screwPortAvailableByFaction = screwBoxManager.GetScrewPortAvailableByFaction();
        Dictionary<GameFaction, int> remainingScrewByFaction = GetRemainingScrewByFaction();
        Dictionary<GameFaction, int> totalBlockObjectsByFaction = GetTotalBlockObjectsByFaction();

        GameFaction[] factionSortedByDifficulty = totalBlockObjectsByFaction.OrderByDescending(item => item.Value).Select(item => item.Key).ToArray();

        GameFaction nextFaction = GameFaction.Red;

        for (int i = 0; i < _screws.Count; i++)
        {
            if (!_screws[i].IsDone && remainingScrewByFaction[_screws[i].Faction] - screwPortAvailableByFaction[_screws[i].Faction] >= 3)
            {
                if (levelDifficulty == LevelDifficulty.Easy)
                {
                    nextFaction = factionSortedByDifficulty[4];

                    break;
                }
                else if (levelDifficulty == LevelDifficulty.Normal)
                {
                    nextFaction = factionSortedByDifficulty[1];

                    break;
                }
                else if (levelDifficulty == LevelDifficulty.Hard)
                {
                    nextFaction = factionSortedByDifficulty[0];

                    break;
                }
            }
        }

        return nextFaction;
    }
}
