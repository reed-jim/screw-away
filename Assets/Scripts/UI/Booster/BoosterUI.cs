using System;
using UnityEngine;
using UnityEngine.UI;

public class BoosterUI : MonoBehaviour
{
    [SerializeField] private Button addMoreScrewPortButton;
    [SerializeField] private Button clearAllScrewPortsButton;

    public static event Action addMoreScrewPortEvent;
    public static event Action clearAllScrewPortsEvent;

    private void Awake()
    {
        addMoreScrewPortButton.onClick.AddListener(AddMoreScrewPort);
        clearAllScrewPortsButton.onClick.AddListener(ClearAllScrewPorts);
    }

    private void AddMoreScrewPort()
    {
        addMoreScrewPortEvent?.Invoke();
    }

    private void ClearAllScrewPorts()
    {
        clearAllScrewPortsEvent?.Invoke();
    }
}
