using UnityEngine;

public class GameVariableInitializer : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;

    [SerializeField] private Vector2Variable canvasSize;
    [SerializeField] private Vector2Variable canvasScale;

    private void Awake()
    {
        canvasSize.Value = canvas.sizeDelta;
    }

    private void Start()
    {
        canvasScale.Value = canvas.localScale;
    }
}
