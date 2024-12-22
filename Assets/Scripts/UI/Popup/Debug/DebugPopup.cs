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
        debugPlayLevelButton.onClick.AddListener(DebugPlayLevel);
    }

    private void DebugPlayLevel()
    {
        int level = int.Parse(levelInput.text);

        toLevelEvent?.Invoke(level);

        Hide();
    }
}
