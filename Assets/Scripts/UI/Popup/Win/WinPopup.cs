using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class WinPopup : BasePopup
{
    [SerializeField] private Button continueButton;

    protected override void RegisterMoreEvent()
    {
        ScrewManager.winLevelEvent += Show;

        continueButton.onClick.AddListener(Continue);
    }

    protected override void UnregisterMoreEvent()
    {
        ScrewManager.winLevelEvent -= Show;
    }

    private void Continue()
    {
        Addressables.LoadSceneAsync(GameConstants.MENU_SCENE);
    }
}
