using UnityEngine;

public class ObjectPoolingEverythingInitializer : MonoBehaviour
{
    [SerializeField] private GameObject vehicleEngineSoundPrefab;
    [SerializeField] private GameObject hitObstacleSoundPrefab;
    [SerializeField] private GameObject getInVehicleSoundPrefab;
    [SerializeField] private GameObject vehicleMoveOutSoundPrefab;
    [SerializeField] private GameObject screwBox;
    [SerializeField] private GameObject screwPortPrefab;
    [SerializeField] private GameObject fakeScrewPrefab;
    [SerializeField] private GameObject loosenScrewSoundPrefab;
    [SerializeField] private GameObject loosenScrewFailSoundPrefab;

    [SerializeField] private GameObject taskItemUIPrefab;

    private bool _isInitialized;

    private void Start()
    {
        if (!_isInitialized)
        {
            InitPool();

            _isInitialized = true;
        }
    }

    private void InitPool()
    {
        // RegisterPool<AudioSource>(GameConstants.VEHICLE_ENGINE_SOUND, vehicleEngineSoundPrefab, 3);
        // RegisterPool<AudioSource>(GameConstants.HIT_OBSTACLE_SOUND, hitObstacleSoundPrefab, 3);
        // RegisterPool<AudioSource>(GameConstants.GET_IN_VEHICLE_SOUND, getInVehicleSoundPrefab, 3);
        // RegisterPool<AudioSource>(GameConstants.VEHICLE_MOVE_OUT_SOUND, vehicleMoveOutSoundPrefab, 3);
        RegisterPool<ScrewBox>(GameConstants.SCREW_BOX, screwBox, 5);
        RegisterPool<ScrewBoxSlot>(GameConstants.SCREW_PORT_SLOT, screwPortPrefab, 3);
        RegisterPool<FakeScrew>(GameConstants.FAKE_SCREW, fakeScrewPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.LOOSEN_SCREW_SOUND, loosenScrewSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.LOOSEN_SCREW_FAIL_SOUND, loosenScrewFailSoundPrefab, 3);
        RegisterPool<TaskItemUI>(GameConstants.TASK_ITEM_UI, taskItemUIPrefab, 5);
    }

    private void RegisterPool<T>(string key, GameObject prefab, int poolSize)
    {
        ObjectPool pool = new ObjectPool();

        pool.Init<T>(prefab, poolSize);

        ObjectPoolingEverything.AddToPool(key, pool);
    }
}
