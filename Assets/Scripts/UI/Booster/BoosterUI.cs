using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUI : MonoBehaviour
{
    [SerializeField] private RectTransform boosterContainer;
    [SerializeField] private RectTransform addMoreScrewPortButtonRT;
    [SerializeField] private RectTransform breakModeContainer;

    [SerializeField] private Button addMoreScrewPortButton;
    [SerializeField] private Button breakObjectButton;
    [SerializeField] private Button clearAllScrewPortsButton;

    [SerializeField] private Vector2Variable canvasSize;

    [Header("CUSTOMIZE")]
    [SerializeField] private float transitionTime;

    public static event Action addMoreScrewPortEvent;
    public static event Action enableBreakObjectModeEvent;
    public static event Action clearAllScrewPortsEvent;

    private int _numScrewPortsAdded;
    private Vector2 _initialBoosterContainerPosition;

    private void Awake()
    {
        ScrewSelectionInput.breakObjectEvent += DisableBreakObjectMode;

        addMoreScrewPortButton.onClick.AddListener(AddMoreScrewPort);
        breakObjectButton.onClick.AddListener(EnableBreakObjectMode);
        clearAllScrewPortsButton.onClick.AddListener(ClearAllScrewPorts);

        _initialBoosterContainerPosition = boosterContainer.localPosition;
    }

    void OnDestroy()
    {
        ScrewSelectionInput.breakObjectEvent -= DisableBreakObjectMode;
    }

    private void AddMoreScrewPort()
    {
        addMoreScrewPortEvent?.Invoke();

        _numScrewPortsAdded++;

        if (_numScrewPortsAdded == 2)
        {
            addMoreScrewPortButton.interactable = false;

            Tween.LocalPositionY(addMoreScrewPortButtonRT, addMoreScrewPortButtonRT.localPosition.y - 600, duration: 0.3f);
        }
    }

    private void EnableBreakObjectMode()
    {
        breakModeContainer.gameObject.SetActive(true);

        Tween.LocalPositionY(boosterContainer, -canvasSize.Value.y, duration: transitionTime);
        Tween.LocalPositionY(breakModeContainer, _initialBoosterContainerPosition.y, duration: transitionTime)
        .OnComplete(() =>
        {
            enableBreakObjectModeEvent?.Invoke();
        });
    }

    private void DisableBreakObjectMode()
    {
        Tween.LocalPositionY(boosterContainer, _initialBoosterContainerPosition.y, duration: transitionTime);
        Tween.LocalPositionY(breakModeContainer, -canvasSize.Value.y, duration: transitionTime)
        .OnComplete(() =>
        {
            breakModeContainer.gameObject.SetActive(false);
        });
    }

    private void ClearAllScrewPorts()
    {
        clearAllScrewPortsEvent?.Invoke();
    }
}
