using TMPro;
using UnityEngine;

public class TaskItemUI : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private TMP_Text taskNameText;
    [SerializeField] private TMP_Text progressText;

    public RectTransform Container
    {
        get => container;
    }

    public void SetProgress(float current, float requirement)
    {
        progressText.text = $"{current}/{requirement}";
    }
}
