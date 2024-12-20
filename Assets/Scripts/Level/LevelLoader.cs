using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelLoader : MonoBehaviour
{
    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private IntVariable currentLevel;

    private void Awake()
    {
        DebugPopup.toLevelEvent += GoLevel;

        LoadLevel();
    }

    void OnDestroy()
    {
        DebugPopup.toLevelEvent -= GoLevel;
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

    private void GoLevel(int level)
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);

        currentLevel.Value = level;

        LoadLevel();
    }
}
