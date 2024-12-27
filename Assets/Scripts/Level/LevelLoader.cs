using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static GameEnum;

public class LevelLoader : MonoBehaviour
{
    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private IntVariable currentLevel;

    public static event Action startLevelEvent;
    public static event Action<int> setLevelScrewNumberEvent;
    public static event Action showSceneTransitionEvent;

    private void Awake()
    {
        PausePopup.replayLevelEvent += Replay;
        LosePopup.replayLevelEvent += Replay;
        DebugPopup.toLevelEvent += GoLevel;
        GameplayScreen.nextLevelEvent += NextLevel;
        GameplayScreen.prevLevelEvent += PrevLevel;
        WinPopup.nextLevelEvent += NextLevel;
        WinPopup.goLevelEvent += GoLevel;

        GoLevel(currentLevel.Value);
    }

    void OnDestroy()
    {
        PausePopup.replayLevelEvent -= Replay;
        LosePopup.replayLevelEvent -= Replay;
        DebugPopup.toLevelEvent -= GoLevel;
        GameplayScreen.nextLevelEvent -= NextLevel;
        GameplayScreen.prevLevelEvent -= PrevLevel;
        WinPopup.nextLevelEvent -= NextLevel;
        WinPopup.goLevelEvent -= GoLevel;
    }

    private void LoadLevel()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>($"Level {currentLevel.Value}");

        handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject level = Instantiate(op.Result, transform);

                AutoAssignScrewFaction(level);
            }
        };

        showSceneTransitionEvent?.Invoke();
    }

    private async void GoLevel(int level)
    {
        await Task.Delay(500);

        if (gameObject.transform.childCount > 0)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }

        currentLevel.Value = level;

        startLevelEvent?.Invoke();

        LoadLevel();
    }

    private void NextLevel()
    {
        GoLevel(currentLevel.Value + 1);
    }

    private void PrevLevel()
    {
        GoLevel(currentLevel.Value - 1);
    }

    private void Replay()
    {
        GoLevel(currentLevel.Value);
    }

    private void AutoAssignScrewFaction(GameObject level)
    {
        BaseScrew[] screws = TransformUtil.GetComponentsFromAllChildren<BaseScrew>(level.transform).ToArray();

        GameFaction[] factions = new GameFaction[5] { GameFaction.Blue, GameFaction.Red, GameFaction.Green, GameFaction.Purple, GameFaction.Orange };

        int currentFaction = 0;

        List<GameFaction> remainingFactionForScrews = new List<GameFaction>();

        for (int i = 0; i < screws.Length; i++)
        {
            remainingFactionForScrews.Add(factions[currentFaction]);

            if (i > 0 && (i + 1) % 3 == 0)
            {
                currentFaction++;

                if (currentFaction >= factions.Length)
                {
                    currentFaction = 0;
                }
            }
        }

        for (int i = 0; i < screws.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, remainingFactionForScrews.Count);

            screws[i].ScrewId = i;
            screws[i].Faction = remainingFactionForScrews[randomIndex];

            screws[i].ScrewServiceLocator.screwFaction.SetColorByFaction();

            remainingFactionForScrews.RemoveAt(randomIndex);
        }

        setLevelScrewNumberEvent?.Invoke(screws.Length);
    }
}
