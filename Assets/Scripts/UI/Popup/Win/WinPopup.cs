using System;
using System.Threading.Tasks;
using Lean.Localization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class WinPopup : BasePopup
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button returnHomeButton;
    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedLevelCompleted;

    [SerializeField] private IntVariable currentLevel;

    public static event Action nextLevelEvent;

    protected override void RegisterMoreEvent()
    {
        GameStateWin.winLevelEvent += OnLevelWinAsync;

        continueButton.onClick.AddListener(Continue);
        returnHomeButton.onClick.AddListener(ReturnHome);
    }

    protected override void UnregisterMoreEvent()
    {
        GameStateWin.winLevelEvent -= OnLevelWinAsync;
    }

    private async void OnLevelWinAsync()
    {
        Show();

        await Task.Delay(500);

        SetLevelCompleted();

        AudioSource winSound = ObjectPoolingEverything.GetFromPool<AudioSource>(GameConstants.WIN_SOUND);

        winSound.Play();
    }

    // private async void OnEnable()
    // {
    //     await Task.Delay(500);

    //     SetLevelCompleted();

    //     AudioSource winSound = ObjectPoolingEverything.GetFromPool<AudioSource>(GameConstants.WIN_SOUND);

    //     winSound.Play();
    // }

    public void SetLevelCompleted()
    {
        // localizedLevelCompleted.TranslationName = GameConstants.LEVEL_COMPLETED;

        localizedLevelCompleted.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{currentLevel.Value}");
    }

    private void Continue()
    {
        Hide();

        nextLevelEvent?.Invoke();
    }

    private void ReturnHome()
    {
        Addressables.LoadSceneAsync(GameConstants.MENU_SCENE);
    }
}
