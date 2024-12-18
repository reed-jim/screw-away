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

        GameFaction[] factions = new GameFaction[5] { GameFaction.Red, GameFaction.Blue, GameFaction.Green, GameFaction.Purple, GameFaction.Orange };

        for (int i = 0; i < factions.Length; i++)
        {
            remainingScrewByFaction.Add(factions[i], 0);
        }

        for (int i = 0; i < _screws.Count; i++)
        {
            if (_screws[i].IsDone)
            {
                continue;
            }

            remainingScrewByFaction[_screws[i].Faction]++;
        }

        return remainingScrewByFaction;
    }

    private Dictionary<GameFaction, int> GetTotalBlockObjectsByFaction()
    {
        Dictionary<GameFaction, int> totalBlockObjectsByFaction = new Dictionary<GameFaction, int>();

        GameFaction[] factions = new GameFaction[5] { GameFaction.Red, GameFaction.Blue, GameFaction.Green, GameFaction.Purple, GameFaction.Orange };

        for (int i = 0; i < factions.Length; i++)
        {
            totalBlockObjectsByFaction.Add(factions[i], 0);
        }

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

            totalBlockObjectsByFaction[_screws[i].Faction] += numberBlockObject;
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
        GameFaction faction = GetFactionForNewScrewBox();

        if (faction == GameFaction.None)
        {
            return;
        }

        Spawn(faction);

        void Spawn(GameFaction faction)
        {
            spawnScrewBoxEvent?.Invoke(faction);
        }
    }

    private void AssignFactionForNewScrewBox(ScrewBox screwBox)
    {
        GameFaction faction = GetFactionForNewScrewBox();

        if (faction == GameFaction.None)
        {
            return;
        }

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

        GameFaction nextFaction = GameFaction.None;

        int difficultyLevel = factionSortedByDifficulty.Length;

        bool isFound = false;

        // Debug.Log("---------------------------------------");

        // for (int i = 0; i < factionSortedByDifficulty.Length; i++)
        // {
        //     GameFaction faction = factionSortedByDifficulty[i];

        //     Debug.Log($"{faction} Remaining: {remainingScrewByFaction[faction]} Available: {screwPortAvailableByFaction[faction]}");
        // }

        for (int i = 0; i < factionSortedByDifficulty.Length; i++)
        {
            GameFaction faction = factionSortedByDifficulty[i];

            if ((remainingScrewByFaction[faction] - screwPortAvailableByFaction[faction]) < 3)
            {
                continue;
            }

            if (levelDifficulty == LevelDifficulty.Easy && difficultyLevel <= 3)
            {
                nextFaction = factionSortedByDifficulty[4];

                isFound = true;

                break;
            }
            else if (levelDifficulty == LevelDifficulty.Normal && difficultyLevel <= 4)
            {
                nextFaction = factionSortedByDifficulty[1];

                isFound = true;

                break;
            }
            else if (levelDifficulty == LevelDifficulty.Hard && difficultyLevel <= 5)
            {
                nextFaction = factionSortedByDifficulty[0];

                isFound = true;

                break;
            }

            levelDifficulty--;
        }

        if (!isFound)
        {
            for (int i = 0; i < factionSortedByDifficulty.Length; i++)
            {
                GameFaction faction = factionSortedByDifficulty[i];

                if ((remainingScrewByFaction[faction] - screwPortAvailableByFaction[faction]) < 3)
                {
                    continue;
                }
                else
                {
                    nextFaction = faction;

                    isFound = true;

                    break;
                }
            }
        }

        // for (int i = 0; i < _screws.Count; i++)
        // {
        //     Debug.Log(remainingScrewByFaction[_screws[i].Faction] + "/" + screwPortAvailableByFaction[_screws[i].Faction]);

        //     if (!_screws[i].IsDone && (remainingScrewByFaction[_screws[i].Faction] - screwPortAvailableByFaction[_screws[i].Faction]) >= 3)
        //     {
        //         if (levelDifficulty == LevelDifficulty.Easy)
        //         {
        //             nextFaction = factionSortedByDifficulty[4];

        //             break;
        //         }
        //         else if (levelDifficulty == LevelDifficulty.Normal)
        //         {
        //             nextFaction = factionSortedByDifficulty[1];

        //             break;
        //         }
        //         else if (levelDifficulty == LevelDifficulty.Hard)
        //         {
        //             nextFaction = factionSortedByDifficulty[0];

        //             break;
        //         }
        //     }
        // }

        return nextFaction;
    }
}
