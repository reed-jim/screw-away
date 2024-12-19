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

    private void Awake()
    {
        ScrewBoxCameraManager.setCameraEvent += OnCameraSet;

        unlockByAdsButton.gameObject.SetActive(false);

        unlockByAdsButton.onClick.AddListener(Unlock);

        // while (screwBoxCameraObserver.ScrewBoxCamera == null)
        // {
        //     await Task.Delay(1000);
        // }

        // unlockByAdsButtonRT.localPosition = screwBoxCameraObserver.ScrewBoxCamera.WorldToScreenPoint(transform.position) - 0.5f * (Vector3)canvasSize.Value;

        // await Task.Delay(6000);

        // Debug.Log(screwBoxCameraObserver.ScrewBoxCamera.WorldToScreenPoint(transform.position) + "/" + canvasSize.Value);

        // unlockByAdsButtonRT.localPosition = screwBoxCameraObserver.ScrewBoxCamera.WorldToScreenPoint(transform.position) - 0.5f * (Vector3)canvasSize.Value;
    }

    void OnDestroy()
    {
        ScrewBoxCameraManager.setCameraEvent -= OnCameraSet;
    }

    private async void OnCameraSet(Camera camera)
    {
        await Task.Delay(1500);

        unlockByAdsButtonRT.localPosition = camera.WorldToScreenPoint(transform.position) - 0.5f * (Vector3)canvasSize.Value;
    }

    private async Task DelaySetPositionAsync()
    {
        while (screwBoxCameraObserver.ScrewBoxCamera == null)
        {
            Debug.Log("SAFERIO Awake " + screwBoxCameraObserver.ScrewBoxCamera);
            await Task.Delay(1000);
        }

        await Task.Delay(3000);

        Debug.Log(screwBoxCameraObserver.ScrewBoxCamera.WorldToScreenPoint(transform.position) + " SAFERIO/" + canvasSize.Value);

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
