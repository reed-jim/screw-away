using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static GameEnum;

public class GameplayScreen : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button openDebugPopupButton;
    [SerializeField] private Button debugNextLevelButton;
    [SerializeField] private Button debugPrevLevelButton;

    public static event Action<ScreenRoute> switchRouteEvent;
    public static event Action nextLevelEvent;
    public static event Action prevLevelEvent;

    void Awake()
    {
        pauseButton.onClick.AddListener(Pause);
        openDebugPopupButton.onClick.AddListener(OpenDebugPopupButton);
        debugNextLevelButton.onClick.AddListener(NextLevel);
        debugPrevLevelButton.onClick.AddListener(PrevLevel);
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

    private void NextLevel()
    {
        nextLevelEvent?.Invoke();
    }

    private void PrevLevel()
    {
        prevLevelEvent?.Invoke();
    }
}
