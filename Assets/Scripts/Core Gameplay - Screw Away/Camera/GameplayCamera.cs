using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
    [SerializeField] private Camera gameplayCamera;

    private float _targetOrthographicSize;

    void Awake()
    {
        PinchGesture.pinchGestureEvent += Zoom;

        _targetOrthographicSize = gameplayCamera.orthographicSize;
    }

    void OnDestroy()
    {
        PinchGesture.pinchGestureEvent -= Zoom;
    }

    void Update()
    {
        gameplayCamera.orthographicSize = Mathf.Lerp(gameplayCamera.orthographicSize, _targetOrthographicSize, 0.333f);
    }

    private void Zoom(float orthographicSize)
    {
        _targetOrthographicSize = orthographicSize;
    }
}
