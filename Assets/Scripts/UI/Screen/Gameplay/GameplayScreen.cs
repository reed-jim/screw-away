using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : MonoBehaviour
{
    [SerializeField] private Button openDebugPopupButton;

    public static event Action openDebugPopupEvent;

    void Awake()
    {
        openDebugPopupButton.onClick.AddListener(OpenDebugPopupButton);
    }

    private void OpenDebugPopupButton()
    {
        openDebugPopupEvent?.Invoke();
    }
}
