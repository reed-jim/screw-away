using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static GameEnum;

public class ScrewsDataManager : MonoBehaviour
{
    [SerializeField] private BaseScrew[] _screws;

    [SerializeField] private ScrewBoxManager screwBoxManager;

    [SerializeField] private IntVariable currentLevel;

    public static event Action spawnFreshLevelScrewBoxesEvent;


    void Awake()
    {
        ScrewManager.setScrewsEvent += OnAllLevelScrewsObservered;

        StartCoroutine(PeriodicallySaving());
    }

    void OnDestroy()
    {
        ScrewManager.setScrewsEvent -= OnAllLevelScrewsObservered;
    }

    private IEnumerator PeriodicallySaving()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(3f);

        while (true)
        {
            yield return waitForSeconds;

            SaveLevelData();
        }
    }

    private async void LoadLevelData()
    {
        LevelProgressData levelProgressData = DataUtility.Load<LevelProgressData>($"{GameConstants.SCREWS_DATA}_{currentLevel.Value}", new LevelProgressData());

        // No data for current level
        if (levelProgressData.ScrewsData == null)
        {
            spawnFreshLevelScrewBoxesEvent?.Invoke();

            return;
        }

        ScrewData[] screwsData = levelProgressData.ScrewsData;
        ScrewBoxData[] screwBoxesData = levelProgressData.ScrewBoxesData;

        for (int i = 0; i < screwBoxesData.Length; i++)
        {
            screwBoxManager.SpawnScrewBox(screwBoxesData[i].Faction, screwBoxesData[i].IsLocked);
        }

        // GameFaction[] screwBoxfactions = screwsData
        //     .Where(item => item != null)
        //     .Where(item => item.IsDone && !item.IsInScrewPort && !item.IsDestroyed)
        //     .Select(item => item.Faction).ToArray();

        // for (int i = 0; i < screwBoxfactions.Length; i++)
        // {
        //     screwBoxManager.SpawnScrewBox(screwBoxfactions[i]);
        // }

        // // No data for current level
        // if (screwBoxfactions.Length == 0)
        // {
        //     spawnFreshLevelScrewBoxesEvent?.Invoke();

        //     return;
        // }

        await Task.Delay(2000);

        // foreach (var id in _screwsWithId.Keys)
        // {
        //     BaseScrew screw = _screwsWithId[id];
        //     ScrewData screwData = screwsData.First(item => item.ScrewId == screw.ScrewId);

        //     if (screwData == null)
        //     {
        //         continue;
        //     }

        //     if (screwData.IsDestroyed)
        //     {
        //         screw.gameObject.SetActive(false);

        //         screw.IsDone = true;

        //         continue;
        //     }

        //     if (screwData.IsInScrewPort)
        //     {
        //         ScrewBoxSlot screwBoxSlot = screwBoxManager.CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: true);

        //         screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);
        //     }

        //     if (screwData.IsDone)
        //     {
        //         ScrewBoxSlot screwBoxSlot = screwBoxManager.CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: true);

        //         screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);
        //     }
        // }

        Dictionary<BaseScrew, ScrewData> screwsWithData = new Dictionary<BaseScrew, ScrewData>();

        for (int i = 0; i < _screws.Length; i++)
        {
            for (int j = 0; j < screwsData.Length; j++)
            {
                BaseScrew screw = _screws[i];
                ScrewData screwData = screwsData[j];

                if (screwData.ScrewId == screw.ScrewId)
                {
                    screwsWithData.Add(screw, screwData);
                }
            }
        }

        foreach (var screw in screwsWithData.Keys)
        {
            ScrewData screwData = screwsWithData[screw];

            if (screwData == null)
            {
                continue;
            }

            if (screwData.IsDestroyed)
            {
                screw.ForceUnscrew();

                continue;
            }

            if (screwData.IsInScrewPort)
            {
                ScrewBoxSlot screwBoxSlot = screwBoxManager.CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: true);

                screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);
            }

            if (screwData.IsDone)
            {
                ScrewBoxSlot screwBoxSlot = screwBoxManager.CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: true);

                screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);
            }
        }

        // for (int i = 0; i < screwsWithData.Keys.Count; i++)
        // {
        //     BaseScrew screw = _screws[i];
        //     ScrewData screwData = screwsData[i];

        //     if (screwData == null)
        //     {
        //         continue;
        //     }

        //     if (screwData.IsDestroyed)
        //     {
        //         screw.gameObject.SetActive(false);

        //         screw.IsDone = true;

        //         continue;
        //     }

        //     if (screwData.IsInScrewPort)
        //     {
        //         ScrewBoxSlot screwBoxSlot = screwBoxManager.CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: true);

        //         screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);

        //         // for (int j = 0; j < screwBoxManager.ScrewPorts.Count; j++)
        //         // {
        //         //     ScrewBoxSlot screwBoxSlot = screwBoxManager.ScrewPorts[j];

        //         //     if (!screwBoxSlot.IsFilled)
        //         //     {
        //         //         screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);

        //         //         break;
        //         //     }
        //         // }
        //     }

        //     if (screwData.IsDone)
        //     {
        //         ScrewBoxSlot screwBoxSlot = screwBoxManager.CheckAvailableScrewBoxes(screw.Faction, isIncludeScrewPorts: true);

        //         screw.Loose(screw.ScrewId, screw.Faction, screwBoxSlot);
        //     }
        // }
    }

    private ScrewBoxData[] GetScrewBoxesData()
    {
        ScrewBoxData[] screwBoxesData = new ScrewBoxData[screwBoxManager.ScrewBoxs.Length];

        for (int i = 0; i < screwBoxManager.ScrewBoxs.Length; i++)
        {
            screwBoxesData[i] = new ScrewBoxData();

            if (screwBoxManager.ScrewBoxs[i] == null)
            {
                continue;
            }

            screwBoxesData[i].Faction = screwBoxManager.ScrewBoxs[i].Faction;
            screwBoxesData[i].IsLocked = screwBoxManager.ScrewBoxs[i].IsLocked;
        }

        return screwBoxesData;
    }

    private void SaveLevelData()
    {
        ScrewData[] screwsData = new ScrewData[_screws.Length];

        // foreach (var id in _screwsWithId.Keys)
        // {
        //     ScrewData screwData = new ScrewData();

        //     screwData.ScrewId = _screwsWithId[id].ScrewId;
        //     screwData.Faction = _screwsWithId[id].Faction;
        //     screwData.IsDone = _screwsWithId[id].IsDone;
        //     screwData.IsInScrewPort = _screwsWithId[id].IsInScrewPort;

        //     if (!_screwsWithId[id].gameObject.activeSelf)
        //     {
        //         screwData.IsDestroyed = true;
        //     }

        //     screwsData.Add(id, screwData);
        // }

        for (int i = 0; i < _screws.Length; i++)
        {
            if (_screws[i] == null)
            {
                break;
            }

            screwsData[i] = new ScrewData();

            screwsData[i].ScrewId = _screws[i].ScrewId;
            screwsData[i].Faction = _screws[i].Faction;
            screwsData[i].IsDone = _screws[i].IsDone;
            screwsData[i].IsInScrewPort = _screws[i].IsInScrewPort;

            if (!_screws[i].gameObject.activeSelf)
            {
                screwsData[i].IsDestroyed = true;
            }
        }

        ScrewBoxData[] screwBoxesData = GetScrewBoxesData();

        LevelProgressData levelProgressData = new LevelProgressData();

        levelProgressData.ScrewsData = screwsData;
        levelProgressData.ScrewBoxesData = screwBoxesData;

        DataUtility.SaveAsync($"{GameConstants.SCREWS_DATA}_{currentLevel.Value}", levelProgressData);
    }

    private void OnAllLevelScrewsObservered(BaseScrew[] screws)
    {
        _screws = screws;

        LoadLevelData();
    }
}
