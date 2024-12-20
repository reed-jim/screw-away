using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugPopup : BasePopup
{
    [SerializeField] private TMP_InputField levelInput;

    [SerializeField] private Button debugPlayLevelButton;

    public static event Action<int> toLevelEvent;

    protected override void RegisterMoreEvent()
    {
        GameplayScreen.openDebugPopupEvent += Show;

        debugPlayLevelButton.onClick.AddListener(DebugPlayLevel);
    }

    protected override void UnregisterMoreEvent()
    {
        GameplayScreen.openDebugPopupEvent -= Show;
    }

    private void DebugPlayLevel()
    {
        int level = int.Parse(levelInput.text);

        toLevelEvent?.Invoke(level);

        Hide();
    }
}
