using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Saferio.Util.SaferioTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform progressFillRT;
    [SerializeField] private RectTransform loadingTextRT;
    [SerializeField] private Image fadeBackground;
    [SerializeField] private Image progressFill;
    [SerializeField] private TMP_Text loadingText;

    [Header("CUSTOMIZE")]
    [SerializeField] private float startDelay;
    [SerializeField] private float fadeOutDuration;

    private Vector2 _canvasSize;
    private List<Tween> _tweens;


    private void Awake()
    {
        _tweens = new List<Tween>();

        _canvasSize = canvas.sizeDelta;

        Transition();
    }

    private void OnDestroy()
    {
        CommonUtil.StopAllTweens(_tweens);
    }

    private void Transition()
    {
        _tweens.Add(Tween.Custom(0, 1, duration: 0.95f * startDelay, onValueChange: newVal =>
        {
            progressFill.fillAmount = newVal;

            loadingText.text = $"{(int)(newVal * 100)}%";
        }));

        SaferioTween.DelayAsync(startDelay, onCompletedAction: (() =>
        {
            SaferioTween.LocalPositionAsync(progressFillRT, new Vector3(0, -_canvasSize.y), duration: fadeOutDuration);
            SaferioTween.AlphaAsync(fadeBackground, 0, duration: fadeOutDuration, onCompletedAction: () =>
            {
                container.gameObject.SetActive(false);
            });
        }));
    }
}
