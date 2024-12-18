using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScrewBoxUI : MonoBehaviour
{
    [SerializeField] private RectTransform unlockByAdsButtonRT;
    [SerializeField] private Button unlockByAdsButton;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private ScrewBoxCameraObserver screwBoxCameraObserver;
    [SerializeField] private Vector2Variable canvasSize;

    #region EVENT
    public static event Action<int> unlockScrewBox;
    #endregion

    async void Awake()
    {
        unlockByAdsButton.gameObject.SetActive(false);

        unlockByAdsButton.onClick.AddListener(Unlock);

        while (screwBoxCameraObserver.ScrewBoxCamera == null)
        {
            await Task.Delay(1000);
        }

        await Task.Delay(3000);

        unlockByAdsButtonRT.localPosition = screwBoxCameraObserver.ScrewBoxCamera.WorldToScreenPoint(transform.position) - 0.5f * (Vector3)canvasSize.Value;
    }

    public void Lock()
    {
        unlockByAdsButton.gameObject.SetActive(true);
    }

    private void Unlock()
    {
        unlockByAdsButton.gameObject.SetActive(false);
        
        unlockScrewBox?.Invoke(gameObject.GetInstanceID());
    }
}
