using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : BasePopup
{
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
        turnMusicToggle.onValueChanged.AddListener(SettingGameMusic);
        turnSoundToggle.onValueChanged.AddListener(SettingGameSound);
    }

    private void SettingGameMusic(bool isTurnOn)
    {
        gameSetting.IsTurnOnBackgroundMusic = isTurnOn;

        enableBackgroundMusicEvent?.Invoke(isTurnOn);
    }

    private void SettingGameSound(bool isTurnOn)
    {
        gameSetting.IsTurnOnSound = isTurnOn;

        enableGameSoundEvent?.Invoke(isTurnOn);
    }
}
