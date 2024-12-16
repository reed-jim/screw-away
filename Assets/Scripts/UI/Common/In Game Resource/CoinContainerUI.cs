using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinContainerUI : MonoBehaviour
{
    [SerializeField] private RectTransform iconRT;

    [SerializeField] private Vector2Variable canvasSize;
    [SerializeField] private Vector2Variable canvasScale;
    [SerializeField] private Button testCollectCoinButton;

    #region EVENT
    public static event Action<Vector3> collectCoinEvent;
    #endregion

    private void Awake()
    {
        SaferioIAPManager.iapProductPurchasedCompletedEvent += OnCoinPurchased;

        testCollectCoinButton.onClick.AddListener(OnCoinPurchased);
    }

    private void OnDestroy()
    {
        SaferioIAPManager.iapProductPurchasedCompletedEvent -= OnCoinPurchased;
    }

    private void OnCoinPurchased()
    {
        Vector2 positionRelativeToCanvas = ((Vector2)iconRT.position - 0.5f * canvasSize.Value) / canvasScale.Value.x;

        collectCoinEvent?.Invoke(positionRelativeToCanvas);
    }
}
