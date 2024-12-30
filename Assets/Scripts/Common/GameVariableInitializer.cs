using UnityEngine;

public class GameVariableInitializer : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;

    [SerializeField] private Vector2Variable canvasSize;
    [SerializeField] private Vector2Variable canvasScale;

    [SerializeField] private IntVariable currentLevel;
    [SerializeField] private UserResourcesObserver userResourcesObserver;

    private void Awake()
    {
        canvasSize.Value = canvas.sizeDelta;

        currentLevel.Load();

        userResourcesObserver.Load();
    }

    private void Start()
    {
        canvasScale.Value = canvas.localScale;
    }
}
