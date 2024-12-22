using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class GameplayScreen : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button openDebugPopupButton;

    public static event Action<ScreenRoute> switchRouteEvent;

    void Awake()
    {
        pauseButton.onClick.AddListener(Pause);
        openDebugPopupButton.onClick.AddListener(OpenDebugPopupButton);
    }

    private async void Pause()
    {
        switchRouteEvent?.Invoke(ScreenRoute.Pause);

        await Task.Delay(1000);

        Time.timeScale = 0;
    }

    private void OpenDebugPopupButton()
    {
        switchRouteEvent?.Invoke(ScreenRoute.Debug);
    }
}
