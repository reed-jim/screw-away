using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private void Awake()
    {
        startGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        Addressables.LoadSceneAsync(GameConstants.GAMEPLAY_SCENE);
    }
}
