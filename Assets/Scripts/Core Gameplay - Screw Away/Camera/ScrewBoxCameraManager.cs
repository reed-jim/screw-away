using UnityEngine;

public class ScrewBoxCameraManager : MonoBehaviour
{
    [SerializeField] private Camera screwBoxCamera;

    [SerializeField] private ScrewBoxCameraObserver screwBoxCameraObserver;

    void Awake()
    {
        screwBoxCameraObserver.ScrewBoxCamera = screwBoxCamera;
    }
}
