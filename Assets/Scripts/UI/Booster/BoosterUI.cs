using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUI : MonoBehaviour
{
    [SerializeField] private RectTransform addMoreScrewPortButtonRT;

    [SerializeField] private Button addMoreScrewPortButton;
    [SerializeField] private Button breakObjectButton;
    [SerializeField] private Button clearAllScrewPortsButton;

    public static event Action addMoreScrewPortEvent;
    public static event Action enableBreakObjectModeEvent;
    public static event Action clearAllScrewPortsEvent;

    private int _numScrewPortsAdded;

    private void Awake()
    {
        addMoreScrewPortButton.onClick.AddListener(AddMoreScrewPort);
        breakObjectButton.onClick.AddListener(EnableBreakObjectMode);
        clearAllScrewPortsButton.onClick.AddListener(ClearAllScrewPorts);
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
        enableBreakObjectModeEvent?.Invoke();
    }

    private void ClearAllScrewPorts()
    {
        clearAllScrewPortsEvent?.Invoke();
    }
}
