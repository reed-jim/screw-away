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
        ScrewManager.winLevelEvent += Show;

        continueButton.onClick.AddListener(Continue);
        returnHomeButton.onClick.AddListener(ReturnHome);
    }

    protected override void UnregisterMoreEvent()
    {
        ScrewManager.winLevelEvent -= Show;
    }

    private async void OnEnable()
    {
        await Task.Delay(500);

        SetLevelCompleted();
    }

    public void SetLevelCompleted()
    {
        localizedLevelCompleted.TranslationName = GameConstants.LEVEL_COMPLETED;

        localizedLevelCompleted.UpdateTranslationWithParameter(GameConstants.LEVEL_PARAMETER, $"{currentLevel.Value}");
    }

    private void Continue()
    {
        nextLevelEvent?.Invoke();
    }

    private void ReturnHome()
    {
        Addressables.LoadSceneAsync(GameConstants.MENU_SCENE);
    }
}
