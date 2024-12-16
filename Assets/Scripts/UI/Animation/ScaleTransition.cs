using PrimeTween;
using UnityEngine;

public class ScaleTransition : MonoBehaviour, ISaferioUIAnimation
{
    [SerializeField] private RectTransform target;

    [Header("SCRIPTABLE OBJECT")]
    [SerializeField] private Vector2Variable canvasSize;

    [Header("CUSTOMIZE")]
    [SerializeField] private float duration;

    public void Show()
    {
        target.gameObject.SetActive(true);

        target.localScale = Vector3.zero;

        Tween.Scale(target, 1, duration: duration);

        // SaferioTween.LocalPositionAsync(target, Vector2.zero, duration: duration);
    }

    public void Hide()
    {
        Tween.Scale(target, 0, duration: duration).OnComplete(() =>
        {
            target.gameObject.SetActive(false);
        });

        // SaferioTween.LocalPositionAsync(target, new Vector2(-canvasSize.Value.x, 0), duration: duration, onCompletedAction: () =>
        // {
        //     target.gameObject.SetActive(false);
        // });
    }
}
