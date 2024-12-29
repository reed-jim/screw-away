using UnityEngine;

public class ObjectPoolingEverythingInitializer : MonoBehaviour
{
    public static ObjectPoolingEverythingInitializer Instance;

    [SerializeField] private GameObject vehicleEngineSoundPrefab;
    [SerializeField] private GameObject hitObstacleSoundPrefab;
    [SerializeField] private GameObject getInVehicleSoundPrefab;
    [SerializeField] private GameObject vehicleMoveOutSoundPrefab;
    [SerializeField] private GameObject screwBox;
    [SerializeField] private GameObject screwPortPrefab;
    [SerializeField] private GameObject fakeScrewPrefab;
    [SerializeField] private GameObject hammerPrefab;
    [SerializeField] private GameObject loosenScrewSoundPrefab;
    [SerializeField] private GameObject loosenScrewFailSoundPrefab;
    [SerializeField] private GameObject screwBoxDoneSoundPrefab;
    [SerializeField] private GameObject breakObjectSoundPrefab;
    [SerializeField] private GameObject winSoundPrefab;
    [SerializeField] private GameObject loseSoundPrefab;
    [SerializeField] private GameObject destroyObjectPartFxPrefab;

    [SerializeField] private GameObject taskItemUIPrefab;

    private bool _isInitialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

            _isInitialized = true;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (!_isInitialized)
        {
            InitPool();
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
        RegisterPool<Transform>(GameConstants.HAMMER, hammerPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.LOOSEN_SCREW_SOUND, loosenScrewSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.LOOSEN_SCREW_FAIL_SOUND, loosenScrewFailSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.SCREW_BOX_DONE_SOUND, screwBoxDoneSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.BREAK_OBJECT_SOUND, breakObjectSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.WIN_SOUND, winSoundPrefab, 3);
        RegisterPool<AudioSource>(GameConstants.LOSE_SOUND, loseSoundPrefab, 3);
        RegisterPool<TaskItemUI>(GameConstants.TASK_ITEM_UI, taskItemUIPrefab, 5);
        RegisterPool<ParticleSystem>(GameConstants.DESTROY_OBJECT_PART_FX, destroyObjectPartFxPrefab, 5);
    }

    private void RegisterPool<T>(string key, GameObject prefab, int poolSize)
    {
        ObjectPool pool = new ObjectPool();

        pool.Init<T>(prefab, poolSize);

        ObjectPoolingEverything.AddToPool(key, pool);
    }
}
