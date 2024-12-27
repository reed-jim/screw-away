using PrimeTween;
using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
    [SerializeField] private Camera gameplayCamera;

    private float _targetOrthographicSize;

    void Awake()
    {
        PinchGesture.pinchGestureEvent += Zoom;
        BasicObjectPart.shakeCameraEvent += Shake;

        _targetOrthographicSize = gameplayCamera.orthographicSize;
    }

    void OnDestroy()
    {
        PinchGesture.pinchGestureEvent -= Zoom;
        BasicObjectPart.shakeCameraEvent -= Shake;
    }

    void Update()
    {
        gameplayCamera.orthographicSize = Mathf.Lerp(gameplayCamera.orthographicSize, _targetOrthographicSize, 0.333f);
    }

    private void Zoom(float orthographicSize)
    {
        _targetOrthographicSize = orthographicSize;
    }

    private void Shake()
    {
        Tween.ShakeCamera(gameplayCamera, 1, duration: 0.3f);
    }
}
