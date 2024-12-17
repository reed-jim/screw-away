using UnityEngine;

public class GameplayCamera : MonoBehaviour
{
    [SerializeField] private Camera gameplayCamera;

    void Awake()
    {
        PinchGesture.pinchGestureEvent += Zoom;
    }

    void OnDestroy()
    {
        PinchGesture.pinchGestureEvent -= Zoom;
    }

    private void Zoom(float orthographicSize)
    {
        gameplayCamera.orthographicSize = orthographicSize;
    }
}
