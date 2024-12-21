using System;
using UnityEngine;

public class GameSettingManager : MonoBehaviour
{
    [SerializeField] private GameSetting gameSetting;

    public static event Action<bool> enableBackgroundMusicEvent;
    public static event Action<bool> enableGameSoundEvent;

    private void Awake()
    {
        SettingPopup.enableBackgroundMusicEvent += EnableBackgroundMusic;
        SettingPopup.enableGameSoundEvent += EnableGameSound;

        EnableBackgroundMusic(gameSetting.IsTurnOnBackgroundMusic);
        EnableGameSound(gameSetting.IsTurnOnSound);

        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        SettingPopup.enableBackgroundMusicEvent -= EnableBackgroundMusic;
        SettingPopup.enableGameSoundEvent -= EnableGameSound;
    }

    private void EnableBackgroundMusic(bool isEnable)
    {
        enableBackgroundMusicEvent?.Invoke(isEnable);
    }

    private void EnableGameSound(bool isEnable)
    {
        enableGameSoundEvent?.Invoke(isEnable);
    }
}
