using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static GameEnum;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button luckyWheelButton;

    public static event Action<ScreenRoute> switchRouteEvent;

    private void Awake()
    {
        startGameButton.onClick.AddListener(StartGame);
        luckyWheelButton.onClick.AddListener(OpenLuckyWheelScreen);
    }

    private void StartGame()
    {
        Addressables.LoadSceneAsync(GameConstants.GAMEPLAY_SCENE);
    }

    private void OpenLuckyWheelScreen()
    {
        switchRouteEvent?.Invoke(ScreenRoute.LuckyWheel);
    }
}
