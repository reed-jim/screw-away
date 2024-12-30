using System;
using System.Threading.Tasks;
using Lean.Localization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static GameEnum;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button openRemoveAdPopupButton;
    [SerializeField] private Button luckyWheelButton;
    [SerializeField] private Button weeklyTaskButton;
    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedLevelText;

    [SerializeField] private IntVariable currentLevel;

    public static event Action<ScreenRoute> switchRouteEvent;

    private void Awake()
    {
        startGameButton.onClick.AddListener(StartGame);
        openRemoveAdPopupButton.onClick.AddListener(OpenRemoveAdPopup);
        luckyWheelButton.onClick.AddListener(OpenLuckyWheelScreen);
        weeklyTaskButton.onClick.AddListener(OpenWeeklyTaskScreen);

        localizedLevelText.textTranslatedEvent += OnLevelTextTranslated;
    }

    void OnDestroy()
    {
        localizedLevelText.textTranslatedEvent -= OnLevelTextTranslated;
    }

    private void OnLevelTextTranslated()
    {
        localizedLevelText.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{currentLevel.Value}");
    }

    private void StartGame()
    {
        PlayClickSound();

        Addressables.LoadSceneAsync(GameConstants.GAMEPLAY_SCENE);
    }

    private void OpenRemoveAdPopup()
    {
        PlayClickSound();

        switchRouteEvent?.Invoke(ScreenRoute.RemoveAd);
    }

    private void OpenLuckyWheelScreen()
    {
        PlayClickSound();

        switchRouteEvent?.Invoke(ScreenRoute.LuckyWheel);
    }

    private void OpenWeeklyTaskScreen()
    {
        PlayClickSound();

        switchRouteEvent?.Invoke(ScreenRoute.WeeklyTask);
    }

    private void PlayClickSound()
    {
        AudioSource clickSound = ObjectPoolingEverything.GetFromPool<AudioSource>(GameConstants.CLICK_SOUND);

        clickSound.Play();
    }
}
