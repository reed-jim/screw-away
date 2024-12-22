using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LosePopup : BasePopup
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button returnHomeButton;

    public static event Action replayLevelEvent;

    protected override void RegisterMoreEvent()
    {
        ScrewBoxManager.loseLevelEvent += Show;

        replayButton.onClick.AddListener(Replay);
        returnHomeButton.onClick.AddListener(ReturnHome);
    }

    protected override void UnregisterMoreEvent()
    {
        ScrewBoxManager.loseLevelEvent -= Show;
    }

    private void Replay()
    {
        replayLevelEvent?.Invoke();

        Hide();
    }

    private void ReturnHome()
    {
        Addressables.LoadSceneAsync(GameConstants.MENU_SCENE);
    }
}
