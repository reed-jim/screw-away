using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelLoader : MonoBehaviour
{
    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private IntVariable currentLevel;

    public static event Action startLevelEvent;

    private void Awake()
    {
        LosePopup.replayLevelEvent += Replay;
        DebugPopup.toLevelEvent += GoLevel;
        GameplayScreen.nextLevelEvent += NextLevel;
        GameplayScreen.prevLevelEvent += PrevLevel;

        GoLevel(currentLevel.Value);
    }

    void OnDestroy()
    {
        LosePopup.replayLevelEvent -= Replay;
        DebugPopup.toLevelEvent -= GoLevel;
        GameplayScreen.nextLevelEvent -= NextLevel;
        GameplayScreen.prevLevelEvent -= PrevLevel;
    }

    private void LoadLevel()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>($"Level {currentLevel.Value}");

        handle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject loadedObject = Instantiate(op.Result, transform);
            }
        };
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
}
