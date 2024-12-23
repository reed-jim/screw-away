using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PausePopup : BasePopup
{
    [SerializeField] private Button returnHomeButton;
    [SerializeField] private Toggle turnMusicToggle;
    [SerializeField] private Toggle turnSoundToggle;

    [SerializeField] private GameSetting gameSetting;

    public static event Action<bool> enableBackgroundMusicEvent;
    public static event Action<bool> enableGameSoundEvent;

    protected override void MoreActionInAwake()
    {
        turnMusicToggle.isOn = gameSetting.IsTurnOnBackgroundMusic;
        turnSoundToggle.isOn = gameSetting.IsTurnOnSound;
    }

    protected override void RegisterMoreEvent()
    {
        returnHomeButton.onClick.AddListener(ReturnHome);
        turnMusicToggle.onValueChanged.AddListener(EnableGameMusic);
        turnSoundToggle.onValueChanged.AddListener(EnableGameSound);
    }

    protected override void AfterHide()
    {
        Time.timeScale = 1;
    }

    private void ReturnHome()
    {
        Time.timeScale = 1;

        Addressables.LoadSceneAsync(GameConstants.MENU_SCENE);
    }

    private void EnableGameMusic(bool isTurnOn)
    {
        gameSetting.IsTurnOnBackgroundMusic = isTurnOn;

        enableBackgroundMusicEvent?.Invoke(isTurnOn);
    }

    private void EnableGameSound(bool isTurnOn)
    {
        gameSetting.IsTurnOnSound = isTurnOn;

        enableGameSoundEvent?.Invoke(isTurnOn);
    }
}
