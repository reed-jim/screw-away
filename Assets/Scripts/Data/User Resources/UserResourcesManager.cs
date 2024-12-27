using System;
using UnityEngine;

public class UserResourcesManager : MonoBehaviour
{
    [SerializeField] private UserResourcesObserver userResourcesObserver;

    #region EVENT
    public static event Action<float> updateCoinTextEvent;
    #endregion

    void Awake()
    {
        RewardCollectingUI.addCoinEvent += AddCoin;
    }

    void OnDestroy()
    {
        RewardCollectingUI.addCoinEvent -= AddCoin;
    }

    private void AddCoin(float value)
    {
        userResourcesObserver.CoinValue += value;

        updateCoinTextEvent?.Invoke(userResourcesObserver.CoinValue);
    }
}
