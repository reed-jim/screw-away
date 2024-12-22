using System.Threading.Tasks;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Image progressBarFill;

    [SerializeField] private LeanLocalizedTextMeshProUGUI localizedDescriptionText;

    #region CACHE
    private float _cachedRequirementValue;
    private float _cachedCurrentValue;
    #endregion

    public RectTransform Container
    {
        get => container;
    }

    private async void OnEnable()
    {
        await Task.Delay(500);

        SetDescription();
    }

    public void SetDescription()
    {
        localizedDescriptionText.UpdateTranslationWithParameter(
            LeanLocalization.GetTranslation(localizedDescriptionText.TranslationName), "task_requirement_value", _cachedRequirementValue.ToString());
    }

    public void SetProgress(float current, float requirement)
    {
        float progress = current / requirement;

        progressText.text = $"{current}/{requirement}";

        progressBarFill.fillAmount = progress;

        _cachedCurrentValue = current;
        _cachedRequirementValue = requirement;
    }
}
