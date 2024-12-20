using System;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUI : MonoBehaviour
{
    [SerializeField] private Button addMoreScrewPortButton;
    [SerializeField] private Button breakObjectButton;
    [SerializeField] private Button clearAllScrewPortsButton;

    public static event Action addMoreScrewPortEvent;
    public static event Action enableBreakObjectModeEvent;
    public static event Action clearAllScrewPortsEvent;

    private void Awake()
    {
        addMoreScrewPortButton.onClick.AddListener(AddMoreScrewPort);
        breakObjectButton.onClick.AddListener(EnableBreakObjectMode);
        clearAllScrewPortsButton.onClick.AddListener(ClearAllScrewPorts);
    }

    private void AddMoreScrewPort()
    {
        addMoreScrewPortEvent?.Invoke();
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
