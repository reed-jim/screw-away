using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class PopupFadeBackground : MonoBehaviour
{
    [SerializeField] private Image fadeBackground;

    [Header("CUSTOMIZE")]
    [SerializeField] private float transitionDuration;

    void Awake()
    {
        BasePopup.popupShowEvent += OnPopupShow;

        fadeBackground.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        BasePopup.popupShowEvent -= OnPopupClose;
    }

    private void OnPopupShow()
    {
        fadeBackground.gameObject.SetActive(true);

        Tween.Alpha(fadeBackground, 1, duration: transitionDuration);
    }

    private void OnPopupClose()
    {
        Tween.Alpha(fadeBackground, 0, duration: transitionDuration).OnComplete(() =>
        {
            fadeBackground.gameObject.SetActive(false);
        });
    }
}
