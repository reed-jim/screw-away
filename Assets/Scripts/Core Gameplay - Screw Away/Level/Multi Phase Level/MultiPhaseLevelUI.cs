using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class MultiPhaseLevelUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Slider progressBar;

    [SerializeField] private RectTransform milestonePrefab;

    private RectTransform[] _milestones;

    private void Awake()
    {
        MultiPhaseLevelManager.updateUIEvent += UpdateMultiPhaseLevelUI;
    }

    private void OnDestroy()
    {
        MultiPhaseLevelManager.updateUIEvent -= UpdateMultiPhaseLevelUI;
    }

    private void UpdateMultiPhaseLevelUI(Dictionary<int, int> numberScrewByPhase, float progress)
    {
        if (_milestones == null)
        {
            _milestones = new RectTransform[numberScrewByPhase.Keys.Count];

            int totalScrew = numberScrewByPhase.Sum(item => item.Value);
            float currentNumberScrew = 0;

            for (int i = 0; i < _milestones.Length; i++)
            {
                _milestones[i] = Instantiate(milestonePrefab, container);

                Vector3 localPosition = Vector3.zero;

                currentNumberScrew += numberScrewByPhase[i];

                localPosition.x = -0.5f * container.sizeDelta.x + (currentNumberScrew / totalScrew) * container.sizeDelta.x;

                _milestones[i].localPosition = localPosition;
            }
        }

        Tween.Custom(progressBar.value, progress, duration: 0.3f, onValueChange: newVal =>
        {
            progressBar.value = newVal;
        });
    }
}
